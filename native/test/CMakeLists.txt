add_executable(test
  dummy.cpp
)

target_link_libraries(test PRIVATE
  sonora-objects
  Catch2::Catch2WithMain
)

include(CTest)
include(Catch)
catch_discover_tests(test)

add_executable(midi midi.cpp)
target_link_libraries(midi PRIVATE sonora-objects PortMidi::portmidi)
