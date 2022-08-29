using BWolf.Utilities.StatModification;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.StatModification
{
    public class StatSystemTextDisplay : MonoBehaviour
    {
        [SerializeField]
        private PointStatSystem system = null;

        private Text textComponent;

        private void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        private void Update()
        {
            textComponent.text = $"{system.Current}/{system.Max}";
        }
    }
}