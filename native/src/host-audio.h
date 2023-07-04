#pragma once

#include <stdbool.h>
#include <stdint.h>

#ifdef __cplusplus
namespace sonora::host {
#endif

#ifdef miniaudio_h
typedef ma_context *PMAContext;
typedef ma_device_info *PMADeviceInfo;
typedef ma_device *PMADevice;
#else
typedef void *PMAContext;
typedef void *PMADeviceInfo;
typedef void *PMADevice;
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
typedef uint32_t (*FnDeviceEnumeratorCount)(DeviceEnumerator *dev_enum,
                                            bool playback);
typedef char *(*FnDeviceEnumeratorGetDeviceName)(DeviceEnumerator *dev_enum,
                                                 bool playback,
                                                 uint32_t index);

typedef PMADevice (*FnDeviceNew)(PMAContext ctx, DeviceEnumerator *dev_enum,
                                 bool playback, uint32_t index,
                                 uint32_t channels, uint32_t sample_rate);
typedef void (*FnDeviceFree)(PMADevice device);
typedef void (*FnDeviceStart)(PMADevice device);
typedef void (*FnDeviceStop)(PMADevice device);

// Temp
typedef void (*FnDeviceSetOsc)(PMADevice device, int osc);
typedef void (*FnDeviceSetNote)(PMADevice device, uint8_t note);

#ifdef __cplusplus
extern "C" {
#endif

PMAContext SnrAudioContextNew();
void SnrAudioContextFree(PMAContext ctx);

DeviceEnumerator *SnrDeviceEnumeratorNew(PMAContext ctx);
void SnrDeviceEnumeratorFree(DeviceEnumerator *dev_enum);
uint32_t SnrDeviceEnumeratorCount(DeviceEnumerator *dev_enum, bool playback);
char *SnrDeviceEnumeratorGetDeviceName(DeviceEnumerator *dev_enum,
                                       bool playback, uint32_t index);

PMADevice SnrDeviceNew(PMAContext ctx, DeviceEnumerator *dev_enum,
                       bool playback, uint32_t index,
                       uint32_t channels, uint32_t sample_rate);
void SnrDeviceFree(PMADevice device);
void SnrDeviceStart(PMADevice device);
void SnrDeviceStop(PMADevice device);
void SnrDeviceSetOsc(PMADevice device, int osc);
void SnrDeviceSetNote(PMADevice device, uint8_t note);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora::host
#endif
