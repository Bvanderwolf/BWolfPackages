using BWolf.Gameplay;
using UnityEngine;

public class InventoryUser : MonoBehaviour
{
    [SerializeField]
    private Transform _gridParent;
    
    private Inventory _inventory;

    private void Awake()
    {
        // Create a new inventory with a capacity of 3.
        _inventory = new Inventory(3);
        
        // Add a stick to the inventory.
        _inventory.Add("stick");

        // Set the stack limit of a stick to 6.
        _inventory.SetStackLimit("stick", 6);
        
        // Add five sticks to the inventory.
        _inventory.Add("stick", 5);

        _inventory.Insert(2, "sword");
    }
}
