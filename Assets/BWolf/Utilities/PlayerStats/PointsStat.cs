using UnityEngine;

namespace BWolf.PlayerStatistics
{
    [CreateAssetMenu(menuName = "PlayerStats/Points")]
    public class PointsStat : PlayerStat
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
            ResetToBaseValue();
        }

        public override void ResetToBaseValue()
        {
            _value = _base;
        }

        public static PointsStat FromValue(int value, int? baseValue = null)
        {
            PointsStat instance = CreateInstance<PointsStat>();
            instance._base = baseValue ?? value;
            instance._value = value;
            return instance;
        } 
    }
}
