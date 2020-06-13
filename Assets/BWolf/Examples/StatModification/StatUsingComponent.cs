using BWolf.Utilities.StatModification;
using UnityEngine;

namespace BWolf.Examples.StatModification
{
    public class StatUsingComponent : MonoBehaviour
    {
        [SerializeField]
        private StatModifierInfo statModifier = new StatModifierInfo();

        [SerializeField]
        private StatSystem statSystem = null;

        [SerializeField]
        private int current = 0;

        private void Awake()
        {
            statSystem.SetCurrentToMax();
        }

        private void Update()
        {
            statSystem.UpdateModifiers();
            current = statSystem.Current;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                statSystem.AddTimedModifier(statModifier, 1);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                statSystem.AddConditionalModifier(statModifier).OnSecondPassed += (name, value) =>
                {
                    print(name + " : " + value);
                };
            }
        }
    }
}