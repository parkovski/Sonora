#include "miniaudio.h"
#include "device.h"
#include <stdlib.h>
#include <stdio.h>
#include <math.h>

static void playback_callback(ma_device *device, void *out, const void *in,
                              uint32_t frames) {
  (void)in;
  SnrInstrumentGenerate(device->pUserData, frames, out);
}

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
    config.pUserData = SnrInstrumentNew(channels, sample_rate);
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
    SnrInstrumentFree(device->pUserData);
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

Instrument *SnrDeviceGetInstrument(PMADevice device) {
  return device->pUserData;
}
