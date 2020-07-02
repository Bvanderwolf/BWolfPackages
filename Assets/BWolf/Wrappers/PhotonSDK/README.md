# Photon SDK Wrapper

A library for networking using the Photon multiplayer SDK.

### Current Version (1.0)
------------------

#### Simple Matchmaking 
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
  


