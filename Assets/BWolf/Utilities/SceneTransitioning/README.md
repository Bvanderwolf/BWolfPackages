## Scene Transitioning

A small package containing a utility for transitioning from one scene to another.

### Features

- Automatic Unloading and Loading of Scenes with Unity's LoadSceneMode.Additive
- Possibility to Use Intro and Outro Enumerators to be used as Coroutines to provide a nice transition animation
- Possibility to Use an OnProgressUpdate function to show progress of transition on screen. 

### Overview

Using this package, start by adding the SceneTransitionSystem component to a gameobject in the scene that you will load first.
In the editor you can fill in a name of a scene that will serve as the transition ui scene. Make sure to have this scene available
in your build settings since the SceneTransitionSystem will load this additively on Awake. 

Implement the ITransitionProvider interface to provide the SceneTransitionSystem with a Transition containing an outro and intro
in the form of enumerators used as [coroutines](https://docs.unity3d.com/Manual/Coroutines.html) aswell as a function providing
a percentage of the progress. Make sure your behaviours that implement this interface are part of the transition ui scene loaded
by the SceneTransitionSystem.

Using the Transition function provided by the SceneTransitionSystem you can transition from scene to scene by providing it either
with the TransitionName of your ITransitionProvider implemention or a reference to the component itself in addition to ofcourse the 
scene name and load mode.

### Dependencies
This package makes use of the following packages:
- SingletonBehaviours

### Download

You can download the UnityPackage here: https://drive.google.com/file/d/14Nxd8WixgryCpJ-Z57p3yhDfJ1T5PXc7/view?usp=sharing
