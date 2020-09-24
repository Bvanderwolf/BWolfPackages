# SingletonBehaviour for easy use of the singleton pattern in Unity

provides singleton functionalities using inheritance to provide the monobehaviour tools like coroutines

------------------------------

These classes were heavily inspired by a blog post and a github project

- https://blog.mzikmund.com/2019/01/a-modern-singleton-in-unity/ ~ martin zikmund
- https://github.com/Goodgulf281/Unity-Formation-Movement/blob/master/Scripts/Utilities/Singleton.cs ~ goodgulf281

## Features

- LazySingletonBehaviour Class: Use this class only when you want singleton functionality on one of your classes without the use of inspector fields. This 
class uses the lazy pattern to create a new game object at run time when it is being called. This negates the problem with having to destroy an 
already existing singleton when returning to the original scene where the singleton was created. 

- SingletonBehaviour Class: Use this clas when you want your monobehaviour to have singleton functionalities but also be able to set inspector fields 
beforehand. When returning to the scene this singleton was created in, the already existing singleton will remain and the copy will destroy itself. 

## Packages that use this Utility
Example of usage of this utility can be found in multiple other packages

 - [ListPooling](https://github.com/Bvanderwolf/BWolfPackages/tree/master/Assets/BWolf/Utilities/ListPooling)
 - [InputManager](https://github.com/Bvanderwolf/BWolfPackages/tree/master/Assets/BWolf/Utilities/InputManager)
 - [ShapeShifting](https://github.com/Bvanderwolf/BWolfPackages/tree/master/Assets/BWolf/Utilities/ShapeShifting)
 - [PlayerProgression](https://github.com/Bvanderwolf/BWolfPackages/tree/master/Assets/BWolf/Utilities/PlayerProgression)

To learn more about lazy initialization:
https://docs.microsoft.com/en-us/dotnet/framework/performance/lazy-initialization 

You can download the UnityPackage here: https://drive.google.com/file/d/1EVlhAtmSM5FUpVh2AwKruheKZmt620G7/view?usp=sharing
