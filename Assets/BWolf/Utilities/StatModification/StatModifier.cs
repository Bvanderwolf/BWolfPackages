using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>Base class for modifiers, defining all the generic members</summary>
    public abstract class StatModifier
    {
        [SerializeField, Tooltip("Identifier for stat modifier")]
        protected string name = "";

        [SerializeField, Tooltip("Can this modifier stack with modifiers with the same name?")]
        protected bool canStack = true;

        [SerializeField, Tooltip("Will this modifier increase or subtract")]
        protected bool increase = false;

        [SerializeField, Tooltip("Will this modifier modify current value or max value?")]
        protected bool modifiesCurrent = false;

        [SerializeField, Tooltip("Does this modifier, when max is modified, also modify current?")]
        protected bool modifiesCurrentWithMax = true;

        public abstract bool Finished { get; }
        public abstract StatModifier Clone { get; }

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

        public abstract void Modify(StatSystem system);
    }
}