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

