// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>Interface to be implemented by Monobehaviour classes that can provide a scene transition for the scene transition system</summary>
    public interface ITransitionProvider
    {
        string TransitionName { get; }

        IEnumerator Outro();

        IEnumerator Intro();

        void OnProgressUpdated(float perc);
    }
}