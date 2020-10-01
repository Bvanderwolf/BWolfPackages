# Player Progression

A scalable package focused on providing options to create, manage and save player progression using scriptable objects.

## Features
  
### Quest Management


In this package a Quest is a container for multiple Quest Tasks. It can be set active to start updating its tasks. Completing all tasks will complete the quest. 

By default there are 3 types of QuestTask
 - DoOnceTask: do something only once
 - IncrementTask: do something a set ammount of times
 - MinimalValueTask: accumulate a minimal ammount of value

Quests can be interacted with by using the QuestGiver class. The quest giver class is a monobehaviour containing quests which can be
managed by a derived class to make it possible to make a quest active and to update its tasks until completed.

These can be created by using the Creation menu in the Project View: Create/PlayerProgression/Quest & Create/PlayerProgression/QuestTasks

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

The Achievement class from which all Achievement types derive implements the IAchievementInfo interface.
This interface is used to provide information to display and retreive from callbacks. The QuestManager class provides
an array of ActiveQuests which can be retreived to display and an event to provide callbacks for when
a quest has been completed. The PlayerProperty class provides this functionality for Achievements held by PlayerProperties.

The UnityPackage can be downloaded here: https://drive.google.com/file/d/1CLyNduqJDfh1h6d6Cj6L1l-H1AuGyWcs/view?usp=sharing
