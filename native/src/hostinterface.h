#pragma once

#include <stdint.h>
#include <stdbool.h>

#ifdef ma_dr_wav_h
typedef ma_dr_wav *PDrWav;
#else
typedef void *PDrWav;
#endif

#ifdef __cplusplus
namespace sonora::host {
#endif

typedef bool (*FnIsHosted)();
typedef PDrWav (*FnWaveNewFromFile)(const char *filename);
typedef void (*FnWaveFree)(PDrWav wave);
typedef uint64_t (*FnWaveGetFrames)(PDrWav wave);
typedef uint32_t (*FnWaveGetChannels)(PDrWav wave);
typedef uint64_t (*FnWaveReadFrames)(PDrWav wave, float *data, uint64_t size);

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
} HostInterface;

#if 0
struct HostedMainArgs {
    int argc;
    const char_t *const *argv;
    HostInterface host;
};
#endif

#ifdef __cplusplus
} // namespace sonora::host

extern "C" {
#endif

PDrWav SnrWaveNewFromFile(const char *filename);
void SnrWaveFree(PDrWav wave);
uint64_t SnrWaveGetFrames(PDrWav wave);
uint32_t SnrWaveGetChannels(PDrWav wave);
uint64_t SnrWaveReadFrames(PDrWav wave, float *data, uint64_t size);

#ifdef __cplusplus
} // extern "C"
#endif
