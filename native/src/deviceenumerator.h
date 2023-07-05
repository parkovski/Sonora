#pragma once

#include "audiocontext.h"
#include <stdint.h>
#include <stdbool.h>

#ifdef __cplusplus
namespace sonora {
#endif

#ifdef miniaudio_h
typedef ma_device_info *PMADeviceInfo;
#else
typedef void *PMADeviceInfo;
#endif

typedef struct DeviceEnumerator {
  uint32_t playback_count;
  uint32_t capture_count;
  PMADeviceInfo playback_infos;
  PMADeviceInfo capture_infos;
} DeviceEnumerator;

typedef DeviceEnumerator *(*FnDeviceEnumeratorNew)(PMAContext ctx);
typedef void (*FnDeviceEnumeratorFree)(DeviceEnumerator *dev_enum);
typedef uint32_t (*FnDeviceEnumeratorCount)(DeviceEnumerator *dev_enum,
                                            bool playback);
typedef const char *(*FnDeviceEnumeratorGetDeviceName)(
  DeviceEnumerator *dev_enum, bool playback, uint32_t index);

#ifdef __cplusplus
extern "C" {
#endif

DeviceEnumerator *SnrDeviceEnumeratorNew(PMAContext ctx);
void SnrDeviceEnumeratorFree(DeviceEnumerator *dev_enum);
uint32_t SnrDeviceEnumeratorCount(DeviceEnumerator *dev_enum, bool playback);
const char *
SnrDeviceEnumeratorGetDeviceName(DeviceEnumerator *dev_enum,
                                 bool playback, uint32_t index);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora
#endif
