namespace BWolf.Utilities.StatModification
{
    /// <summary>Base class for modifiers, defining all the generic members</summary>
    public abstract class StatModifier
    {
        protected string name;
        protected bool canStack;
        protected bool increase;
        protected bool modifiesCurrent;
        protected bool modifiesCurrentWithMax;

        public string Name
        {
            get { return name; }
        }

        public bool CanStack
        {
            get { return canStack; }
        }

        public bool Increase
        {
            get { return increase; }
        }

        public bool ModifiesCurrent
        {
            get { return modifiesCurrent; }
        }

        public bool ModifiesCurrentWithMax
        {
            get { return modifiesCurrentWithMax; }
        }

        public abstract bool Finished { get; }

        public abstract void Modify(StatSystem system);
    }
}