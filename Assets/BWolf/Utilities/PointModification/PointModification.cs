using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.PointModifications
{
    public struct PointModification
    {
        public int value;

        public bool modifiesCurrent;

        public bool modifiesCurrentWithMax;

        public PointModification(int value)
        {
            this.value = value;
            this.modifiesCurrent = true;
            this.modifiesCurrentWithMax = false;
        }

        public static PointModification None => new PointModification(0);
    }
}
