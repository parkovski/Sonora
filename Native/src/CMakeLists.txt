set(SOURCES
  drlibs.c
  log.cpp
  midi.cpp
  miniaudio.c
  startup.cpp
  wave.cpp
)

if(CMAKE_SYSTEM_PROCESSOR MATCHES "^[Aa][Mm][Dd]64$" OR
    CMAKE_SYSTEM_PROCESSOR MATCHES "^[Xx]86_64$")
  set(DOTNET_ARCH x64)
elseif(CMAKE_SYSTEM_PROCESSOR MATCHES "^[Aa][Rr][Mm]64$" OR
    CMAKE_SYSTEM_PROCESSOR MATCHES "^[Aa][Aa][Rr][Cc][Hh]64$")
  set(DOTNET_ARCH arm64)
endif()

message(STATUS "Compiling for ${DOTNET_ARCH}")

add_executable(sonorahost nethost/host.cpp)
set_target_properties(sonorahost PROPERTIES OUTPUT_NAME Sonora.Host)
if(WIN32)
  set(dotnet_nethost_dll "nethost.dll")
  set(sonora_native_dll "Sonora.Native.dll")
  if(DOTNET_ARCH STREQUAL x64)
    set(dotnet_glob "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x64/*/runtimes/win-x64/native")
  elseif(DOTNET_ARCH STREQUAL arm64)
    set(dotnet_glob "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-arm64/*/runtimes/win-arm64/native")
  else()
    message(FATAL_ERROR "Unsupported architecture")
  endif()
elseif(UNIX AND NOT APPLE)
  set(dotnet_nethost_dll "libnethost.so")
  set(sonora_native_dll "libSonora.Native.so")
  if(DOTNET_ARCH STREQUAL x64)
    set(dotnet_glob "/usr/share/dotnet/packs/Microsoft.NETCore.App.Host.arch-x64/*/runtimes/arch-x64/native")
  elseif(DOTNET_ARCH STREQUAL arm64)
    set(dotnet_glob "/usr/share/dotnet/packs/Microsoft.NETCore.App.Host.arch-arm64/*/runtimes/arch-arm64/native")
  else()
    message(FATAL_ERROR "Fill in .NET paths here")
  endif()
elseif(APPLE)
  set(dotnet_nethost_dll "libnethost.dylib")
  set(sonora_native_dll "libSonora.Native.dylib")
  message(FATAL_ERROR "Fill in .NET paths here")
endif()
file(GLOB dotnet_link_dir LIST_DIRECTORIES TRUE ${dotnet_glob})

if(dotnet_link_dir)
  message(STATUS "Found .NET runtime in \"${dotnet_link_dir}\"")
else()
  message(FATAL_ERROR "Could not find .NET runtime")
endif()
target_link_directories(sonorahost PRIVATE ${dotnet_link_dir})
target_link_libraries(sonorahost PRIVATE nethost)
file(COPY "${dotnet_link_dir}/${dotnet_nethost_dll}"
  DESTINATION "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}"
)

add_library(sonoradll SHARED ${SOURCES})
set_target_properties(sonoradll PROPERTIES OUTPUT_NAME Sonora.Native)
target_compile_definitions(sonoradll PRIVATE SONORA_DLL SPDLOG_FMT_EXTERNAL)
target_include_directories(sonoradll PRIVATE
  ${DRLIBS_INCLUDE_DIRS} ${MINIAUDIO_INCLUDE_DIRS})
target_link_libraries(sonoradll PRIVATE
  fmt::fmt
  spdlog::spdlog
  libremidi
)

if(NOT DEFINED CMAKE_BUILD_TYPE OR CMAKE_BUILD_TYPE MATCHES "Deb")
  add_custom_command(TARGET sonoradll POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy
    "${CMAKE_BINARY_DIR}/bin/${sonora_native_dll}"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../Sonora/bin/Debug/net10.0")
elseif(CMAKE_BUILD_TYPE MATCHES "Rel")
  add_custom_command(TARGET sonoradll POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy
    "${CMAKE_BINARY_DIR}/bin/${sonora_native_dll}"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../Sonora/bin/Release/net10.0")
endif()
