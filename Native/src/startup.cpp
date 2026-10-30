#include "sonora.h"
#include "log.h"
#include <spdlog/spdlog.h>
#include <spdlog/sinks/stdout_color_sinks.h>

#ifdef _WIN32
#include <winrt/base.h>
#define STD_INPUT_HANDLE ((unsigned long)-10)
#define STD_OUTPUT_HANDLE ((unsigned long)-11)
#define STD_ERROR_HANDLE ((unsigned long)-12)
#define FILE_TYPE_CHAR 2
extern "C" void * __stdcall GetStdHandle(unsigned long n);
extern "C" unsigned long __stdcall GetFileType(void *handle);
#else
#define STDIN_FILENO 0
#define STDOUT_FILENO 1
#define STDERR_FILENO 2
extern "C" int isatty(int fd);
#endif

namespace sonora::log {
  std::shared_ptr<spdlog::logger> g_outLogger;
  std::shared_ptr<spdlog::logger> g_errLogger;
}

using namespace sonora;

SNRAPI SNREXPORT void SnrStartup() {
#ifdef _WIN32
  winrt::init_apartment();
#endif

  log::g_outLogger = spdlog::stdout_color_mt("sonora.out");
  log::g_errLogger = spdlog::stderr_color_mt("sonora.err");
  log::g_outLogger->set_level(spdlog::level::trace);
  log::g_errLogger->set_level(spdlog::level::warn);

  if (
#ifdef _WIN32
    GetFileType(GetStdHandle(STD_OUTPUT_HANDLE)) == FILE_TYPE_CHAR
#else
    isatty(STDOUT_FILENO)
#endif
  ) {
    log::g_outLogger->set_pattern("[%H:%M:%S.%e] %^%l:%$ %v");
  } else {
    log::g_outLogger->set_pattern("[%H:%M:%S.%e] %l: %v");
  }

  if (
#ifdef _WIN32
    GetFileType(GetStdHandle(STD_ERROR_HANDLE)) == FILE_TYPE_CHAR
#else
    isatty(STDERR_FILENO)
#endif
  ) {
    log::g_errLogger->set_pattern("[%H:%M:%S.%e] %^%l:%$ %v");
  } else {
    log::g_errLogger->set_pattern("[%H:%M:%S.%e] %l: %v");
  }

  log::info("Sonora.Native initialized.");
}

SNRAPI SNREXPORT void SnrShutdown() {
  log::info("Sonora.Native shutting down.");

  log::g_outLogger = nullptr;
  log::g_errLogger = nullptr;
  spdlog::shutdown();
}
