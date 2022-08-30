namespace BWolf.Utilities
{
    /// <summary>
    /// Defines a method that interpolates between an initial and target
    /// value given a certain percentage.
    /// </summary>
    /// <typeparam name="T">The type of object to do the interpolation between.</typeparam>
    public delegate T LerpFunction<T>(T initial, T target, float percentage);
}
