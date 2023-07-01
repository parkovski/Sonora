set(SOURCES
  hostinterface.cpp
  host-audio.c
  host-wave.c
  miniaudio.c
)

add_executable(sonora host.cpp ${SOURCES})
target_link_directories(sonora PRIVATE "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x64/7.0.7/runtimes/win-x64/native")
target_link_libraries(sonora PRIVATE nethost)

if(CMAKE_BUILD_TYPE STREQUAL Debug)
  add_library(sonoradbg SHARED dll.cpp ${SOURCES})
endif()
