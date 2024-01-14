# Fast 2D board game example
Example draws random grid which can be scaled by slider. There is white movable unit which spawns other color units in spiral pattern around itself. Clear button removes units which have neighbors of same color.

It illustrates techniques to optimize drawing very large grid and fast addition/removal of units on grid.

## Technical details
- Project was created in Unity 2022.3.7f1.
- Grid size is set in Assets/Resources/BoardConfig.json
- Other configurations as ScriptableObjects are in Assets/Configs
- Simple service locator is used as a module system.
- Tested on a phone with 2500x2500 size and 120fps with no framerate drops.
