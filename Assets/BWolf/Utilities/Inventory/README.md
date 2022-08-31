# Inventory

Provides a easy to use, compact and extendable way to manage items inside an inventory.
State members are protected meaning extension through inheritance makes it possible to add project specific methods like Sorting.

## Features
- An Inventory class to manage items.
- A serializable Item struct.

## Usage Examples
### Adding items
```c#
using BWolf.Gameplay;

public class InventoryUser
{
    private Inventory _inventory;

    public InventoryUser()
    {
        // Create an inventory with a capacity of 1.
        _inventory = new Inventory(1);
        
        // Set the stack limit of the stick to 6.
        _inventory.SetStackLimit("stick", 6);
        
        // Add a stick to the inventory.
        _inventory.Add("stick");

        // Add five sticks to the inventory.
        _inventory.Add("stick", 5);
        
        // Add a stick ignoring the capacity of the inventory (overflowing it).
        _inventory.Add("stick", true);
    }
}
```
### Inserting items
```c#
using BWolf.Gameplay;

public class InventoryUser
{
    private Inventory _inventory;

    public InventoryUser()
    {
        // Create an inventory with a capacity of 3.
        _inventory = new Inventory(3);
        
        // Add a stick to the inventory.
        _inventory.Add("stick");

        // Insert a sword at the last position in the inventory.
        _inventory.Insert(2, "sword");
    }
}
```
### Switching items
```c#
using BWolf.Gameplay;

public class InventoryUser
{
    private Inventory inventory;

    public InventoryUser()
    {
        // Create an inventory with a capacity of 2.
        inventory = new Inventory(2);
        
        // Add a stick to the inventory.
        _inventory.Add("stick");

        // Add a sword to the inventory.
        _inventory.Add("sword");
        
        // Switch the stick and sword their positions in the inventory.
        _inventory.Switch(0, 1);
    }
}
```
### Removing items
```c#
using BWolf.Gameplay;

public class InventoryUser
{
    private Inventory _inventory;

    public InventoryUser()
    {
        // Create an inventory with a capacity of 3.
        _inventory = new Inventory(3);
        
        // Insert 3 sticks at the second position in the inventory.
        _inventory.Insert(1, "stick", 3);

        // Remove 2 items at the second position in the inventory.
        _inventory.RemoveAt(1, 2);
    }
}
```
