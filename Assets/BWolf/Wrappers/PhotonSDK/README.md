# Photon SDK Wrapper

A library for networking using the Photon multiplayer SDK.

Latest Downloadable (Google Drive):
[UnityPackage](https://drive.google.com/file/d/1kFEojh88C8Bp2ioc1oYW1R8_ehtAognP/view?usp=sharing)

## Current Version (1.0)

### Simple Matchmaking 
- Connect using photon's default settings asset
- Join and Leave lobbies
- Receive lobby statics information
- Join, Create and Leave rooms
- Receive Room information inside a lobby

### Object Instantiation and Destruction
- Objects can be instantiated using a Pooling system
- Objects can be destroyed using a Pooling system
- Objects are categorized into Movable and Static Objects where Movable objects have a PhotonView and Static Objects dont

### Events
- Events can be registered 
- Events can be listened to 
- Events can be raised 
- Types can be registered so they can be used as event content

### Requests
- Requests can be registered 
- Requests can be listened to 
- Requests can be raised 

### Callbacks 
- Clients can receive callbacks on matchmaking events
- Clients can receive callbacks on room events

### Synchronizing objects
- A MovableNetworkedObject script implementing IPunObservable to synchronize the transform's position/rotation/scale properties

### The Client class
- A local client instance containing your local client's information using photon's local player
- properties synchronized with others using Photon's Player Properties
- A client handler class managing local client and client properties
  

### Demo Game
- 1 demo scene in which the player can connect to a lobby and room
- 1 demo scene where the player can play the game Tic-Tac Toe with another player
- Multiple scripts using the Photon Wrapper functionalities
