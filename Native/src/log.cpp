#include "sonora.h"
#include "log.h"

using namespace sonora;

SNRAPI SNREXPORT void SnrLogTrace(const char *s) {
  log::trace("{}", s);
}

SNRAPI SNREXPORT void SnrLogDebug(const char *s) {
  log::debug("{}", s);
}

SNRAPI SNREXPORT void SnrLogInfo(const char *s) {
  log::info("{}", s);
}

SNRAPI SNREXPORT void SnrLogWarn(const char *s) {
  log::warn("{}", s);
}

SNRAPI SNREXPORT void SnrLogError(const char *s) {
  log::error("{}", s);
}

SNRAPI SNREXPORT void SnrLogCritical(const char *s) {
  log::critical("{}", s);
}
