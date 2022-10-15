using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Timer : IDisposable
{
    public readonly bool logOnDispose;

    private readonly Stopwatch _stopwatch;

    public Timer(bool logOnDispose)
    {
        this.logOnDispose = logOnDispose;
        
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }


    public void Dispose()
    {
        _stopwatch.Stop();
        
        if (logOnDispose)
            Debug.Log($"Elapsed: {_stopwatch.Elapsed}");
    }
}
