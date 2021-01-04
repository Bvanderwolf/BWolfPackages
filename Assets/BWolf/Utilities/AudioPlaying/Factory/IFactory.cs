// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

namespace BWolf.Utilities
{
    /// <summary>
    /// Represents a factory.
    /// </summary>
    /// <typeparam name="T">Specifies the type to create.</typeparam>
    public interface IFactory<T>
    {
        T Create();
    }
}