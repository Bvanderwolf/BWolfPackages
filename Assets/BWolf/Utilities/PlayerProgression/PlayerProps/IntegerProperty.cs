// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>An Integer value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/IntegerProperty")]
    public class IntegerProperty : PlayerProperty<int>
    {
        [SerializeField]
        private IntegerAchievement[] achievements = null;

        public override ProgressableObject<int>[] Achievements
        {
            get { return achievements; }
        }
    }
}