This program is designed to visually simulate a cellular automata following the 23/3 rules of Conway's Game of Life.

![cell automata cap](https://user-images.githubusercontent.com/95654983/150011081-75f99a8c-375e-485b-98c2-eba8e9cd61e8.JPG)

The program is run on Windows Presentation Foundation. The code for the simulation is in the code-behind file MainWindow.xaml.cs.

This simulation plays out on a 30x30 grid of cells. A cell survives if it has exactly 2 or 3 neighbours, and dies otherwise. A new cell is born from a dead cell if it has exactly three neighbours. New generations are refreshed every 50 milliseconds. 
