add_executable(test
  dummy.cpp
)

target_link_libraries(test PRIVATE
  Catch2::Catch2WithMain
)

include(CTest)
include(Catch)
catch_discover_tests(test)
