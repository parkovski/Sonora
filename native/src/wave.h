#pragma once

#include <stdint.h>

#ifdef __cplusplus
namespace sonora {
#endif

#ifdef ma_dr_wav_h
typedef ma_dr_wav *PDRWav;
#else
typedef void *PDRWav;
#endif

typedef PDRWav (*FnWaveNewFromFile)(const char *filename);
typedef void (*FnWaveFree)(PDRWav wave);
typedef uint64_t (*FnWaveGetFrames)(PDRWav wave);
typedef uint32_t (*FnWaveGetChannels)(PDRWav wave);
typedef uint64_t (*FnWaveReadFrames)(PDRWav wave, float *data, uint64_t size);

#ifdef __cplusplus
extern "C" {
#endif

PDRWav SnrWaveNewFromFile(const char *filename);
void SnrWaveFree(PDRWav wave);
uint64_t SnrWaveGetFrames(PDRWav wave);
uint32_t SnrWaveGetChannels(PDRWav wave);
uint64_t SnrWaveReadFrames(PDRWav wave, float *data, uint64_t size);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora
#endif
