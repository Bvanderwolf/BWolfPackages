using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace BWolf.PointModifications.Tests
{
    public class Test_PointSystem
    {
        private static int[] _negativePoints = new int[] { -1000, -250, -5 };

        private static int[] _positivePoints = new int[] { 1000, 250, 5 };

        private static float[] _intervals = new float[] { 0.75f, 0.75f, 0.125f }; 

        private static float[] _times = new float[] { 3f, 1f, 0.25f };
        
        [Test]
        public void Test_Update_PointModifier_Negative([ValueSource(nameof(_negativePoints))] int value)
        {
            // Arrange.
            PointSystem system = new PointSystem(1000, 1000);
            PointModifier modifier = new PointModifier(string.Empty, value,true,false);
            int currentBefore = system.Current;
            system.Add(modifier);
            
            // Act.
            system.Update();
            
            // Assert.
            Assert.AreEqual(currentBefore + value, system.Current);
        }
        
        [Test]
        public void Test_Update_PointModifier_Positive([ValueSource(nameof(_positivePoints))] int value)
        {
            // Arrange.
            PointSystem system = new PointSystem(0, 1000);
            PointModifier modifier = new PointModifier(string.Empty, value,true,false);
            int currentBefore = system.Current;
            system.Add(modifier);
            
            // Act.
            system.Update();
            
            // Assert.
            Assert.AreEqual(currentBefore + value, system.Current);
        }
        
        [UnityTest]
        public IEnumerator Test_Update_TimedPointModifier_Negative(
            [ValueSource(nameof(_negativePoints))] int value, 
            [ValueSource(nameof(_times))] float time)
        {
            // Arrange.
            PointSystem system = new PointSystem(1000, 1000);
            TimedPointModifier modifier = new TimedPointModifier(string.Empty, value, time);
            int currentBefore = system.Current;
            float yieldTime = 0f;
            system.Add(modifier);
            
            // Act.
            while (yieldTime <= time)
            {
                system.Update();
                yieldTime += Time.deltaTime;
                yield return null;
            }
            
            // Assert.
            Assert.AreEqual(currentBefore + value, system.Current);
        }
        
        [UnityTest]
        public IEnumerator Test_Update_TimedPointModifier_Positive(
            [ValueSource(nameof(_positivePoints))] int value, 
            [ValueSource(nameof(_times))] float time)
        {
            // Arrange.
            PointSystem system = new PointSystem(0, 1000);
            TimedPointModifier modifier = new TimedPointModifier(string.Empty, value, time);
            int currentBefore = system.Current;
            float yieldTime = 0f;
            system.Add(modifier);
            
            // Act.
            while (yieldTime <= time)
            {
                system.Update();
                yieldTime += Time.deltaTime;
                yield return null;
            }
            
            // Assert.
            Assert.AreEqual(currentBefore + value, system.Current);
        }
        
        [UnityTest]
        public IEnumerator Test_Update_TimedPointModifier_Interval_Positive(
            [ValueSource(nameof(_positivePoints))] int value, 
            [ValueSource(nameof(_times))] float time,
            [ValueSource(nameof(_intervals))] float interval)
        {
            // Arrange.
            PointSystem system = new PointSystem(0, 1000);
            TimedPointModifier modifier = new TimedPointModifier(string.Empty, value, time, interval);
            int currentBefore = system.Current;
            float yieldTime = 0f;
            system.Add(modifier);
            
            // Act.
            while (yieldTime <= time)
            {
                system.Update();
                yieldTime += Time.deltaTime;
                yield return null;
            }
            
            // Assert.
            Assert.AreEqual(currentBefore + value, system.Current);
        }
    }
}
