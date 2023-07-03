#include "miniaudio.h"
#include "host-audio.h"
#include <stdlib.h>
#include <stdio.h>
#include <math.h>

ma_context *SnrAudioContextNew() {
  ma_context *ctx = malloc(sizeof(ma_context));
  if (ma_context_init(NULL, 0, NULL, ctx)) {
    free(ctx);
    return NULL;
  }
  return ctx;
}

void SnrAudioContextFree(ma_context *ctx) {
  ma_context_uninit(ctx);
  free(ctx);
}

DeviceEnumerator *SnrDeviceEnumeratorNew(PMAContext ctx) {
  DeviceEnumerator *e = malloc(sizeof(DeviceEnumerator));
  if (ma_context_get_devices(ctx, &e->playback_infos, &e->playback_count,
                             &e->capture_infos, &e->capture_count) !=
      MA_SUCCESS) {
    free(e);
    return NULL;
  }
  return e;
}

void SnrDeviceEnumeratorFree(DeviceEnumerator *dev_enum) {
  // Don't need to free device infos because MA keeps track of them.
  free(dev_enum);
}

uint32_t SnrDeviceEnumeratorCount(DeviceEnumerator *dev_enum, bool playback) {
  if (playback) {
    return dev_enum->playback_count;
  } else {
    return dev_enum->capture_count;
  }
}

char *SnrDeviceEnumeratorGetDeviceName(DeviceEnumerator *dev_enum,
                                       bool playback,
                                       uint32_t index) {
  if (playback) {
    return dev_enum->playback_infos[index].name;
  } else {
    return dev_enum->capture_infos[index].name;
  }
}

static const float Tau = 6.28318530717959f;
struct Osc {
  enum {
    OscSine,
    OscSaw,
    OscSquare,
    OscTriangle,
  } mode;
  float freq;
  float phase;
  uint32_t channels;
  uint32_t sample_rate;
  float phase_increment;
};

static void OscUpdateIncrement(struct Osc *osc) {
  osc->phase_increment = osc->freq * Tau / osc->sample_rate;
}

static struct Osc *OscNew(float freq, uint32_t sample_rate) {
  struct Osc *osc = malloc(sizeof(struct Osc));
  osc->mode = OscSine;
  osc->freq = freq;
  osc->phase = 0;
  osc->channels = 2;
  osc->sample_rate = sample_rate;
  OscUpdateIncrement(osc);
  return osc;
}

static void OscGenerate(struct Osc *osc, float *buf, uint32_t frames) {
  uint32_t channels = osc->channels;

#define BEGIN \
  for (uint32_t i = 0; i < frames; ++i) {
#define END \
    for (uint32_t j = 0; j < channels; ++j) { \
      buf[i*channels+j] = value; \
    } \
    osc->phase += osc->phase_increment; \
    while (osc->phase >= Tau) { \
      osc->phase -= Tau; \
    } \
  }

  switch (osc->mode) {
  case OscSine:
    BEGIN
      float value = sinf(osc->phase);
    END
    break;

  case OscSaw:
    BEGIN
      float value = 1.0f - 2.0f * osc->phase / Tau;
    END
    break;

  case OscSquare:
    BEGIN
      float value = osc->phase <= 3.14159f ? 1.0f : -1.0f;
    END
    break;

  case OscTriangle:
    BEGIN
      float value = 2.0f * (fabsf(-1.0f + 2.0f * osc->phase / Tau) - 0.5f);
    END
    break;
  }

#undef BEGIN
#undef END
}

static void playback_callback(ma_device *device, void *out, const void *in,
                              uint32_t frames) {
  (void)in;
  struct Osc *osc = device->pUserData;
  OscGenerate(osc, out, frames);
}

void SnrDeviceSetOsc(ma_device *device, int osc) {
  if (osc < 0) osc = 0;
  if (osc > 3) osc = 3;
  ((struct Osc *)device->pUserData)->mode = osc;
}

#if 0
struct SineWave {
  float *samples;
  size_t sample_count;
  size_t offset;
};

static void playback_callback(ma_device *device, void *out, const void *in,
                              uint32_t frames) {
  (void)in;
  struct SineWave *wave = device->pUserData;
  size_t offset = wave->offset;
  uint32_t channels = device->playback.channels;
  for (uint32_t i = 0; i < frames; ++i) {
    if (offset >= wave->sample_count) {
      offset = 0;
    }
    for (uint32_t j = 0; j < channels; ++j) {
      ((float*)out)[i * channels + j] = wave->samples[offset];
      ++offset;
    }
  }
  wave->offset = offset;
}

static struct SineWave *init_sine_wave(uint32_t channels, uint32_t sample_rate) {
  size_t sample_count = channels * sample_rate / 440;
  struct SineWave *wave = malloc(sizeof(struct SineWave) + sizeof(float) * sample_count);
  float *samples = wave->samples = (float*)((char*)wave + sizeof(struct SineWave));
  wave->sample_count = sample_count;
  wave->offset = 0;
  for (size_t i = 0; i < sample_count; i += channels) {
    float value = sinf(2 * 3.14159f * (float)i / sample_count);
    for (size_t j = 0; j < channels; ++j) {
      samples[i + j] = value;
    }
  }
  return wave;
}
#endif

ma_device *SnrDeviceNew(PMAContext ctx, DeviceEnumerator *dev_enum,
                        bool playback, uint32_t index,
                        uint32_t channels, uint32_t sample_rate) {
  ma_device_config config;
  if (playback) {
    config = ma_device_config_init(ma_device_type_playback);
    config.playback.pDeviceID = &dev_enum->playback_infos[index].id;
    config.playback.format = ma_format_f32;
    config.playback.channels = channels;
    config.sampleRate = sample_rate;
    config.dataCallback = playback_callback;
    config.pUserData = OscNew(220, 48000);
  } else {
    config = ma_device_config_init(ma_device_type_capture);
    config.capture.pDeviceID = &dev_enum->capture_infos[index].id;
    config.capture.format = ma_format_f32;
    config.capture.channels = channels;
    config.sampleRate = sample_rate;
    config.dataCallback = NULL;
    config.pUserData = NULL;
  }

  ma_device *device = malloc(sizeof(ma_device));
  if (ma_device_init(ctx, &config, device) != MA_SUCCESS) {
    free(device);
    return NULL;
  }
  printf("sample rate is %d\n", device->sampleRate);

  return device;
}

void SnrDeviceFree(PMADevice device) {
  if (device->pUserData) {
    free(device->pUserData);
  }
  ma_device_uninit(device);
  free(device);
}

void SnrDeviceStart(PMADevice device) {
  ma_device_start(device);
}

void SnrDeviceStop(PMADevice device) {
  ma_device_stop(device);
}
