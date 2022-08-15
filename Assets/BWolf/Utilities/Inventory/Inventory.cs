﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BWolf.Gameplay
{
    /// <summary>
    /// Represents a modifiable inventory.
    /// </summary>
    public class Inventory : IEnumerable
    {
        // *Add event for each operation. Use Enum AddListener(Enum, Action) -> backend is dictionary
        
        /// <summary>
        /// The capacity of this inventory, in other words, how many items can fit
        /// in this inventory.
        /// </summary>
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

        /// <summary>
        /// The actual size of this inventory, in other words, how many item
        /// are actually in this inventory.
        /// </summary>
        public int Size => _items.Length;

        /// <summary>
        /// Returns an inventory item by index.
        /// </summary>
        /// <param name="index"></param>
        public Item this[int index] => _items[index];

        /// <summary>
        /// The capacity of this inventory.
        /// </summary>
        protected int _capacity;

        /// <summary>
        /// The items in this inventory.
        /// </summary>
        protected Item[] _items;

        /// <summary>
        /// The item limits used by this inventory.
        /// </summary>
        private readonly Dictionary<string, int> _stackLimits;

        /// <summary>
        /// Creates a new instance of this inventory with a given capacity.
        /// </summary>
        /// <param name="capacity">The capacity of this inventory.</param>
        public Inventory(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _capacity = capacity;
            _items = new Item[capacity];
            _stackLimits = new Dictionary<string, int>();
        }

        /// <summary>
        /// Sets the stack limit for an item in the inventory.
        /// </summary>
        /// <param name="name">The name of the item to set the stack limit for.</param>
        /// <param name="stackLimit">The stack limit used or the item.</param>
        public void SetStackLimit(string name, int stackLimit)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Trying to set limit of item with empty or null name.");

            if (stackLimit < 1)
                throw new ArgumentOutOfRangeException($"Limit of item can't be smaller than 1.");

            _stackLimits[name] = stackLimit;
        }

        /// <summary>
        /// Switches the positions of two items in the inventory.
        /// </summary>
        /// <param name="firstIndex">The first item's index.</param>
        /// <param name="secondIndex">The second item's index.</param>
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

        /// <summary>
        /// Inserts an item at a given index in the inventory.
        /// </summary>
        /// <param name="index">The index to insert the item at.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="count">The amount of items to insert.</param>
        /// <param name="ignoreCapacity">Whether to ignore the capacity during this operation.</param>
        /// <returns>Whether the insert was completed.</returns>
        public bool Insert(int index, string name, int count = 1, bool ignoreCapacity = false)
        {
            if (index < 0 || index >= _items.Length)
                throw new IndexOutOfRangeException($"Insert index {index} is out of bounds.");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Insert item name was null or empty.");

            Item item = _items[index];

            // If the index corresponds with a default entry, create a new item.
            if (item == default)
            {
                int stackLimit = GetOrCreateStackLimitForItem(name);

                _items[index] = new Item(name, stackLimit, count);
                return true;
            }

            // If the existing entry reached its stack limit, the item count can't be incremented.
            if (item.IsStacked)
                return false;

            IncrementItemCount(name, index, count, ignoreCapacity);
            return true;
        }

        /// <summary>
        /// Adds a new item to the inventory.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="ignoreCapacity">Whether to ignore the capacity during this operation.</param>
        /// <returns>Whether the item was added.</returns>
        public bool Add(string name, bool ignoreCapacity) => Add(name, 1, ignoreCapacity);

        /// <summary>
        /// Adds a new item to the inventory.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="count">The amount of items to add.</param>
        /// <param name="ignoreCapacity">Whether to ignore the capacity during this operation.</param>
        /// <returns>Whether the item was added.</returns>
        public bool Add(string name, int count = 1, bool ignoreCapacity = false)
        {
            // Find the index of an existing item with given name that has not yet reached its stack slimit.
            int index = Array.FindIndex(_items, item => item.name == name && !item.IsStacked);
            if (index == -1)
            {
                int stackLimit = GetOrCreateStackLimitForItem(name);

                // If the item doesn't exist yet, add a new item.
                bool couldBeAdded = AddNewItemToContent(name, stackLimit, count, ignoreCapacity);
                return couldBeAdded;
            }

            // If the item exists, update its count.
            IncrementItemCount(name, index, count, ignoreCapacity);

            return true;
        }

        /// <summary>
        /// Removes an item at a given index.
        /// </summary>
        /// <param name="index">The index to remove the item at</param>
        /// <param name="count">The amount of items to remove.</param>
        /// <returns>The removed item.</returns>
        public Item RemoveAt(int index, int? count = null)
        {
            if (index < 0 || index >= _items.Length)
                throw new IndexOutOfRangeException($"Index {index} out of range when trying to remove.");

            Item item = _items[index];

            // If the item to remove is a default entry, return a default value early.
            if (item == default)
                return default;

            int removeCount = count.GetValueOrDefault();
            if (!count.HasValue || item.stackCount == removeCount)
            {
                // If the remove count is 0 or all items are to be removed, we just set the entry to a default value.
                _items[index] = default;
            }
            else
            {
                // If there is a remove count and it is not the full item count, remove the count of the item.
                item = RemoveCountOfItem(item, index, removeCount);
            }

            return item;
        }

        /// <summary>
        /// Removes items at a given indices.
        /// </summary>
        /// <param name="indices">The indices to remove the items at</param>
        /// <param name="count">The amount of items to remove.</param>
        /// <returns>The removed items.</returns>
        public Item[] RemoveAt(int[] indices, int? count = null)
        {
            if (indices == null)
                throw new ArgumentNullException(nameof(indices));

            Item[] items = new Item[indices.Length];

            for (int i = 0; i < indices.Length; i++)
                items[i] = RemoveAt(indices[i], count);

            return items;
        }

        /// <summary>
        /// Returns the string representation of the inventory.
        /// </summary>
        /// <returns>The string representation of the inventory.</returns>
        public override string ToString()
        {
            if (_items.Length == 0)
                return string.Empty;

            return string.Join(" , ", _items);
        }

        /// <summary>
        /// Returns the enumerator of the inventory.
        /// </summary>
        /// <returns>The enumerator of the inventory.</returns>
        public IEnumerator GetEnumerator() => _items.GetEnumerator();

        /// <summary>
        /// Removes the stack count of an item.
        /// </summary>
        /// <param name="item">The item of which to remove the count.</param>
        /// <param name="indexOfItem">The index of the item.</param>
        /// <param name="removeCount">The amount to remove.</param>
        /// <returns>The updated item.</returns>
        private Item RemoveCountOfItem(Item item, int indexOfItem, int removeCount)
        {
            if (removeCount < 1)
                throw new InvalidOperationException($"Trying to remove a negative amount of items at {indexOfItem}.");

            int itemCount = item.stackCount;
            if (removeCount > itemCount)
                throw new InvalidOperationException(
                    $"Trying to retrieve {removeCount} of item at {indexOfItem} while it has {item.stackCount}");

            // The entry is now a new item with the reduced amount of item count.
            _items[indexOfItem] = new Item(item.name, item.stackLimit, itemCount - removeCount);

            // The item returned has the amount of count removed.
            item.stackCount = removeCount;

            return item;
        }

        /// <summary>
        /// Adds a new item to the content.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="stackLimit">The stack limit for the item.</param>
        /// <param name="stackCount">The stack count for the item.</param>
        /// <param name="ignoreCapacity">Whether to ignore the capacity for this operation.</param>
        /// <returns>Whether the adding succeeded.</returns>
        private bool AddNewItemToContent(string name, int stackLimit, int stackCount, bool ignoreCapacity)
        {
            // First try filling a default entry.
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == default)
                {
                    // Assign the new item to a default entry.
                    AddItemAtIndex(i);
                    return true;
                }
            }

            if (ignoreCapacity)
            {
                // If there are no default entries left but we can ignore capacity, we force a resize and append the new item.
                Resize(_items.Length + 1);
                AddItemAtIndex(_items.Length - 1);

                return true;
            }

            void AddItemAtIndex(int index)
            {
                if (stackCount > stackLimit)
                {
                    // If the count is greater than the stack limit, assign the limit as count
                    // and add a new item with the left over count.
                    int leftOverCount = stackCount - stackLimit;

                    stackCount = stackLimit;
                    _items[index] = new Item(name, stackLimit, stackCount);

                    Add(name, leftOverCount, ignoreCapacity);
                }
                else
                {
                    // If the count is not greater than the stack limit, just assign the new item with given info.
                    _items[index] = new Item(name, stackLimit, stackCount);
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the stack limit for an item, assigning a default
        /// stack limit to the item if none existed yet.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <returns>The stack limit for the item.</returns>
        private int GetOrCreateStackLimitForItem(string name)
        {
            if (!_stackLimits.ContainsKey(name))
            {
                const int DEFAULT_STACK_LIMIT = 1;

                _stackLimits.Add(name, DEFAULT_STACK_LIMIT);
                return DEFAULT_STACK_LIMIT;
            }

            return _stackLimits[name];
        }

        /// <summary>
        /// Increments the amount of an item in the inventory.
        /// </summary>
        /// <param name="name">The name of the existing item to increment.</param>
        /// <param name="index">The index to of the existing item to increment.</param>
        /// <param name="count">The amount to increment.</param>
        /// <param name="ignoreCapacity">Whether to ignore capacity during this operation.</param>
        private void IncrementItemCount(string name, int index, int count, bool ignoreCapacity)
        {
            Item existingItem = _items[index];
            int countToLimit = existingItem.stackLimit - existingItem.stackCount;
            if (count > countToLimit)
            {
                // If the count is greater than the count to limit, assign the count to limit
                // to the existing item's count and add a new item with the left over count.
                int leftOverCount = count - countToLimit;

                existingItem.stackCount += countToLimit;

                Add(name, leftOverCount, ignoreCapacity);
            }
            else
            {
                // If the count is not greater than the count to limit, just assign count to the existing item.
                existingItem.stackCount += count;
            }

            _items[index] = existingItem;
        }

        /// <summary>
        /// Resizes the inventory with a new size.
        /// </summary>
        /// <param name="newSize">The new size of the inventory.</param>
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
}