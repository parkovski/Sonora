#pragma once

#include <stdint.h>

#ifdef __cplusplus
namespace sonora {
#endif

enum {
  OscSine,
  OscSaw,
  OscSquare,
  OscTriangle,
};

typedef struct Oscillator {
  int mode;
  float freq;
  float amplitude;
  float phase;
  float phase_increment;
} Oscillator;

typedef void (*FnOscillatorUpdateIncrement)(Oscillator *osc,
                                            uint32_t sample_rate);
typedef float (*FnOscillatorGetNextSample)(Oscillator *osc);

#ifdef __cplusplus
extern "C" {
#endif

void SnrOscillatorUpdateIncrement(Oscillator *osc, uint32_t sample_rate);
float SnrOscillatorGetNextSample(Oscillator *osc);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora
#endif
