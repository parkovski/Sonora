#pragma once

#include "audiocontext.h"
#include "deviceenumerator.h"

#ifdef __cplusplus
namespace sonora {
#endif

#ifdef miniaudio_h
typedef ma_device *PMADevice;
#else
typedef void *PMADevice;
#endif

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
} // namespace sonora
#endif
