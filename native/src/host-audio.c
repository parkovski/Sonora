#include "miniaudio.h"
#include "host-audio.h"
#include <stdlib.h>

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

static void playback_callback(ma_device *device, void *out, const void *in,
                              uint32_t frames) {
  (void)device;
  (void)in;
  for (uint32_t i = 0; i < frames; ++i) {
    ((float*)out)[i] = (float)rand() / (float)RAND_MAX * 2 - 1;
  }
}

ma_device *SnrDeviceNew(PMAContext ctx, DeviceEnumerator *dev_enum,
                        bool playback, uint32_t index,
                        int format, uint32_t channels, uint32_t sample_rate) {
  ma_device_config config;
  if (playback) {
    config = ma_device_config_init(ma_device_type_playback);
    config.playback.pDeviceID = &dev_enum->playback_infos[index].id;
    config.playback.format = format;
    config.playback.channels = channels;
    config.sampleRate = sample_rate;
    config.dataCallback = playback_callback;
    config.pUserData = NULL;
  } else {
    config = ma_device_config_init(ma_device_type_capture);
    config.capture.pDeviceID = &dev_enum->capture_infos[index].id;
    config.capture.format = format;
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

  return device;
}

void SnrDeviceFree(PMADevice device) {
  ma_device_uninit(device);
  free(device);
}

void SnrDeviceStart(PMADevice device) {
  ma_device_start(device);
}

void SnrDeviceStop(PMADevice device) {
  ma_device_stop(device);
}
