// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>delegate called when a request starts to for example check whether a request with given target view id can be done</summary>
    public delegate bool RequestStartHandler(int targetViewId);
}