// Must come before hostinterface.h for typedefs to work properly.
#include "dr_libs/dr_wav.h"
#include "sonora.h"

#include <stdlib.h>
#include <stdint.h>

SNRAPI SNREXPORT drwav *SnrWaveNewFromFile(const char *filename) {
  drwav *wave = new drwav;
  if (!drwav_init_file(wave, filename, nullptr)) {
    free(wave);
    return nullptr;
  }
  return wave;
}

SNRAPI SNREXPORT void SnrWaveFree(drwav *wave) {
  delete wave;
}

SNRAPI SNREXPORT uint64_t SnrWaveGetFrames(drwav *wave) {
  return wave->totalPCMFrameCount;
}

SNRAPI SNREXPORT uint32_t SnrWaveGetChannels(drwav *wave) {
  return wave->channels;
}

SNRAPI SNREXPORT uint64_t
SnrWaveReadFrames(drwav *wave, float *data, uint64_t size) {
  return drwav_read_pcm_frames_f32(wave, size / wave->channels, data);
}
