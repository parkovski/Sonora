#pragma once

#include <spdlog/spdlog.h>
#include <spdlog/logger.h>
#ifdef _WIN32
# include <fmt/xchar.h>
#endif

namespace sonora::log {

extern std::shared_ptr<spdlog::logger> g_outLogger;
extern std::shared_ptr<spdlog::logger> g_errLogger;

/// Print to stdout with no prefix.
template<typename... Args>
static inline void print(fmt::format_string<Args...> fmt, Args &&...args)
{ fmt::print(stdout, fmt, std::forward<Args>(args)...); }

/// Print to stderr with no prefix.
template<typename... Args>
static inline void eprint(fmt::format_string<Args...> fmt, Args &&...args)
{ fmt::print(stderr, fmt, std::forward<Args>(args)...); }

/// Trace logger.
template<typename... Args>
static inline void trace(fmt::format_string<Args...> fmt, Args &&...args)
{ g_outLogger->trace(fmt, std::forward<Args>(args)...); }

/// Debug logger.
template<typename... Args>
static inline void debug(fmt::format_string<Args...> fmt, Args &&...args)
{ g_outLogger->debug(fmt, std::forward<Args>(args)...); }

/// Info logger.
template<typename... Args>
static inline void info(fmt::format_string<Args...> fmt, Args &&...args)
{ g_outLogger->info(fmt, std::forward<Args>(args)...); }

/// Warning logger.
template<typename... Args>
static inline void warn(fmt::format_string<Args...> fmt, Args &&...args)
{ g_errLogger->warn(fmt, std::forward<Args>(args)...); }

/// Error logger.
template<typename... Args>
static inline void error(fmt::format_string<Args...> fmt, Args &&...args)
{ g_errLogger->error(fmt, std::forward<Args>(args)...); }

/// Critical logger.
template<typename... Args>
static inline void critical(fmt::format_string<Args...> fmt, Args &&...args)
{ g_errLogger->critical(fmt, std::forward<Args>(args)...); }

#ifdef _WIN32

/// Print to stdout with no prefix (wchar_t version).
template<typename... Args>
static inline void print(fmt::wformat_string<Args...> fmt, Args &&...args)
{ fmt::print(stdout, fmt, std::forward<Args>(args)...); }

/// Print to stderr with no prefix (wchar_t version).
template<typename... Args>
static inline void eprint(fmt::wformat_string<Args...> fmt, Args &&...args)
{ fmt::print(stderr, fmt, std::forward<Args>(args)...); }

/// Trace logger (wchar_t version).
template<typename... Args>
static inline void trace(fmt::wformat_string<Args...> fmt, Args &&...args)
{ g_outLogger->trace(fmt, std::forward<Args>(args)...); }

/// Debug logger (wchar_t version).
template<typename... Args>
static inline void debug(fmt::wformat_string<Args...> fmt, Args &&...args)
{ g_outLogger->debug(fmt, std::forward<Args>(args)...); }

/// Info logger (wchar_t version).
template<typename... Args>
static inline void info(fmt::wformat_string<Args...> fmt, Args &&...args)
{ g_outLogger->info(fmt, std::forward<Args>(args)...); }

/// Warning logger (wchar_t version).
template<typename... Args>
static inline void warn(fmt::wformat_string<Args...> fmt, Args &&...args)
{ g_errLogger->warn(fmt, std::forward<Args>(args)...); }

/// Error logger (wchar_t version).
template<typename... Args>
static inline void error(fmt::wformat_string<Args...> fmt, Args &&...args)
{ g_errLogger->error(fmt, std::forward<Args>(args)...); }

/// Critical logger (wchar_t version).
template<typename... Args>
static inline void critical(fmt::wformat_string<Args...> fmt, Args &&...args)
{ g_errLogger->critical(fmt, std::forward<Args>(args)...); }

#endif // _WIN32

} // namespace snr::log
