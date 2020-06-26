using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class GameAssetHandler : MonoBehaviour
    {
        [SerializeField]
        private Material diskMaterial = null;

        [SerializeField]
        private Material crossMaterial = null;

        private void Awake()
        {
            diskMaterial.color = Color.blue;
            crossMaterial.color = Color.red;
        }
    }
}