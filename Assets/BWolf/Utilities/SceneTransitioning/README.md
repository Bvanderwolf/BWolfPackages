# Scene Transitioning

A small package containing a utility for transitioning from one scene to another.

## Features

- Automatic Unloading and Loading of Scenes with Unity's LoadSceneMode.Additive
- Possibility to Add Intro and Outro Enumerators to be used as Coroutines to provide a nice transition animation
- Possibility to Add an OnProgressUpdate Listener to show progress of transition on screen. 

## Hints

- When using the SceneTransitionSystem its Transition function, it will return a SceneTransition Object to which you can
add your Outro, Intro and OnProgressUpdate Listener. You can look at the example in the SceneSwitchUI class.

- Make sure that when you want to switch scenes, the UI elements that are used for the transition are not part of the 
current active scene. The SceneTransitionSystem class will (When using LoadSceneMode.Additive) unload the current active scene
first and then load the scene you wanted to transition to. 

