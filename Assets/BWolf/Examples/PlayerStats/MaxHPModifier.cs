using BWolf.PlayerStatistics;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats/Modifiers/HP")]
public class MaxHPModifier : StatModifier
{
    public int value;
    
    public override void Modify(PlayerStats stats)
    {
        PointsStat health = stats.Get<PointsStat>("Max_HP");
        if (health != null)
        {
            health.Value += value;
        }
        else
        {
            Debug.LogWarning("Failing modifying player stats :: 'Health' of type 'Points' not found.");
        }
    }
}
