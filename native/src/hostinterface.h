#pragma once

#include "audiocontext.h"
#include "deviceenumerator.h"
#include "device.h"
#include "wave.h"

#ifdef __cplusplus
namespace sonora {
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
  FnDeviceGetInstrument DeviceGetInstrument;

  FnInstrumentNew InstrumentNew;
  FnInstrumentFree InstrumentFree;
  FnInstrumentAddVoice InstrumentAddVoice;
  FnInstrumentReleaseVoice InstrumentReleaseVoice;
  FnInstrumentGenerate InstrumentGenerate;
} HostInterface;

#ifdef __cplusplus
} // namespace sonora
#endif
