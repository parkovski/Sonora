#pragma once

#include <stdint.h>
#include <stdbool.h>
#include "oscillator.h"

#ifdef __cplusplus
namespace sonora {
#endif

typedef struct Voice {
  Oscillator osc;
  bool active;
  uint8_t note;
} Voice;

typedef void (*FnVoiceSetPitch)(Voice *voice, uint8_t note);
typedef void (*FnVoiceSetAmplitude)(Voice *voice, float velocity);
typedef float (*FnVoiceGetSample)(Voice *voice);
typedef void (*FnVoiceNextFrame)(Voice *voice);

#ifdef __cplusplus
extern "C" {
#endif

void SnrVoiceSetPitch(Voice *voice, uint8_t note, uint32_t sample_rate,
                      float fundamental);
void SnrVoiceSetAmplitude(Voice *voice, float amplitude);
float SnrVoiceGetSample(Voice *voice);
void SnrVoiceNextFrame(Voice *voice);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora
#endif
