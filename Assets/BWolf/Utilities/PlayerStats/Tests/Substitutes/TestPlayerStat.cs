using BWolf.PlayerStatistics;
using UnityEngine;

public class TestPlayerStat : PlayerStat
{
    public const int DEFAULT_VALUE = -1;
    
    public int intValue;
    
    public override void ResetToBaseValue()
    {
        intValue = DEFAULT_VALUE;
    }
}
