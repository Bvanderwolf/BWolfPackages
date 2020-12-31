# Player Progression

A scalable package focused on providing options to create, manage and save player progression using scriptable objects.

## Features
  
### Quest Management


In this package a Quest is a container for multiple Quest Tasks. It can be set active to start updating its tasks. Completing all tasks will complete the quest. 

By default there are 3 types of QuestTask
 - DoOnceTask: do something only once
 - IncrementTask: do something a set ammount of times
 - MinimalValueTask: accumulate a minimal ammount of value

These can be created by using the Creation menu in the Project View: Create/PlayerProgression/Quest & Create/PlayerProgression/QuestTasks

### PropertyManagement

This package comes with 3 different types of Properties:
  - BooleanProperties
  - FloatProperties
  - IntegerProperties
  
 These can be created by using the Creation menu in the Project View: Create/PlayerProgression/PlayerProps
 
 With PlayerProperties come also Achievements: Boolean, Float and Integer
 A player property can hold multiple achievements. When Updating a derived 
 player property class these achievements can also be updated using the generic GetAchievement and GetAchievements functions.
  
### Progress Saving and Loading

This package uses the static FileStorageSystem class to save and load data from local storage 
this class provides functionality to save and load the current state of Quests, QuestTasks
Player Properties and Achievements. Look for examples of implementations inside the standard
QuestTasks, Achievements and Player Properties provided.

### Displaying and Callbacks

The PropertyManager provides an AchievementCompleted event to get callbacks on achievements completed. The QuestManager class provides
an array of ActiveQuests which can be retreived to display and an event to provide callbacks for when
a quest has been completed. 

## Dependencies

This package makes use of the following packages:
- FileStorage

## Download

The UnityPackage can be downloaded here: https://drive.google.com/file/d/1CLyNduqJDfh1h6d6Cj6L1l-H1AuGyWcs/view?usp=sharing
