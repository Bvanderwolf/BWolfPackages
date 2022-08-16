using NUnit.Framework;
using UnityEngine;

namespace BWolf.PlayerStatistics.Tests
{
    /// <summary>
    /// Tests the <see cref="PlayerStats"/> class.
    /// </summary>
    public class Test_PlayerStats
    {
        [Test]
        public void Test_Add()
        {
            // Arrange.
            PlayerStats statistics;
            PlayerStat statistic;
            
            // Act.
            statistics = new PlayerStats();
            statistic = ScriptableObject.CreateInstance<TestPlayerStat>();
            statistics.Add(statistic);
            
            // Assert.
            Assert.AreEqual(1, statistics.Count);
        }

        [Test]
        public void Test_Remove()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            PlayerStat statistic = ScriptableObject.CreateInstance<TestPlayerStat>();
            statistics.Add(statistic);
            
            // Act.
            statistics.Remove(statistic);
            
            // Assert.
            Assert.AreEqual(0, statistics.Count);
        }
        
        [Test]
        public void Test_Modify()
        {
            // Arrange.
            StatModifier modifier;
            int expected;
            PlayerStats statistics = new PlayerStats();
            TestPlayerStat statistic = ScriptableObject.CreateInstance<TestPlayerStat>();
            statistics.Add(statistic);
            
            // Act.
            expected = 100;
            modifier = StatModifier.New<TestStatModifier>(m => m.intValue = expected);
            statistics.Modify(modifier);
            
            // Assert.
            Assert.AreEqual(expected, statistic.intValue);
        }

        [Test]
        public void Test_UnModify_Specific()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            StatModifier m1 = StatModifier.New<TestStatModifier>();
            StatModifier m2 = StatModifier.New<TestStatModifier>();
            statistics.Modify(m1);
            statistics.Modify(m2);
            
            // Act.
            statistics.UnModify(m1);
            
            // Assert.
            Assert.AreEqual(1, statistics.ModifierCount);
        }

        [Test]
        public void Test_UnModify_All()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            StatModifier m1 = StatModifier.New<TestStatModifier>();
            StatModifier m2 = StatModifier.New<TestStatModifier>();
            statistics.Modify(m1);
            statistics.Modify(m2);
            
            // Act.
            statistics.UnModify();
            
            // Assert.
            Assert.AreEqual(0, statistics.ModifierCount);
        }

        [Test]
        public void Test_GetMultiple()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            PlayerStat ps1 = ScriptableObject.CreateInstance<TestPlayerStat>();
            PlayerStat ps2 = ScriptableObject.CreateInstance<TestPlayerStat>();
            statistics.Add(ps1);
            statistics.Add(ps2);
            
            // Act.
            TestPlayerStat[] array = statistics.GetMultiple<TestPlayerStat>();
            
            // Assert.
            Assert.AreEqual(2, array.Length);
        }
        
        [Test]
        public void Test_GetMultiple_Not_Found()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();

            // Act.
            TestPlayerStat[] array = statistics.GetMultiple<TestPlayerStat>();
            
            // Assert.
            Assert.AreEqual(0, array.Length);
        }

        [Test]
        public void Test_Get_Found()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            PlayerStat ps1 = ScriptableObject.CreateInstance<TestPlayerStat>();
            statistics.Add(ps1);
            
            // Act.
            TestPlayerStat stat = statistics.Get<TestPlayerStat>();
            
            // Assert.
            Assert.NotNull(stat);
        }
        
        [Test]
        public void Test_Get_Not_Found()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();

            // Act.
            TestPlayerStat stat = statistics.Get<TestPlayerStat>();
            
            // Assert.
            Assert.IsNull(stat);
        }

        [Test]
        public void Test_Get_Name_Found()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            string name = "PlayerStatOne";
            PlayerStat ps1 = ScriptableObject.CreateInstance<TestPlayerStat>();
            ps1.name = name;
            statistics.Add(ps1);
            
            // Act.
            TestPlayerStat stat = statistics.Get<TestPlayerStat>(name);
            
            // Assert.
            Assert.NotNull(stat);
        }
        
        [Test]
        public void Test_Get_Name_Not_Found()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            PlayerStat ps1 = ScriptableObject.CreateInstance<TestPlayerStat>();
            statistics.Add(ps1);
            
            // Act.
            TestPlayerStat stat = statistics.Get<TestPlayerStat>("WrongName");
            
            // Assert.
            Assert.IsNull(stat);
        }

        [Test]
        public void Test_Get_NonGeneric_Found()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            string name = "PlayerStatOne";
            PlayerStat ps1 = ScriptableObject.CreateInstance<TestPlayerStat>();
            ps1.name = name;
            statistics.Add(ps1);
            
            // Act.
            PlayerStat stat = statistics.Get(name);
            
            // Assert.
            Assert.NotNull(stat);
        }
        
        [Test]
        public void Test_Get_NonGeneric_NotFound()
        {
            // Arrange.
            PlayerStats statistics = new PlayerStats();
            PlayerStat ps1 = ScriptableObject.CreateInstance<TestPlayerStat>();
            statistics.Add(ps1);
            
            // Act.
            PlayerStat stat = statistics.Get("WrongName");
            
            // Assert.
            Assert.IsNull(stat);
        }
    }
}