cmake_minimum_required(VERSION 3.13)
project(SesamoEngine)

set(CMAKE_CXX_STANDARD 17)

set(Dependencies/GLFW/glfw-3.2.1_DIR)
find_package(Dependencies/GLFW/glfw-3.2.1)

add_executable(SesamoEngine "Source Files/main.cpp")