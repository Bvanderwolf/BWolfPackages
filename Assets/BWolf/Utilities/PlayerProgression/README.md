# Player Progression

A scalable package focused on providing options to create, manage and save player progression using scriptable objects.

## Features
  
### Quest Management

This package comes with 3 different types of Quests: 
 - BooleanQuests
 - FloatQuests
 - IntegerQuests

These can be created by using the Creation menu in the Project View: Create/PlayerProgression/Quests

### PropertyManagement

This package comes with 3 different types of Properties:
  - BooleanProperties
  - FloatProperties
  - IntegerProperties
  
 These can be created by using the Creation menu in the Project View: Create/PlayerProgression/PlayerProps
 
 With PlayerProperties come also a set of the same type of Achievements. A player property can hold 
 achievements of the same type. For Example: A BooleanProperty can hold multiple BooleanAchievements.
  
### Progress Saving and Loading

This packages also comes with a simple static class called ProgressFileSystem.
this class provides functionality to save and load the current state of Quests, 
Properies and Achievements. This is already done internally in the player properties and both standard quest and achievement types.

### Displaying and Callbacks

The ProgressableObject class from which Quest and Achievement types derive implements the IProgressInfo interface.
This interface is used to provide information to display and retreive from callbacks. The QuestManager class provides
a list of IProgressInfo objects which can be retreived to display quests and an event to provide callbacks for when
a quest has been completed. The PlayerProperty class provides this functionality for Achievements held by PlayerProperties.

The UnityPackage can be downloaded here: https://drive.google.com/file/d/1CLyNduqJDfh1h6d6Cj6L1l-H1AuGyWcs/view?usp=sharing
