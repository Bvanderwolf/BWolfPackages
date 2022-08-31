# PingPongValue 

A compact and simple way to store and use values for ping pong operations 
as provided by Unity's Mathf.PingPong method.

## Features
- A serializable PingPong class
- Min and max values (including negative values for min)
- Ping pong count
- A percentage to start from in ping ponging

## Usage example
### Creation and using awaiting it in a coroutine.
```c#
using System.Collections;
using BWolf.Utilities;
using UnityEngine;

public class PingPongUser : MonoBehaviour
{
    private PingPong _shaking;

    private void Awake()
    {
        _shaking = new PingPong();
        _shaking.min = 5;
        _shaking.max = -5;
        _shaking.speed = 8f;
        _shaking.startPercentage = 0.5f;
        _shaking.count = 2;
    }
   
    private void Start()
    {
        // Use the Await method to get a routine for the ping pong operation.
        // Add a method to do something with the ping pong value. 
        IEnumerator routine = _shaking.Await(SetXPosition);
        StartCoroutine(routine);
    }

    private void SetXPosition(float newXPosition)
    {
        Vector3 position = transform.position;
        position.x = newXPosition;
        transform.position = position;
    }
}
```
