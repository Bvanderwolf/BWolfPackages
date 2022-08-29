// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>A stat modifier that, over a given amount of time, increase/decreases given amount of value</summary>
    public class TimedPointStatModifier : PointStatModifier
    {
        [Min(0)]
        private float time = 0;

        [Min(0)]
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

        /// <summary>Returns whether all time for this modifier has been passed</summary>
        public override bool Finished
        {
            get { return timePassed >= time; }
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier using the timed modifier info scriptable object
        /// </summary>
        public TimedPointStatModifier(TimedModifierInfoSO info)
        {
            Name = info.name;
            time = info.time;
            value = info.value;
            IncreasesValue = info.increasesValue;
            ModifiesCurrent = info.modifiesCurrent;
            ModifiesCurrentWithMax = info.modifiesCurrentWithMax;
            IsStackable = info.canStack;
        }

        /// <summary>Modifies system stat based on given time and time passed</summary>
        public override void Modify(PointStatSystem system)
        {
            if (time == 0f)
            {
                //if time is 0 no calculations have to be done and value can just modify the system once
                if (ModifiesCurrent)
                {
                    system.ModifyCurrent(this, IncreasesValue ? value : -value);
                }
                else
                {
                    system.ModifyMax(this, IncreasesValue ? value : -value);
                }
                return;
            }

            timePassed += UnityEngine.Time.deltaTime;
            currentValue = (int)(timePassed / time * value);
            if (currentValue != valueModified)
            {
                int difference = Mathf.Abs(currentValue - valueModified);
                valueModified += difference;

                if (currentValue > value)
                {
                    //handle overshot with large numbers and short times
                    int overShot = currentValue - value;
                    difference -= overShot;
                }

                if (ModifiesCurrent)
                {
                    system.ModifyCurrent(this, IncreasesValue ? difference : -difference);
                }
                else
                {
                    system.ModifyMax(this, IncreasesValue ? difference : -difference);
                    if (ModifiesCurrentWithMax && !system.IsFull)
                    {
                        system.ModifyCurrent(this, IncreasesValue ? difference : -difference);
                    }
                }
            }
        }

        /// <summary>Returns a string representation of this TimedStatModifier</summary>
        public override string ToString()
        {
            return $"TimedStatModifier[name: {Name}, time: {time}, valuePerSecond: {value}, increase: {IncreasesValue}, modifiesCurrent: {ModifiesCurrent}, modifiesCurrentWithMax: {ModifiesCurrentWithMax}, canStack: {IsStackable}]";
        }
    }
}