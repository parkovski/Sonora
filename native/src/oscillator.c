#include "oscillator.h"
#include <math.h>

static const float Pi = 3.14159265358979f;
static const float Tau = 6.28318530717959f;
void SnrOscillatorUpdateIncrement(Oscillator *osc, uint32_t sample_rate) {
  osc->phase_increment = osc->freq * Tau / (float)sample_rate;
}

float SnrOscillatorGetNextSample(Oscillator *osc) {
  float value;
  switch (osc->mode) {
  case OscSine:
    value = sinf(osc->phase);
    break;

  case OscSaw:
    value = 1.f - 2.f * osc->phase / Tau;
    break;

  case OscSquare:
    value = osc->phase <= Pi ? 1.f : -1.f;
    break;

  case OscTriangle:
    value = 2.f * (fabsf(-1.f + 2.f * osc->phase / Tau) - 0.5f);
    break;

  default:
    value = 0;
    break;
  }

  osc->phase += osc->phase_increment;
  while (osc->phase >= Tau) {
    osc->phase -= Tau;
  }

  return value;
}
