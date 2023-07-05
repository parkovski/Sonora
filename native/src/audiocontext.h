#pragma once

#ifdef __cplusplus
namespace sonora {
#endif

#ifdef miniaudio_h
typedef ma_context *PMAContext;
#else
typedef void *PMAContext;
#endif

typedef PMAContext (*FnAudioContextNew)();
typedef void (*FnAudioContextFree)(PMAContext ctx);

#ifdef __cplusplus
extern "C" {
#endif

PMAContext SnrAudioContextNew();
void SnrAudioContextFree(PMAContext ctx);

#ifdef __cplusplus
} // extern "C"
} // namespace sonora
#endif
