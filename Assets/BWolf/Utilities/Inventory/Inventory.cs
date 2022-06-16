using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Inventory : IEnumerable
{
    // *Add event for each operation. Use Enum AddListener(Enum, Action) -> backend is dictionary
    public int Capacity
    {
        get => _capacity;
        set
        {
            if (value == _capacity)
                return;
            
            _capacity = value;
            Resize(_capacity);
        }
    }

    public int Size => _items.Length;

    public Item this[int index] => _items[index];

    protected int _capacity;
    
    protected Item[] _items;

    public Inventory(int capacity)
    {
        _capacity = capacity;
        _items = new Item[capacity];
    }

    public void Switch(int firstIndex, int secondIndex)
    {
        if (firstIndex < 0 || firstIndex >= _items.Length)
            throw new IndexOutOfRangeException($"First index {firstIndex} is out of bounds.");
        
        if (secondIndex < 0 || secondIndex >= _items.Length)
            throw new IndexOutOfRangeException($"Second index {secondIndex} is out of bounds.");
            
        Item firstItem = _items[firstIndex];
        
        _items[firstIndex] = _items[secondIndex];
        _items[secondIndex] = firstItem;
    }

    public bool Insert(int index, string name, int limit = 1)
    {
        if (index < 0 || index >= _items.Length)
            throw new IndexOutOfRangeException($"Insert index {index} is out of bounds.");

        if (string.IsNullOrEmpty(name))
            throw new ArgumentException($"Insert item name was null or empty.");
        
        Item item = _items[index];

        // If the index corresponds with a default entry, create a new item.
        if (item == default)
        {
            _items[index] = new Item(name, limit);
            return true;
        }
        
        // If the existing entry reached its limit, the item count can't be incremented.
        if (item.ReachedLimit)
            return false;
        
        IncrementItemCountAt(index);
        return true;
    }

    public bool Add(string name, bool ignoreCapacity) => Add(name, 1, ignoreCapacity);


    public bool Add(string name, int limit = 1, bool ignoreCapacity = false)
    {
        // Find the index of an existing item with given name that has not yet reached its limit.
        int index = Array.FindIndex(_items, item => item.name == name && item.limit == limit && !item.ReachedLimit);
        if (index == -1)
        {
            // If the item doesn't exist yet, add a new item.
            bool couldBeAdded = AddNewItemToContent(name, limit, ignoreCapacity);
            return couldBeAdded;
        }
        
        // If the item exists, update its count.
        IncrementItemCountAt(index);

        return true;
    }

    public Item RemoveAt(int index, int? count = null)
    {
        Item item = _items[index];
        int removeCount = count.GetValueOrDefault();
        if(removeCount == 0 || item.count == removeCount)
        {
            // If the remove count is 0 or all items are to be removed, we just set the entry to a default value.
            _items[index] = default;
        }
        else
        {
            if (removeCount < 1)
                throw new InvalidOperationException($"Trying to remove a negative amount items at {index}.");

            int itemCount = item.count;
            if (removeCount > itemCount)
                throw new InvalidOperationException(
                    $"Trying to retrieve {removeCount} of item at {index} while it has {item.count}");
                    
            // The entry is now a new item with the reduced amount of item count.
            _items[index] = new Item(item.name, itemCount - removeCount);

            // The item returned has the amount of count removed.
            item.count = removeCount;
        }

        return item;
    }

    public Item[] RemoveAt(int[] indices, int? count = null)
    {
        Item[] items = new Item[indices.Length];
        
        for (int i = 0; i < indices.Length; i++)
            items[i] = RemoveAt(indices[i]);

        return items;
    }

    public override string ToString()
    {
        if (_items.Length == 0)
            return string.Empty;

        return string.Join(" , ", _items);
    }

    public IEnumerator GetEnumerator() => _items.GetEnumerator();

    private bool AddNewItemToContent(string name, int limit, bool ignoreCapacity)
    {
        // First try filling a default entry.
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == default)
            {
                // Assign the new item to a default entry.
                _items[i] = new Item(name, limit);
                return true;
            }
        }

        if (ignoreCapacity)
        {
            // If there are no default entries left but we can ignore capacity, we force a resize and append the new item.
            Resize(_items.Length + 1);
            _items[_items.Length - 1] = new Item(name, limit);
            
            return true;
        }
        
        return false;
    }

    private void IncrementItemCountAt(int index)
    {
        Item existingItem = _items[index];
        existingItem.count++;
        
        _items[index] = existingItem;
    }

    private void Resize(int newSize)
    {
        int currentSize = _items.Length;
        if (newSize == currentSize)
            return;
        
        if (newSize < currentSize)
        {
            // If the inventory is getting smaller, first make sure all items are moved to the front.
            _items = _items.OrderBy(item => item == default).ToArray();

            // If there are more inventory items than the new size can hold, resize to the amount of items in the inventory.
            int itemsInInventory = _items.Count(item => item != default);
            if (itemsInInventory > newSize)
                newSize = itemsInInventory;
        }
        
        Array.Resize(ref _items, newSize);
    }
}
