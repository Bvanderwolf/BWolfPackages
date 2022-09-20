using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace BWolf.StatSystems.Tests
{
    public class Test_PointSystem
    {
        private const int BASE_VALUE = 100;

        private const int TEST_MUTATE_VALUE = 50;

        private const string POINT_NAME = "statistic";

        private struct TestPointMutator : IPointMutator
        {
            public int Order => 0;

            public string[] GetPointNames() => new string[] { POINT_NAME };
            
            public int GetMutatedValue() => TEST_MUTATE_VALUE;
        }

        [Test]
        public void Test_SetBase_New()
        {
            // Arrange.
            PointSystem system = new PointSystem();
            
            // Act.
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Assert.
            Assert.AreEqual(system.GetBase(POINT_NAME), BASE_VALUE);
        }

        [Test]
        public void Test_SetBase_Change()
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, -1);
            
            // Act.
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Assert.
            Assert.AreEqual(system.GetBase(POINT_NAME), BASE_VALUE);
        }

        [Test]
        public void Test_GetBase()
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Act.
            int actual = system.GetBase(POINT_NAME);
            
            // Assert.
            Assert.AreEqual(actual, BASE_VALUE);
        }

        [Test]
        public void Test_GetBase_NotFound()
        {
            // Arrange.
            PointSystem system = new PointSystem();
            
            // Act.
            TestDelegate action = () => system.GetBase(POINT_NAME);
            
            // Assert.
            Assert.Catch<ArgumentException>(action);
        }

        [Test]
        public void Test_GetValue()
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Act.
            int actual = system.GetValue(POINT_NAME);
            
            // Assert.
            Assert.AreEqual(actual, BASE_VALUE);
        }
        
        [Test]
        public void Test_GetValue_Changed()
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, -1);
            
            // Act.
            system.SetBase(POINT_NAME, BASE_VALUE);
            int actual = system.GetValue(POINT_NAME);
            
            // Assert.
            Assert.AreEqual(actual, BASE_VALUE);
        }
        
        [TestCase(new object[] { 50, -50, -20 })]
        [TestCase(new object[] { 50, 100, 200 })]
        [TestCase(new object[] { 50 })]
        [Test]
        public void Test_Mutate_Value(object[] values)
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Act.
            for (int i = 0; i < values.Length; i++)
                system.Mutate(POINT_NAME, (int)values[i]);
            
            // Assert.
            int expected = BASE_VALUE + GetValueTotal(values);
            Assert.AreEqual(system.GetValue(POINT_NAME), expected);
        }

        [TestCase(new object[] { 0.5f, -0.5f, -0.2f })]
        [TestCase(new object[] { 0.5f, 1f, 2f })]
        [TestCase(new object[] { 0.5f })]
        [Test]
        public void Test_Mutate_Percentage(object[] percentages)
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Act.
            for (int i = 0; i < percentages.Length; i++)
                system.Mutate(POINT_NAME, (float)percentages[i]);
            
            // Assert.
            int expected = BASE_VALUE + GetPercentageTotal(percentages);
            Assert.AreEqual(system.GetValue(POINT_NAME), expected);
        }

        [TestCase(new object[] { 0.5f, -0.5f, -0.2f }, new object[] { 50, -50, -20 })]
        [TestCase(new object[] { 0.5f, 1f, 2f }, new object[] { 50, 100, 200 })]
        [TestCase(new object[] { 0.5f }, new object[] { 50 })]
        [Test]
        public void Test_Mutate_Percentage_With_Value(object[] percentages, object[] values)
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Act.
            for (int i = 0; i < percentages.Length; i++)
                system.Mutate(POINT_NAME, (float)percentages[i]);
            
            for (int i = 0; i < values.Length; i++)
                system.Mutate(POINT_NAME, (int)values[i]);
            
            // Assert.
            int expected = BASE_VALUE + GetValueTotal(values) + GetPercentageTotal(percentages);
            Assert.AreEqual(system.GetValue(POINT_NAME), expected);
        }

        [Test]
        public void Test_Mutate_IPointMutator()
        {
            // Arrange.
            PointSystem system = new PointSystem();
            system.SetBase(POINT_NAME, BASE_VALUE);
            
            // Act.
            system.Mutate(new TestPointMutator());
            
            // Assert.
            Assert.AreEqual(system.GetValue(POINT_NAME), BASE_VALUE + TEST_MUTATE_VALUE);
        }

        private static int GetValueTotal(object[] values)
        {
            int total = 0;

            for (int i = 0; i < values.Length; i++)
                total += (int)values[i];
            
            return total;
        }
        
        private static int GetPercentageTotal(object[] values)
        {
            int total = 0;

            for (int i = 0; i < values.Length; i++)
                total += (int)(BASE_VALUE * (float)values[i]);
            
            return total;
        }
    }
}