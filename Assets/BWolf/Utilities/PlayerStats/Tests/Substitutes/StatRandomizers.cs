using UnityEngine;

namespace BWolf.PlayerStatistics.Tests
{
    internal class Test_G_IStatRandomizer : IStatRandomizer<TestStatModifier>
    {
        public void Randomize(TestStatModifier statModifier)
        {
            statModifier.intValue = Random.Range(
                Test_StatModifier.MIN_RANDOM_RANGE, 
                Test_StatModifier.MAX_RANDOM_RANGE);
        }
    }

    internal class Test_IStatRandomizer : IStatRandomizer
    {
        public void Randomize(StatModifier statModifier)
        {
            ((TestStatModifier)statModifier).intValue = Random.Range(
                Test_StatModifier.MIN_RANDOM_RANGE, 
                Test_StatModifier.MAX_RANDOM_RANGE);
        }
    }
}
