set(SOURCES
  host.cpp
)

add_executable(sonora ${SOURCES})
target_link_directories(sonora PRIVATE "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x64/7.0.7/runtimes/win-x64/native")
target_link_libraries(sonora PRIVATE nethost)
