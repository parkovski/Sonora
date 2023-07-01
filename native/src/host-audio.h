#pragma once

#include <stdbool.h>
#include <stdint.h>

#ifdef __cplusplus
namespace sonora::host {
#endif

#ifdef miniaudio_h
typedef ma_context *PMAContext;
typedef ma_device_info *PMADeviceInfo;
#else
typedef void *PMAContext;
typedef void *PMADeviceInfo;
#endif

typedef struct DeviceEnumerator {
  uint32_t playback_count;
  uint32_t capture_count;
  PMADeviceInfo playback_infos;
  PMADeviceInfo capture_infos;
} DeviceEnumerator;

typedef PMAContext (*FnAudioContextNew)();
typedef void (*FnAudioContextFree)(PMAContext ctx);

typedef DeviceEnumerator *(*FnDeviceEnumeratorNew)(PMAContext ctx);
typedef void (*FnDeviceEnumeratorFree)(DeviceEnumerator *dev_enum);
typedef uint32_t (*FnDeviceEnumeratorCount)(DeviceEnumerator *dev_enum, bool playback);
typedef char *(*FnDeviceGetName)(DeviceEnumerator *dev_enum, bool playback, uint32_t index);

#ifdef __cplusplus
extern "C" {
#endif

PMAContext SnrAudioContextNew();
void SnrAudioContextFree(PMAContext ctx);

DeviceEnumerator *SnrDeviceEnumeratorNew(PMAContext ctx);
void SnrDeviceEnumeratorFree(DeviceEnumerator *dev_enum);
uint32_t SnrDeviceEnumeratorCount(DeviceEnumerator *dev_enum, bool playback);
char *SnrDeviceGetName(DeviceEnumerator *dev_enum, bool playback, uint32_t index);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora::host
#endif