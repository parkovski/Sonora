#include "sonora.h"
#include "log.h"
#include <spdlog/spdlog.h>
#include <spdlog/sinks/stdout_color_sinks.h>

#ifdef _WIN32
#define ATTACH_PARENT_PROCESS ((unsigned long)-1)
extern "C" int __stdcall AttachConsole(unsigned long dwProcessId);
#endif

namespace sonora::log {
  std::shared_ptr<spdlog::logger> g_outLogger;
  std::shared_ptr<spdlog::logger> g_errLogger;
}

using namespace sonora;

SNRAPI SNREXPORT void SnrStartup() {
  log::g_outLogger = spdlog::stdout_color_mt("sonora.out");
  log::g_errLogger = spdlog::stderr_color_mt("sonora.err");
  log::g_outLogger->set_level(spdlog::level::trace);
  log::g_errLogger->set_level(spdlog::level::warn);

#ifdef _WIN32
  AttachConsole(ATTACH_PARENT_PROCESS);
#endif

  log::info("Sonora.Native initialized.");
}

SNRAPI SNREXPORT void SnrShutdown() {
  log::info("Sonora.Native shutting down.");

  log::g_outLogger = nullptr;
  log::g_errLogger = nullptr;
  spdlog::shutdown();
}
