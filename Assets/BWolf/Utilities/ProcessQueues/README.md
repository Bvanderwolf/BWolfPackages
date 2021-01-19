# Process Queues

A package containg utility scripts to setup systems that manage coroutine processes by queueing them

----------------------------------------------------

## Features

- An abstract ProcessManager class providing the necessary functionalities for managing processes using queues
- A ProcessQueue scriptable object class making it possible to separate the queue from the manager, making it usefull for e.g. display of processes etc.
- An abstract BroadcastingProcessManager class providing an implementation of a processManager that broadcasts when a process has ended
- A process event channel scriptable object class for supporting the BroadcastingProcessManager implementation with scriptable object events.

## Download

The UnityPackage can be downloaded here: https://drive.google.com/file/d/12qESAHxB9HpGFjVrAL8p004VsKVN64vn/view?usp=sharing
