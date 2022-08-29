using BWolf.Utilities.StatModification;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.StatModification
{
    public class StatSystemImageDisplay : MonoBehaviour
    {
        [SerializeField]
        private PointStatSystem system = null;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
    
            if (image.type != Image.Type.Filled)
            {
                Debug.LogError("stat system image needs to be of filled type!");
            }
        }

        private void Update()
        {
            image.fillAmount = system.Perc;
        }
    }
}