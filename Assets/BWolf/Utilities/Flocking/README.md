# Flocking 

Making flocking possible using FlockingBehaviours as ScriptableObjects

Reference: https://www.youtube.com/watch?v=mjKINQigAE4

### Features

  - Flock class for spawning and managing your flock
  - FockUnit to be moved/rotated by the Flock 
  - 3 main flocking behaviours
    - Cohesion -> CohesionBehaviur.cs
    - Alignment -> AlignmentBehaviour.cs
    - Seperation -> SeperationBehaviour.cs
  - 2 custom flocking behaviours
    - Obstacle Avoidance -> ObstacleAvoidanceBehaviour.cs
    - BoundedInArea -> BoundedBehaviour.cs
  - A Composite Behaviour for combining multiple behaviours and adding weights to each one
  
### Demo

The Flocking demo can be found at

```sh
BWolf -> Examples -> Flocking
```

You can download the package here:
https://drive.google.com/file/d/1_yDPpn5x1JpCHS3XP4h0SyIYvJOHYAH6/view?usp=sharing
