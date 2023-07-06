#pragma once

#include "voice.h"

#ifdef __cplusplus
namespace sonora {
#endif

typedef struct Instrument {
  uint32_t channels;
  uint32_t sample_rate;
  Voice voices[16];
} Instrument;

typedef Instrument *(*FnInstrumentNew)(uint32_t channels,
                                       uint32_t sample_rate);
typedef void (*FnInstrumentFree)(Instrument *instr);
typedef void (*FnInstrumentAddVoice)(Instrument *instr, uint8_t note,
                                     float velocity);
typedef void (*FnInstrumentReleaseVoice)(Instrument *instr, uint8_t note);
typedef void (*FnInstrumentGenerate)(Instrument *instr, uint32_t count,
                                     float *frames);

#ifdef __cplusplus
extern "C" {
#endif

Instrument *SnrInstrumentNew(uint32_t channels, uint32_t sample_rate);
void SnrInstrumentFree(Instrument *instr);
void SnrInstrumentAddVoice(Instrument *instr, uint8_t note, float velocity);
void SnrInstrumentReleaseVoice(Instrument *instr, uint8_t note);
void SnrInstrumentGenerate(Instrument *instr, uint32_t count, float *frames);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora
#endif
