# GameOfLife
 assignment07

If you want to run the game:

Scripts exsists on Main Camera (camera movement and camera zoom), also the child object of camera
cellScriptHolder holds the main script called CellsAndLogic.

Public variables and their uses
Grid Width and Grid Height: Change before starting game, decides the toal amount of cells.
Number Of Cells: Read Only, shows the total cells.

When Stable: Works with Alive/Dead For Round Count, when a cell lives for 1 generation their stability goes up based on AliveFRC, when they are dead stability goes down based on DeadFRC,
When stability reaches (When Stable) the Cell becomes Yellow, when its more then 1/3rd of (when stable) they become Orange, less then 1/3rd they become Red, 0 or less means they get turned off.

Percent alive: Is used when game runs or when the button R is pressed.

If space is pressed the game will pause, allowing you to draaw or erase new cells with (left click/right click).

To move the camera: Paning is middle mouse button, zooming is scrollwheel.

 If Prefabs are not in game: Prefab should have a default cube with changed pivot point (bottom left corner). Background should have a default cube, changed to black color.


Some fun settings: Chaning Alive for round count to 1, And DeadFRC to 0, and when stable to 100 gives a nice growth effect.
AFRC to 3, DFRC to 2 to see oscillator.
