set(SOURCES
  dr_libs.c
  miniaudio.c
  wave.c
)

add_executable(sonora host.cpp)
set_target_properties(sonora PROPERTIES OUTPUT_NAME Sonora)
target_link_directories(sonora PRIVATE
  "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x64/10.0.13/runtimes/win-x64/native"
)
target_link_libraries(sonora PRIVATE nethost)
file(COPY
  "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x64/10.0.13/runtimes/win-x64/native/nethost.dll"
  DESTINATION "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}"
)

add_library(sonoradll SHARED ${SOURCES})
target_compile_definitions(sonoradll PUBLIC -DSONORA_DLL)
set_target_properties(sonoradll PROPERTIES OUTPUT_NAME Sonora.Native)

if(CMAKE_SYSTEM_PROCESSOR MATCHES "[Aa][Mm][Dd]64" OR
    CMAKE_SYSTEM_PROCESSOR MATCHES "[Xx]86_64")
  set(DOTNET_ARCH x64)
elseif(CMAKE_SYSTEM_PROCESSOR MATCHES "[Aa][Rr][Mm]64" OR
    CMAKE_SYSTEM_PROCESSOR MATCHES "[Aa][Aa][Rr][Cc][Hh]64")
  set(DOTNET_ARCH arm64)
endif()

message(STATUS "Compiling for ${DOTNET_ARCH}.")

if(NOT DEFINED CMAKE_BUILD_TYPE OR CMAKE_BUILD_TYPE MATCHES "Deb")
  add_custom_command(TARGET sonoradll POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy
    "${CMAKE_BINARY_DIR}/bin/Sonora.Native.dll"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../Sonora/bin/Debug/net10.0")
  add_custom_command(TARGET sonoradll POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy
    "${CMAKE_BINARY_DIR}/bin/Sonora.Native.dll"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../Sonora/bin/${DOTNET_ARCH}/Debug/net10.0")
elseif(CMAKE_BUILD_TYPE MATCHES "Rel")
  add_custom_command(TARGET sonoradll POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy
    "${CMAKE_BINARY_DIR}/bin/Sonora.Native.dll"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../Sonora/bin/Release/net10.0")
  add_custom_command(TARGET sonoradll POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy
    "${CMAKE_BINARY_DIR}/bin/Sonora.Native.dll"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../Sonora/bin/${DOTNET_ARCH}/Release/net10.0")
endif()

