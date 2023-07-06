#include "hostinterface.h"

#include <cstdio>

namespace sonora {

bool SnrIsHosted() {
  return true;
}

bool SnrIsNotHosted() {
  return false;
}

HostInterface::HostInterface(bool isHosted)
  : IsHosted{isHosted ? SnrIsHosted : SnrIsNotHosted}
  , WaveNewFromFile{SnrWaveNewFromFile}
  , WaveFree{SnrWaveFree}
  , WaveGetFrames{SnrWaveGetFrames}
  , WaveGetChannels{SnrWaveGetChannels}
  , WaveReadFrames{SnrWaveReadFrames}
  , AudioContextNew{SnrAudioContextNew}
  , AudioContextFree{SnrAudioContextFree}
  , DeviceEnumeratorNew{SnrDeviceEnumeratorNew}
  , DeviceEnumeratorFree{SnrDeviceEnumeratorFree}
  , DeviceEnumeratorCount{SnrDeviceEnumeratorCount}
  , DeviceEnumeratorGetDeviceName{SnrDeviceEnumeratorGetDeviceName}
  , DeviceNew{SnrDeviceNew}
  , DeviceFree{SnrDeviceFree}
  , DeviceStart{SnrDeviceStart}
  , DeviceStop{SnrDeviceStop}
  , DeviceGetInstrument{SnrDeviceGetInstrument}
  , InstrumentNew{SnrInstrumentNew}
  , InstrumentFree{SnrInstrumentFree}
  , InstrumentAddVoice{SnrInstrumentAddVoice}
  , InstrumentReleaseVoice{SnrInstrumentReleaseVoice}
  , InstrumentGenerate{SnrInstrumentGenerate}
{}

} // namespace sonora
