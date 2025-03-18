# Technology used
C#, WindowsForms, math, algorithms

# Overview
Program that implements [floating horizon algorithm](https://www.scribd.com/doc/234456127/Floating-Horizon-Algorithm). 

# About project
This method usually used for representation of 3D surface of form:

$$F(x,y,z)=0$$

In this case there was used this function:

$${C_1\cos{(C_2\sqrt{x^2+y^2})} \over \sqrt{x^2+y^2} - C_1\sin{(C_3\sqrt{(x-0.1)^2+y^2})}}+200-z=0$$

**C1**, **C2** and **C3** are variable values, that user can interact with.

## Examples

![Example of algorithm 1](pictures/example.gif)

