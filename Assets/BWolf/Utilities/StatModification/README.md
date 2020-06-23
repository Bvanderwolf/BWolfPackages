# Stat Modification

A Utility class for modifiying and representing stats in games.

### Features

  - A StatSystem class containing:
    - a current and a max value which can both be modified using Modifier classes
    - 6 types of callbacks for stat system events
    - ways to add and remove modifiers from the system
    - ways to visualize current and max values using a fillable image or text
    
  - A serializable struct containing information about a stat modifier called "StatModifierInfo"
    
  - 2 Modification types which can be applied to a stat system
    - TimedStatModifier: modifies the stat system for given ammount of value over given ammount of time
    - ConditionalStatModifier: modifies the stat system for given ammount of value, each second, until given condition has been met
    
  - A demo scene showing off all the different functionalities of the stat system and stat modifier
  
### Examples

The demo scene and demo scripts can be found inside:

```sh
BWolf -> Examples -> StatModification
```

And on github:
https://github.com/Bvanderwolf/BWolfPackages/tree/master/Assets/BWolf/Examples/StatModification
