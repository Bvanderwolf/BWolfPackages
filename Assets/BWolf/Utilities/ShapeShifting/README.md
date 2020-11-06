# Shape Shifting

A compact and expandable way to make an object switch shape using the Template Method Design Pattern.

## Design Patterns
- Template Method

## Features

- Creation of Shapes
- Retreival of Shape Templates for shifting
- Shift function for Shifting the shape of your game object

## Limitations

- Shape shifting uses the transform attributes of the gameobject and its children. 
Grandchildren of the game object are not taken into account when initializing the shape

- The part count (child count of game object shape references) needs to be equal to the one it is shifting to. 
Shifting from a circle with 4 parts to a rectangle with 5 is not possible.

## Download

You can download the UnityPackage here: https://drive.google.com/file/d/11jTShkcF7BkosXKVCQiMygGKOSBO8rif/view?usp=sharing 
