#include <new>
#include "hostinterface.h"

extern "C"
void
#ifdef _WIN32
__declspec(dllexport)
#else
__attribute__((visibility("default")))
#endif
GetHostInterface(sonora::host::HostInterface *host) {
  new (host) sonora::host::HostInterface(false);
}
