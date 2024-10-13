# Fourier_Pendulum

## Technology used
C#, WindowsForms, Math, Algorithms, InnoSetup, VisualStudio

## Overview
Program that decomposes a picture into two Fourier series and draws it as a chain of pendulums 

## About project
This project was made for 2 purposes: (1) for fun and (2) to use [Inno Setup](https://jrsoftware.org/isinfo.php) technology

## UI
To draw use the left mouse button;

to refresh screen click right mouse button.

## Program example
![video of lightning](pictures/example.gif)

## Brief math explanation
[Fourier series](https://en.wikipedia.org/wiki/Fourier_series) are usualy used to transform some function into the series of functions.

In current program sequence straight lines appear instead of functions. 

To decompose straight lines into a Fourier series:
1. the whole picture represent in the form of two lists: (1) X-points and (2) Y-points.
2. Each list we transform using [
Discrete Fourier transform](https://en.wikipedia.org/wiki/Discrete_Fourier_transform)
3. Now we have 2 Fourier series
4. Each element of series gives us frequency, phase and amplitude, which gives us a vector with length=*amplitude*, direction=*phase* and rotation speed = *frequency*
5. chaining those vectors one after another we get the picture.