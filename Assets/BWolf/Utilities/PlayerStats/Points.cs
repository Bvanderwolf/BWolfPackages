using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats/Points")]
public class Points : PlayerStat
{
    [SerializeField]
    private int _base = 100;

    [SerializeField]
    private int _value;
    
    public int Value
    {
        get => _value;
        set
        {
            if (value == _value)
                return;

            _value = value;
            OnValueChanged();
        }
    }
    
    private void OnEnable()
    {
        Debug.Log("enabled");
        ResetToBaseValue();
    }

    public override void ResetToBaseValue()
    {
        _value = _base;
    }
}
