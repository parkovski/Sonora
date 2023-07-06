#include "voice.h"
#include <math.h>

static const float Tau = 6.28318530717959f;
void SnrVoiceSetPitch(Voice *voice, uint8_t note, uint32_t sample_rate,
                      float fundamental) {
  voice->note = note;
  voice->osc.freq = fundamental * powf(2, ((int8_t)note - 69) / 12.f);
  SnrOscillatorUpdateIncrement(&voice->osc, sample_rate);
}

void SnrVoiceSetAmplitude(Voice *voice, float amplitude) {
  voice->osc.amplitude = amplitude;
}

float SnrVoiceGetSample(Voice *voice) {
  return sinf(voice->osc.phase) * voice->osc.amplitude;
}

void SnrVoiceNextFrame(Voice *voice) {
  voice->osc.phase += voice->osc.phase_increment;
  while (voice->osc.phase >= Tau) {
    voice->osc.phase -= Tau;
  }
}
