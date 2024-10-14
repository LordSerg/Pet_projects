# Celluar Automats

## Technology used
C#, WindowsForms, Math, Algorithms, dataStructure, VisualStudio

## Overview
Program that implements work of cellular automaton, including [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)

## About project
This project i've made while experimenting with [cellular automatons](https://en.wikipedia.org/wiki/Cellular_automaton). For example, how would differ behaveor of the system, relatively to the change of neighbourhood radius, or born condition, or survival condition...

## UI
The program has a simple UI:
- Button **Set new set** refreshes the current field and applies random location to the cells;
- **Occupancy %** - is for percentage of cells relatively to empty space when new set is applied;
- **Neighbourhood radius** - is a radius of view of each cell;
- NON - is abbreviature from Number Of Neighbours.

Other UI is intuitevly clear.

## Program example
![video of lightning](pictures/example.gif)

## Brief explanation
Usually the celluar automaton is an endless 2D plane with square cells on it. every cell can be either **ALIFE** or **DEAD**. (in program the light gray is for 'alife' and dark gray is for 'dead').

Every step of the program every cell is asked a question:
<p align="center">HOW MANY NEIGHBOURS DO I HAVE???</p>
Depending on the answer the cell is making decision, weather to:

- Stayin['](https://youtu.be/I_izvAbhExY?t=43)  alife
- Die
- Bron

Explains this by population:

If there are **too many** alife cells in the neighbourhood - then it's too tight for everyone - then to free some space some cells should "free" it ;)

If there are **too few** alife cells in the neighbourhood - then it's too lonely for the cell - and it dies;

but if there are **just right** population density - then everyone is comfortable - good time to stay alife and make other cells alife.


### In the original [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)
The condition for **ALIFE** cell to survive is *2 or 3* neighbours, in other cases the cell becomes **DEAD**

The condition for **DEAD** cell to become **ALIFE** is *exactly 3* neighbours;

And last, but not least - the radius of watching equals 1 (so the maximum number of neighbours = 8).