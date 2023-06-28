#include "hostinterface.h"

#include <cstdio>

namespace sonora::host {

void SnrHello() {
  printf("Hello from C++\n");
}

HostInterface::HostInterface()
  : Hello{SnrHello}
  , WaveNewFromFile{SnrWaveNewFromFile}
  , WaveFree{SnrWaveFree}
  , WaveGetFrames{SnrWaveGetFrames}
  , WaveGetChannels{SnrWaveGetChannels}
  , WaveReadFrames{SnrWaveReadFrames}
{}

} // namespace sonora::host
