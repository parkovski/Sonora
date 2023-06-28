// Must come before hostinterface.h for typedefs to work properly.
#include "miniaudio.h"
#include "hostinterface.h"

#include <stdlib.h>

ma_dr_wav *SnrWaveNewFromFile(const char *filename) {
  ma_dr_wav *wave = malloc(sizeof(ma_dr_wav));
  if (!ma_dr_wav_init_file(wave, filename, NULL)) {
    free(wave);
    return NULL;
  }
  return wave;
}

void SnrWaveFree(ma_dr_wav *wave) {
  free(wave);
}

uint64_t SnrWaveGetFrames(ma_dr_wav *wave) {
  return wave->totalPCMFrameCount;
}

uint32_t SnrWaveGetChannels(ma_dr_wav *wave) {
  return wave->channels;
}

uint64_t SnrWaveReadFrames(ma_dr_wav *wave, float *data, uint64_t size) {
  return ma_dr_wav_read_pcm_frames_f32(wave, size / wave->channels, data);
}
