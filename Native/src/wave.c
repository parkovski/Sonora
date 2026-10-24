// Must come before hostinterface.h for typedefs to work properly.
#include "dr_wav.h"
#include "sonora.h"

#include <stdlib.h>
#include <stdint.h>

SNREXPORT drwav *SnrWaveNewFromFile(const char *filename) {
  drwav *wave = malloc(sizeof(drwav));
  if (!drwav_init_file(wave, filename, NULL)) {
    free(wave);
    return NULL;
  }
  return wave;
}

SNREXPORT void SnrWaveFree(drwav *wave) {
  free(wave);
}

SNREXPORT uint64_t SnrWaveGetFrames(drwav *wave) {
  return wave->totalPCMFrameCount;
}

SNREXPORT uint32_t SnrWaveGetChannels(drwav *wave) {
  return wave->channels;
}

SNREXPORT uint64_t SnrWaveReadFrames(drwav *wave, float *data, uint64_t size) {
  return drwav_read_pcm_frames_f32(wave, size / wave->channels, data);
}
