set(SOURCES
  audiocontext.c
  device.c
  deviceenumerator.c
  hostinterface.cpp
  miniaudio.c
  wave.c
)

add_library(sonora-objects OBJECT ${SOURCES})

add_executable(sonora host.cpp)
target_link_directories(sonora PRIVATE
  "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x64/7.0.7/runtimes/win-x64/native"
)
target_link_libraries(sonora PRIVATE
  sonora-objects
  nethost
)

if(CMAKE_BUILD_TYPE STREQUAL Debug)
  add_library(sonoradbg SHARED dll.cpp)
  target_link_libraries(sonoradbg PRIVATE sonora-objects)
endif()
