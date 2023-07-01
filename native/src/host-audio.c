#include "miniaudio.h"
#include "host-audio.h"
#include <stdlib.h>
#include <stdio.h>

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

char *SnrDeviceGetName(DeviceEnumerator *dev_enum, bool playback, uint32_t index) {
  printf("SnrDeviceGetName(0x%zX, %s, %u)\n",
         (size_t)dev_enum, playback?"true":"false", index);
  if (playback) {
    printf("name: %s\n", dev_enum->playback_infos[index].name);
    return dev_enum->playback_infos[index].name;
  } else {
    return dev_enum->capture_infos[index].name;
  }
}
