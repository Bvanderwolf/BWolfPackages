// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>An interface providing information on an achievement</summary>
    public interface IAchievementInfo
    {
        string Name { get; }
        string Description { get; }
        float Progress { get; }
    }
}