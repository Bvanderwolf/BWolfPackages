using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>A stat modifier than, over a given amount of time, increase/decrease given amount of value</summary>
    [System.Serializable]
    public class TimedStatModifier : StatModifier
    {
        [SerializeField, Min(0), Tooltip("Time it takes for this modifier to finish modifying the stat system")]
        private float time = 0;

        [SerializeField, Min(0), Tooltip("The ammount of value it will modify over given amount of time")]
        private int value = 0;

        private float timePassed;

        private int valueModified;
        private int currentValue;

        public float Time
        {
            get { return time; }
        }

        public int Value
        {
            get { return value; }
        }

        /// <summary>Returns whether the given time has been reached</summary>
        public override bool Finished
        {
            get { return timePassed >= time; }
        }

        /// <summary>Returns whether this modifier has a valid time</summary>
        public bool HasValidTime
        {
            get { return time >= 0; }
        }

        /// <summary>Returns whether this modifier has a valid value</summary>
        public bool HasValidValue
        {
            get { return value > 0; }
        }

        /// <summary>Returns a new instance of this modifier. Use this if you have stored a modifier and want to re-use it</summary>
        public override StatModifier Clone
        {
            get { return new TimedStatModifier(name, time, value, increase, modifiesCurrent, modifiesCurrentWithMax, canStack); }
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="modifiesCurrent">Will this modifier modify current value or max value?</param>
        /// <param name="canStack">Can this modifier stack with modifiers with the same name?</param>
        public TimedStatModifier(string name, float time, int value, bool increase, bool modifiesCurrent, bool modifiesCurrentWithMax, bool canStack)
        {
            this.name = name;
            this.time = time;
            this.value = value;
            this.increase = increase;
            this.modifiesCurrent = modifiesCurrent;
            this.modifiesCurrentWithMax = modifiesCurrentWithMax;
            this.canStack = canStack;
        }

        /// <summary>The amount the value should increase or decrease</summary>
        public void ModifyValue(int amount)
        {
            value += amount;
        }

        /// <summary>The amount the time should increase or decrease</summary>
        public void ModifyTime(int amount)
        {
            time += amount;
        }

        /// <summary>Modifies system stat based on given time and time passed</summary>
        public override void Modify(StatSystem system)
        {
            if (time == 0f)
            {
                if (modifiesCurrent)
                {
                    system.ModifyCurrent(this, increase ? value : -value);
                }
                else
                {
                    system.ModifyMax(this, increase ? value : -value);
                }
                return;
            }

            timePassed += UnityEngine.Time.deltaTime;
            currentValue = (int)(timePassed / time * value);
            if (currentValue != valueModified)
            {
                int difference = Mathf.Abs(currentValue - valueModified);
                valueModified += difference;
                if (modifiesCurrent)
                {
                    system.ModifyCurrent(this, increase ? difference : -difference);
                }
                else
                {
                    system.ModifyMax(this, increase ? difference : -difference);
                    if (modifiesCurrentWithMax && !system.IsFull)
                    {
                        system.ModifyCurrent(this, increase ? difference : -difference);
                    }
                }
            }
        }

        /// <summary>Returns identifaction string for this timed stat modifier instance</summary>
        public override string ToString()
        {
            return $"TimedStatModifier[name: {name}, time: {time}, valuePerSecond: {value}, increase: {increase}, modifiesCurrent: {modifiesCurrent}, modifiesCurrentWithMax: {modifiesCurrentWithMax}, canStack: {canStack}]";
        }
    }
}