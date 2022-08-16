namespace BWolf.PlayerStatistics.Tests
{
    public class TestStatModifier : StatModifier
    {
        public bool BooleanValue = false;

        public int intValue = 0;
        
        public override void Modify(PlayerStats stats)
        {
            TestPlayerStat testStatistic = stats.Get<TestPlayerStat>();
            if (testStatistic != null)
                testStatistic.intValue = intValue;
        }
    }
}
