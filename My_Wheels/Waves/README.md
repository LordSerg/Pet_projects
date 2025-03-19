# Technology used
C++, OpenGL, GLFW, math, algorithms

# Overview
Program that simulates light wave on the plane.

# About project
The idea can't be easier: every pixel has its current height, current velocity, weight and connection to the neighbour pixels.

Every pixel can only move up or down.

Every frame each of pixel moves and checks, what is the velocity of its neighbourhood and changes velocity of itself, averaging neighbourhood.

The higher pixel - the brighter pixel.

For beauty I also added similar mechanics to each color(RGB) separately.

## Examples
Example of working program (speeded up)
![Example of algorithm 1](pictures/waves.gif)

