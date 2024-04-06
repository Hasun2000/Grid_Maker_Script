# Grid Maker Script
 This Unity project is a simple project that contains a Unity script that can create a grid of pre-defined game objects from the inspector panel. The Script has two parts, the "GridMaker.cs" in the Editor folder, which controls a very small part of the Unity Editor inspector, and the "GridCreator.cs" that should be attached to a game object in the hierarchy to create a grid.
 
The Grid Creator class has 4 variables.
1. (int) Width - the width of the supposed grid
2. (int) Length - the length of the supposed grid
3. (float) Spacing - space between the cells of the grid
4. (Prefab) Grid cell game object
 
## How to Use 
+ First, Download or clone the project.
+ Create a new empty game object
+ Add the Grid Creator script from the scripts folder
+ Define the variables as your preference
+ Create the Grid!

## Functionality
The script contains three buttons.
  + Create Grid - create the grid with the defined parameters
  + Clear Grid - clear the current grid and all objects attached to the parent game object,
  + Delete - delete/removes the grid creator script component from the Game object

The script has another functionality, which can be used to create a single path between two grid cells. in the scene view, when taking the mouse over a grid cell, that grid cell will be highlighted with yellow color. clicking on that cell will select that as the starting point of the path, and clicking on another cell will select that as the end point of the path. 

Feel free to customize the scripts to your needs!
