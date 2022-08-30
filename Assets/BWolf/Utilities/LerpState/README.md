# LerpState 

A compact and simple way to store and use state for linear interpolation operations

## Features

- LerpOf<T> generic serializable class with Default Lerp implementations
  - Vector2Lerp
  - Vector3Lerp
  - QuaternionLerp
  - FloatLerp
- LerpFunction<T> generic delegate
- EasingFunction delegate supported by static EasingFunctions class
  - NoEase
  - easeOutSine
  - easeInSine
  - easeInOutSine

## Example Usage
### Creating a new instance
```c#
using BWolf.Utilities;

public class LerpValueUser
{
    private LerpOf<Vector3> lerp;
    
    public LerpValueUser()
    {
        Vector3 initial = new Vector3(0, 0, 0);
        Vector3 target = new Vector3(0, 1, 0);
        lerp = new LerpOf<Vector3>(initial, target);
    }
}
```
### Using serialization and starting
```c#
using BWolf.Utilities;
using UnityEngine;

public class LerpValueUser : MonoBehaviour
{
    // The default implentations of LerpOf<T> can all be serialized.
    // LerpOf<T> itself is only serializable in Unity 2020.1 and up.
    [SerializeField]
    private Vector3Lerp lerp;
    
    private void Awake()
    {
        // If you want a curve to be added to your interpolation
        // Set the easing function.
        lerp.easingFunction = EasingFunctions.easeInSine;
    }
    
    private void Start()
    {
        // Use the Await method to get a routine for the linear interpolation.
        // Add a method to do something with the linearly interpolated value.
        IEnumerator routine = lerp.Await(SetPosition);
        StartCoroutine(routine);
    }
    
    private void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
```
### Using the static methods
```c#
using BWolf.Utilities;
using UnityEngine;

public class LerpValueUser : MonoBehaviour
{  
    [SerializeField]
    private Vector3 initial;
    
    [SerializeField]
    private Vector3 target;
    
    [SerializeField]
    private float totalTime;
    
    private void Start()
    {   
        // Using the static Await method you can create a routine for linear
        // interpolation without need of a variable. 
        IEnumerator routine = LerpOf<Vector3>.Await(
            initial, 
            target, 
            Vector3.Lerp, 
            SetPosition, totalTime
        );
        
        StartCoroutine(routine);
    }
    
    private void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
```
