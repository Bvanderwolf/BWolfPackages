using System;
using NUnit.Framework;
using BWolf.PlayerStatistics;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * TODO:
 * Add unit tests
 * Check inventory (IInventory interface?) compatability with PlayerData
 * Check stat modification compatability with PlayerData 
 */
namespace BWolf.PlayerStatistics.Tests
{
    /// <summary>
    /// Tests the <see cref="StatModifier"/> class.
    /// </summary>
    public class Test_StatModifier
    {
        private class Test_G_IStatRandomizer : IStatRandomizer<TestStatModifier>
        {
            public void Randomize(TestStatModifier statModifier)
            {
                statModifier.IntValue = Random.Range(MIN_RANDOM_RANGE, MAX_RANDOM_RANGE);
            }
        }

        private class Test_IStatRandomizer : IStatRandomizer
        {
            public void Randomize(StatModifier statModifier)
            {
                ((TestStatModifier)statModifier).IntValue = Random.Range(MIN_RANDOM_RANGE, MAX_RANDOM_RANGE);
            }
        }
        
        private const int MIN_RANDOM_RANGE = 50;

        private const int MAX_RANDOM_RANGE = 100;
        
        [Test]
        public void Test_New_Generic_No_Constructor()
        {
            // Arrange.
            TestStatModifier modifier;

            // Act.
            modifier = StatModifier.New<TestStatModifier>();
            
            // Assert.
            Assert.NotNull(modifier);
        }
        
        [Test]
        public void Test_New_Generic_Constructor()
        {
            // Arrange.
            TestStatModifier modifier;

            // Act.
            modifier = StatModifier.New<TestStatModifier>(m => m.BooleanValue = true);
            
            // Assert.
            Assert.IsTrue(modifier.BooleanValue);
        }

        [Test]
        public void Test_New_NonGeneric_Correct_Type()
        {
            // Arrange.
            StatModifier modifier;
            Type expected;
            
            // Act.
            expected = typeof(TestStatModifier);
            modifier = StatModifier.New(expected);

            // Assert.
            Assert.AreEqual(expected, modifier.GetType());
        }

        [Test]
        public void Test_New_NonGeneric_Null_Type()
        {
            // Arrange.
            TestDelegate action;
            
            // Act.
            action = () => StatModifier.New(null);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }
        
        [Test]
        public void Test_New_NonGeneric_NonStatModifier()
        {
            // Arrange.
            TestDelegate action;
            
            // Act.
            action = () => StatModifier.New(typeof(ScriptableObject));
            
            // Assert.
            Assert.Catch<ArgumentException>(action);
        }
        
        [Test]
        public void Test_New_NonGeneric_Constructor()
        {
            // Arrange.
            StatModifier modifier;
            
            // Act.
            modifier = StatModifier.New(typeof(TestStatModifier), m =>
            {
                ((TestStatModifier)m).BooleanValue = true;
            });
            
            // Assert.
            Assert.IsTrue(((TestStatModifier)modifier).BooleanValue);
        }
        
        [Test]
        public void Test_Random_Generic()
        {
            // Arrange.
            IStatRandomizer<TestStatModifier> randomizer;
            TestStatModifier modifier;

            // Act.
            randomizer = new Test_G_IStatRandomizer();
            modifier = StatModifier.Random(randomizer);
            
            // Assert.
            Assert.IsTrue(modifier.IntValue >= MIN_RANDOM_RANGE && modifier.IntValue < MAX_RANDOM_RANGE);
        }

        [Test]
        public void Test_Random_Generic_Null_Randomizer()
        {
            // Arrange.
            TestDelegate action;
            IStatRandomizer<TestStatModifier> randomizer;
            
            // Act.
            randomizer = null;
            action = () => StatModifier.Random(randomizer);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }

        [Test]
        public void Test_Random_NonGeneric_Correct_Type()
        {
            // Arrange.
            IStatRandomizer randomizer;
            StatModifier modifier;
            Type expected;
            
            // Act.
            randomizer = new Test_IStatRandomizer();
            expected = typeof(TestStatModifier);
            modifier = StatModifier.Random(expected, randomizer);
            
            // Assert.
            Assert.AreEqual(expected, modifier.GetType());
        }
        
        [Test]
        public void Test_Random_NonGeneric()
        {
            // Arrange.
            IStatRandomizer randomizer;
            TestStatModifier modifier;
            
            // Act.
            randomizer = new Test_IStatRandomizer();
            modifier = (TestStatModifier)StatModifier.Random(typeof(TestStatModifier), randomizer);
            
            // Assert.
            Assert.IsTrue(modifier.IntValue >= MIN_RANDOM_RANGE && modifier.IntValue < MAX_RANDOM_RANGE);
        }
        
        [Test]
        public void Test_Random_NonGeneric_Null_Randomizer()
        {
            // Arrange.
            TestDelegate action;
            IStatRandomizer randomizer;
            
            // Act.
            randomizer = null;
            action = () => StatModifier.Random(typeof(TestStatModifier), randomizer);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }
    }
}