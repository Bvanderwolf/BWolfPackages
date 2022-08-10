using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats/Modifiers/HP")]
public class HealthPointModifier : StatModifier
{
    public int value;
    
    public override void Modify(PlayerStats stats)
    {
        Points health = stats.Get<Points>("Max_HP");
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
