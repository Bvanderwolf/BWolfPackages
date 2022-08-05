namespace BWolf.Utilities
{
    public abstract class LerpState : IContinuable
    {
        public abstract float TotalTime { get; }
        
        public abstract float Percentage { get; }
        
        public abstract float RemainingTime { get; }
        
        public abstract void Reset();
        
        public abstract bool Continue();
    }
}
