using System;

public class Inventory
{
    public struct Item : IEquatable<Item>, IFormattable
    {
        public readonly string name;

        public int count;
        
        // *Add maxCount

        public Item(string name, int count = 1)
        {
            this.name = name;
            this.count = count;
        }

        public override bool Equals(object other)
        {
            if (!(other is Item item))
                return false;

            return Equals(item);
        }

        public bool Equals(Item other) => name == other.name;

        public override int GetHashCode() => name.GetHashCode();

        public static bool operator ==(Item lhs, Item rhs) => lhs.Equals(rhs);

        public static bool operator !=(Item lhs, Item rhs) => !(lhs == rhs);

        public override string ToString() => ToString(null, null);

        public string ToString(string format) => ToString(format, null);

        public string ToString(string format, IFormatProvider formatProvider) => name;
    }

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

    public int Size => _content.Length;

    private int _capacity;
    
    private Item[] _content;

    public Inventory(int capacity)
    {
        _capacity = capacity;
        _content = new Item[capacity];
    }

    public void Switch(string first, string second)
    {
        
    }
    
    
    public bool Add(string name, bool ignoreCapacity = false)
    {
        int index = Array.FindIndex(_content, item => item.name == name);
        if (index == -1)
        {
            // If the item doesn't exist yet, add a new item.
            bool couldBeAdded = AddNewItemToContent(name, ignoreCapacity);
            return couldBeAdded;
        }
        
        // If the item exists, update its count.
        IncrementItemCountAt(index);

        return true;
    }

    public void Remove(string name)
    {
        
    }

    private bool AddNewItemToContent(string name, bool ignoreCapacity)
    {
        for (int i = 0; i < _content.Length; i++)
        {
            if (_content[i] == default)
            {
                // Assign the new item to a default entry.
                _content[i] = new Item(name);
                return true;
            }
        }

        if (ignoreCapacity)
        {
            // If there are no default entries left but we can ignore capacity, we force a resize and append the new item.
            Resize(_content.Length);
            _content[_content.Length - 1] = new Item(name);
            
            return true;
        }
        
        return false;
    }

    private void IncrementItemCountAt(int index)
    {
        Item existingItem = _content[index];
        existingItem.count++;
        
        _content[index] = existingItem;
    }

    private void Resize(int newSize)
    {
        int currentSize = _content.Length;
        if (newSize == currentSize)
            return;

        if (newSize > currentSize)
        {
            // If the new size is bigger than the current size, resize the content to be bigger.
            Array.Resize(ref _content, newSize);
        }
        else
        {
            // If the new size is smaller than the current size, cut the content size to the last default entry greater or equal to the current size.
            Array.Sort(_content);
            
        }
    }
}
