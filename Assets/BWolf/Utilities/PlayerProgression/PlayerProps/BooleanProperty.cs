// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>A boolean value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/BooleanProperty")]
    public class BooleanProperty : PlayerProperty<bool>
    {
        [SerializeField]
        private BooleanAchievement[] achievements = null;

        public override ProgressableObject<bool>[] Achievements
        {
            get { return achievements; }
        }
    }
}