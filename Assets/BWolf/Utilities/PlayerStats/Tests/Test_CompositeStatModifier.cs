﻿using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace BWolf.PlayerStatistics.Tests
{
    /// <summary>
    /// Tests the <see cref="CompositeStatModifier"/>
    /// </summary>
    public class Test_CompositeStatModifier
    {
        [Test]
        public void Test_New()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            
            // Act.
            builder = CompositeStatModifier.New();
            
            // Assert.
            Assert.NotNull(builder);
        }

        [Test]
        public void Test_New_No_Modifiers()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            TestDelegate action;
            
            // Act.
            builder = CompositeStatModifier.New();
            action = () => builder.Build();
            
            // Assert.
            Assert.Catch<InvalidOperationException>(action);
        }

        [Test]
        public void Test_New_Builder_AddNew()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            
            // Act.
            builder = CompositeStatModifier.New();
            builder.AddNew<TestStatModifier>(m => m.BooleanValue = true);
            
            // Assert.
            Assert.NotNull(builder.Build());
        }

        [Test]
        public void Test_New_Builder_AddNew_Null_Constructor()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            Action<TestStatModifier> constructor;
            TestDelegate action;
            
            // Act.
            builder = CompositeStatModifier.New();
            constructor = null;
            action = () => builder.AddNew(constructor);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }
        
        [Test]
        public void Test_New_Builder_Add()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            TestStatModifier modifier;
            
            // Act.
            builder = CompositeStatModifier.New();
            modifier = ScriptableObject.CreateInstance<TestStatModifier>();
            builder.Add(modifier);
            
            // Assert.
            Assert.NotNull(builder.Build());
        }

        [Test]
        public void Test_New_Builder_Add_Null_Modifier()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            TestStatModifier modifier;
            TestDelegate action;
            
            // Act.
            builder = CompositeStatModifier.New();
            modifier = null;
            action = () => builder.Add(modifier);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }
        
        [Test]
        public void Test_New_Builder_AddNew_NonGeneric()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            
            // Act.
            builder = CompositeStatModifier.New();
            builder.AddNew(typeof(TestStatModifier), m => ((TestStatModifier)m).BooleanValue = true);
            
            // Assert.
            Assert.NotNull(builder.Build());
        }

        [Test]
        public void Test_New_Builder_AddNew_NonGeneric_Null_Constructor()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            Action<StatModifier> constructor;
            TestDelegate action;
            
            // Act.
            builder = CompositeStatModifier.New();
            constructor = null;
            action = () => builder.AddNew(typeof(TestStatModifier), constructor);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }
        
        [Test]
        public void Test_New_Builder_AddNew_NonGeneric_Null_Type()
        {
            // Arrange.
            CompositeStatModifier.Builder builder;
            Action<StatModifier> constructor;
            TestDelegate action;
            
            // Act.
            builder = CompositeStatModifier.New();
            constructor = m => ((TestStatModifier)m).BooleanValue = true;
            action = () => builder.AddNew(null, constructor);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }
    }
}