add_executable(test
  test.cpp
)

target_link_libraries(test PRIVATE
  sonoralib
  Catch2::Catch2WithMain
)

include(CTest)
include(Catch)
catch_discover_tests(test)
