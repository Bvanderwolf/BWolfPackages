using UnityEngine;

public abstract class StatModifier : ScriptableObject
{
    public abstract void Modify(PlayerStats stats);
    
    /* Builder pattern? Toevoegen van virtual Random methode? Composite pattern gebruiken. */
    // public static StatModifier Random()
}
