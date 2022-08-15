using BWolf.PlayerStatistics;
using UnityEngine;

public class StatModifyBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;

    [SerializeField]
    private HealthPointModifier _modifier;

    private void Awake()
    {
        Abilities abilities = _playerData.Stats.Get<Abilities>();
        Debug.Log($"Checking out the {abilities.name} statistic.");
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerData.Stats.Get<Points>("Max_HP").ValueChanged += OnHealthPointValueChange;
        _playerData.Stats.Get("Max_MP").ValueChanged += OnManaValueChange;
        _playerData.Stats.Modify(_modifier);
    }

    private void OnHealthPointValueChange()
    {
        Debug.Log("Health value has changed.");
    }

    private void OnManaValueChange()
    {
        Debug.Log("Mana value has changed.");
    }
}
