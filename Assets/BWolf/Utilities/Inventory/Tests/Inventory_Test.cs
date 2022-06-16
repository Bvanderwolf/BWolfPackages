﻿using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;

public class Inventory_Test 
{
   /// <summary>
   /// Tests whether the capacity set correctly.
   /// </summary>
   [TestCase(1000)]
   [TestCase(100)]
   [TestCase(0)]
   [Test]
   public void Test_Constructor_Capacity(int capacity)
   {
      // Arrange.
      Inventory inventory;

      // Act.
      inventory = new Inventory(capacity);
      
      // Assert.
      Assert.AreEqual(capacity, inventory.Capacity);
   }
   
   /// <summary>
   /// Tests whether the size set correctly.
   /// </summary>
   [TestCase(1000)]
   [TestCase(100)]
   [TestCase(0)]
   [Test]
   public void Test_Constructor_Size(int capacity)
   {
      // Arrange.
      Inventory inventory;

      // Act.
      inventory = new Inventory(capacity);
      
      // Assert.
      Assert.AreEqual(capacity, inventory.Size);
   }

   [TestCase(2000, 500, 1500)]
   [TestCase(200, 50, 100)]
   [TestCase(2, 0, 1)]
   [Test]
   public void Test_Switch(int capacity, int firstIndex, int secondIndex)
   {
      // Arrange.
      const string ITEM_ONE_NAME = "item_one";
      const string ITEM_TWO_NAME = "item_two";
      
      Inventory inventory = new Inventory(capacity);
      inventory.Insert(firstIndex,ITEM_ONE_NAME);
      inventory.Insert(secondIndex, ITEM_TWO_NAME);
      
      // Act.
      inventory.Switch(firstIndex, secondIndex);
      
      // Assert.
      Assert.AreEqual(inventory[firstIndex].name, ITEM_TWO_NAME);
      Assert.AreEqual(inventory[secondIndex].name, ITEM_ONE_NAME);
   }
   
   [TestCase(2, 1, 3)]
   [TestCase(2, 1, 2)]
   [TestCase(2, -1, 1)]
   [Test]
   public void Test_Switch_Out_Of_Range(int capacity, int firstIndex, int secondIndex)
   {
      // Arrange.
      Inventory inventory = new Inventory(capacity);
      
      // Act.
      TestDelegate action = () => inventory.Switch(firstIndex, secondIndex);
      
      // Assert.
      Assert.Catch<IndexOutOfRangeException>(action);
   }

   [TestCase(5, 4, "item_one")]
   [TestCase(5, 0, "item_one")]
   [Test]
   public void Test_Insert_Add_New(int capacity, int index, string name)
   {
      // Arrange.
      Inventory inventory = new Inventory(capacity);
      
      // Act.
      inventory.Insert(index, name);
      
      // Assert.
      Assert.AreEqual(inventory[index].name, name);
   }

   [TestCase(5, 4, "item_one", 5)]
   [TestCase(5, 0, "item_one", 2)]
   [Test]
   public void Test_Insert_Increment_Existing(int capacity, int index, string name, int count)
   {
      // Arrange.
      Inventory inventory = new Inventory(capacity);
      inventory.Insert(index, name, count);
      
      // Act.
      for (int counter = 0; counter < count; counter++)
         inventory.Insert(index, name);
      
      // Assert.
      Assert.AreEqual(count, inventory[index].count);
   }

   [TestCase(2, 3)]
   [TestCase(2, 2)]
   [TestCase(2, -1)]
   [Test]
   public void Test_Insert_Out_Of_Range(int capacity, int index)
   {
      // Arrange.
      const string ITEM_NAME = "item";
      Inventory inventory = new Inventory(capacity);
      
      // Act.
      TestDelegate action = () => inventory.Insert(index, ITEM_NAME);
      
      // Assert.
      Assert.Catch<IndexOutOfRangeException>(action);
   }
   
   [TestCase("")]
   [TestCase(null)]
   [Test]
   public void Test_Insert_Invalid_Name(string name)
   {
      // Arrange.
      const int INSERT_INDEX = 0;
      const int CAPACITY = 1;
      Inventory inventory = new Inventory(CAPACITY);
      
      // Act.
      TestDelegate action = () => inventory.Insert(INSERT_INDEX, name);
      
      // Assert.
      Assert.Catch<ArgumentException>(action);
   }
   
   [TestCase("item_one", 5)]
   [TestCase("item_one", 1)]
   [Test]
   public void Test_Insert_Reached_Limit(string name, int limit)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      inventory.Insert(0, name, limit);

      for (int i = 0; i < limit - 1; i++)
         inventory.Insert(0, name, limit);
      
      // Act.
      bool inserted = inventory.Insert(0, name, limit);
      
      // Assert.
      Assert.IsFalse(inserted);
   }

   [TestCase("item_one")]
   [Test]
   public void Test_Add_New_Item(string name)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      
      // Act.
      inventory.Add(name);
      
      // Assert.
      Assert.AreEqual(inventory[0].name, name);
   }

   [TestCase("item_one", 25)]
   [TestCase("item_one", 5)]
   [TestCase("item_one", 1)]
   [Test]
   public void Test_Add_Increment_Item(string name, int count)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      
      // Act.
      for (int i = 0; i < count; i++)
         inventory.Add(name, count);
      
      // Assert.
      Assert.AreEqual(inventory[0].count, count);
   }

   [TestCase(new object[] { "item_one", "item_two", "item_three", "item_four" })]
   [Test]
   public void Test_Add_Multiple_Items(object[] items)
   {
      // Arrange.
      Inventory inventory = new Inventory(items.Length);
      
      // Act.
      for (int i = 0; i < items.Length; i++)
         inventory.Add((string)items[i]);
      
      // Assert.
      for (int i = 0; i < items.Length; i++)
         Assert.AreEqual(inventory[i].name, (string)items[i]);
   }

   [TestCase("item_one", 11)]
   [TestCase("item_one", 6)]
   [TestCase("item_one", 2)]
   [Test]
   public void Test_Add_Reached_Limit(string name, int count)
   {
      // Arrange.
      int limit = count - 1;
      Inventory inventory = new Inventory(2);
      
      // Act.
      for (int i = 0; i < count; i++)
         inventory.Add(name, limit);
      
      // Assert
      Assert.AreEqual(inventory[1].name, name);
   }


   [TestCase("item_one", "item_two")]
   [Test]
   public void Test_Add_Inventory_Full(string itemOneName, string itemTwoName)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      inventory.Add(itemOneName);

      // Act.
      bool itemWasAdded = inventory.Add(itemTwoName);
      
      // Assert.
      Assert.IsFalse(itemWasAdded);
   }

   [TestCase("item_one")]
   [Test]   
   public void Test_Add_Ignore_Capacity_Return_Value(string name)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      inventory.Add(name);
      
      // Act.
      bool itemWasAdded = inventory.Add(name, true);
      
      // Assert.
      Assert.IsTrue(itemWasAdded);
   }

   [TestCase("item_one", 10)]
   [TestCase("item_one", 2)]
   [Test]
   public void Test_Add_Ignore_Capacity_Size_Update(string name, int count)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      inventory.Add(name);
      
      // Act.
      for(int counter = 0; counter < count - 1; counter++)
         inventory.Add(name, true);
      
      // Assert.
      Assert.AreEqual(count, inventory.Size);
   }

   [TestCase("item_one", 0)]
   [Test]
   public void Test_RemoveAt_No_Count(string name, int removeIndex)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      inventory.Add(name);
      
      // Act.
      inventory.RemoveAt(removeIndex);
      
      // Act.
      Assert.AreEqual(inventory[0], default(Item));
   }

   [TestCase("item_one", 0)]
   [Test]
   public void Test_RemoveAt_No_Count_Return_Value_Name(string name, int removeIndex)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      inventory.Add(name);
      
      // Act.
      Item item = inventory.RemoveAt(removeIndex);
      
      // Assert.
      Assert.AreEqual(name, item.name);
   }
   
   [TestCase("item_one", 0, 10)]
   [TestCase("item_one", 0, 5)]
   [TestCase("item_one", 0, 1)]
   [Test]
   public void Test_RemoveAt_No_Count_Return_Value_Count(string name, int removeIndex, int count)
   {
      // Arrange.
      Inventory inventory = new Inventory(1);
      inventory.Add(name);
      for (int counter = 0; counter < count - 1; counter++)
         inventory.Add(name);

      // Act.
      Item item = inventory.RemoveAt(removeIndex);
      
      // Assert.
      Assert.AreEqual(count, item.count);
   }
}
