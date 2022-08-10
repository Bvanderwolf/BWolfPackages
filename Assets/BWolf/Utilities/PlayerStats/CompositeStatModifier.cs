using UnityEngine;

public abstract class CompositeStatModifier : StatModifier
{
    [SerializeField]
    private StatModifier[] _modifiers;
    
    public override void Modify(PlayerStats stats)
    {
        for(int i = 0; i < _modifiers.Length; i++)
            _modifiers[i].Modify(stats);
    }
}
