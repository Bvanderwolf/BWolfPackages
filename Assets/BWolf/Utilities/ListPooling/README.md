# List Pooling

A Utility class for creation and disposing of lists using the Csharp programming language

### Features

  - List pooling for GameObject Lists
  - List Pooling for Transform Lists
  - An Example script explaining how to use above features


You can also:
  - Change default implementations of GameObject and Transform List pools
  - Create your own List Pool for a type of list that you use regulairly

### Changing Default Implementations

Implementations of the GameObject and Transform list pools can be customized

GameObject List Pool can be found in the project at

```sh
BWolf -> Utilities -> ListPooling -> GameObjectListPool.cs
```

For Transform List Pool...

```sh
BWolf -> Utilities -> ListPooling -> TransformListPool.cs
```

The script will contain functions like this which can be edited if you would like to.

```csharp
public override List<GameObject> Create(int capacity)
{
    List<GameObject> list;
    if (!bag.TryTake(out list))
     
        list = new List<GameObject>(capacity);
    }
    else
    {
        list.Capacity = capacity;
    }
    return list;
}
```

### Adding your own custom list pool 

Creating a new list pool can be done in a few easy steps

- Make sure your new List pool class derives from the abstract ListPool class, setting T to the type of list pool you want. (TransformListPool derives from ListPool\<Transform\>)
- Make sure you implement all methods using the bag given to you by the ListPool class for pooling. (You can look at Transform and GameObject list pools for the default implementation)
- Add your new custom list pool to the ListPoolService its "pools" dictionary where the key is your new type and the value a new instance of your custom list pool class.

This is what the default dictionary looks like:
```csharp
private static Dictionary<Type, object> pools = new Dictionary<Type, object>
{
    { typeof(GameObject), new GameObjectListPool() },
    { typeof(Transform),  new TransformListPool()  }
};
```
