#pragma once

#include "host-wave.h"
#include "host-audio.h"
#include <stdbool.h>

#ifdef __cplusplus
namespace sonora::host {
#endif

typedef bool (*FnIsHosted)();

typedef struct HostInterface {
#ifdef __cplusplus
  HostInterface(bool isHosted);
#endif

  FnIsHosted IsHosted;

  FnWaveNewFromFile WaveNewFromFile;
  FnWaveFree WaveFree;
  FnWaveGetFrames WaveGetFrames;
  FnWaveGetChannels WaveGetChannels;
  FnWaveReadFrames WaveReadFrames;

  FnAudioContextNew AudioContextNew;
  FnAudioContextFree AudioContextFree;

  FnDeviceEnumeratorNew DeviceEnumeratorNew;
  FnDeviceEnumeratorFree DeviceEnumeratorFree;
  FnDeviceEnumeratorCount DeviceEnumeratorCount;
  FnDeviceEnumeratorGetDeviceName DeviceEnumeratorGetDeviceName;

  FnDeviceNew DeviceNew;
  FnDeviceFree DeviceFree;
  FnDeviceStart DeviceStart;
  FnDeviceStop DeviceStop;
  FnDeviceSetOsc DeviceSetOsc;
} HostInterface;

#ifdef __cplusplus
} // namespace sonora::host
#endif
