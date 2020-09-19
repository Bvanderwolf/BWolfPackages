using BWolf.Utilities.PlayerProgression.Achievements;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>A floating point value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/FloatProperty")]
    public class FloatProperty : PlayerProperty<float>
    {
        [SerializeField]
        private FloatAchievement[] achievements = null;

        public override ProgressableObject<float>[] Achievements
        {
            get { return achievements; }
        }
    }
}