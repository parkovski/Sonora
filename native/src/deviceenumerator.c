#include "miniaudio.h"
#include "deviceenumerator.h"
#include <stdlib.h>

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

const char *
SnrDeviceEnumeratorGetDeviceName(DeviceEnumerator *dev_enum,
                                 bool playback, uint32_t index) {
  if (playback) {
    return dev_enum->playback_infos[index].name;
  } else {
    return dev_enum->capture_infos[index].name;
  }
}
