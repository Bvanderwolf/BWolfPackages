namespace BWolf.Utilities.StatModification
{
    /// <summary>Base class for modifiers to derive from to modify a stat system</summary>
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

        /// <summary>To be implemented to make the stat system able to check whether to remove this modifier or not</summary>
        public abstract bool Finished { get; }

        /// <summary>To be implemented to make the stat system modify itself using this method</summary>
        public abstract void Modify(StatSystem system);
    }
}