using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats/Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private PlayerStats _stats;

    public PlayerStats Stats => _stats;
}
