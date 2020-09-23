// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

namespace BWolf.Utilities.PlayerProgression
{
    /// <summary>An interface providing information on a ProgressableObject</summary>
    public interface IProgressInfo
    {
        string Name { get; }
        string Description { get; }
        float Progress { get; }
    }
}