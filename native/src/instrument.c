#include "instrument.h"
#include <stdlib.h>
#include <string.h>

Instrument *SnrInstrumentNew(uint32_t channels, uint32_t sample_rate) {
  Instrument *i = malloc(sizeof(Instrument));
  i->channels = channels;
  i->sample_rate = sample_rate;
  memset(i->voices, 0, sizeof(i->voices));
  return i;
}

void SnrInstrumentFree(Instrument *instr) {
  free(instr);
}

void SnrInstrumentAddVoice(Instrument *instr, uint8_t note, float velocity) {
  uint32_t voices = sizeof(instr->voices) / sizeof(instr->voices[0]);
  Voice *voice = NULL;
  for (uint32_t i = 0; i < voices; ++i) {
    if (!instr->voices[i].active) {
      voice = &instr->voices[i];
      break;
    }
  }
  if (voice == NULL) {
    // Need to steal a voice.
    return;
  }

  voice->active = true;
  voice->osc.phase = 0;
  SnrVoiceSetPitch(voice, note, instr->sample_rate, 440.f);
  SnrVoiceSetAmplitude(voice, velocity);
}

void SnrInstrumentReleaseVoice(Instrument *instr, uint8_t note) {
  uint32_t voices = sizeof(instr->voices) / sizeof(instr->voices[0]);
  for (uint32_t i = 0; i < voices; ++i) {
    if (instr->voices[i].active && instr->voices[i].note == note) {
      instr->voices[i].active = false;
      break;
    }
  }
}

void SnrInstrumentGenerate(Instrument *instr, uint32_t count, float *frames) {
  uint32_t channels = instr->channels;
  uint32_t samples = count * channels;
  uint32_t voices = sizeof(instr->voices) / sizeof(instr->voices[0]);
  float sample;
  for (uint32_t i = 0; i < samples; i += channels) {
    sample = 0;
    for (uint32_t v = 0; v < voices; ++v) {
      if (instr->voices[v].active) {
        sample += SnrVoiceGetSample(&instr->voices[v]);
        SnrVoiceNextFrame(&instr->voices[v]);
      }
    }
    for (uint32_t j = 0; j < channels; ++j) {
      frames[i + j] = sample;
    }
  }
}
