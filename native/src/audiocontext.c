#include "miniaudio.h"
#include "audiocontext.h"
#include <stdlib.h>

ma_context *SnrAudioContextNew() {
  ma_context *ctx = malloc(sizeof(ma_context));
  if (ma_context_init(NULL, 0, NULL, ctx)) {
    free(ctx);
    return NULL;
  }
  return ctx;
}

void SnrAudioContextFree(ma_context *ctx) {
  ma_context_uninit(ctx);
  free(ctx);
}
