### System Bootstrapping

This package is heavily influenced by a [blog](https://medium.com/medialesson/simple-service-locator-for-your-unity-project-40e317aad307) on service locators by Dino Fejzagić

------------------------
A small package for facilitating an easy and scalable singleton boot approach.

-------------------------

Singletons in Unity make a lot of things very easy. They also have some annoying features related to the Unity Livecycle of objects and randomization of destruction of objects,
causing null reference exeptions among other things. One of the more annoying parts to me is the part where your singletons are part of a scene and returning to this scene
causes the destruction of the duplicate singletons in this scene that are now conflicting with the singletons set to not be destroyed on load.

This conflict in combination with the fact that code in Awake can and will still run when an object is destroyed immediately after its creation, can cause a lot of problems with
events, coroutines and other interaction of your singletons with other scripts.

To circumvent this problem you would want your singletons to be created only once during the start of your game. Luckily there is an attribute that makes this possible.
This attribute called [RuntimeInitializeOnLoadMethod](https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html) gives you the option of 
executing a function once after or before loading the first scene.

To visualize the problem and how the bootstrapper fixes it:

# Without Bootstrap (placing them in the scene)
![Without Bootstrap](ReadmeImages/WithoutBootstrap.png)

# With Bootstrap
![With Bootstrap](ReadmeImages/WithBootstrap.png)
