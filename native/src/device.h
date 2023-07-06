#pragma once

#include "audiocontext.h"
#include "deviceenumerator.h"
#include "instrument.h"

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
typedef Instrument *(*FnDeviceGetInstrument)(PMADevice device);

#ifdef __cplusplus
extern "C" {
#endif

PMADevice SnrDeviceNew(PMAContext ctx, DeviceEnumerator *dev_enum,
                       bool playback, uint32_t index,
                       uint32_t channels, uint32_t sample_rate);
void SnrDeviceFree(PMADevice device);
void SnrDeviceStart(PMADevice device);
void SnrDeviceStop(PMADevice device);
Instrument *SnrDeviceGetInstrument(PMADevice device);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora
#endif
