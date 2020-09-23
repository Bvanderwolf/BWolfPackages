// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.ShapeShifting
{
    public class ShapeShifter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float defaultShiftTime = 1f;

        [SerializeField]
        private ShapeType defaultShape = ShapeType.Circle;

        private Shape currentShape;
        private bool shifting;

        /// <summary>The shape this ShapeShifter started with</summary>
        public ShapeType DefaultShape
        {
            get { return defaultShape; }
        }

        /// <summary>Sets current shape</summary>
        public void SetShape(Shape shape)
        {
            currentShape = shape;
        }

        /// <summary>Starts the shifting progress towards given new shift type</summary>
        public void Shift(ShapeType type)
        {
            Shift(type, defaultShiftTime);
        }

        /// <summary>Starts the shifting progress towards given new shift type in given ammount of time</summary>
        public void Shift(ShapeType type, float time)
        {
            if (shifting)
            {
                return;
            }

            Shape newShape = ShapeManager.Instance.GetShapeTemplate(type);
            if (newShape.PartCount != currentShape.PartCount)
            {
                Debug.LogWarning("Shapes part count isn't corresponding :: won't shift");
                return;
            }

            if (newShape.Type == currentShape.Type)
            {
                return;
            }

            StartCoroutine(ShiftShape(newShape, time));
        }

        /// <summary>Shifts this game object and its children towards given new shape over </summary>
        private IEnumerator ShiftShape(Shape newShape, float time)
        {
            shifting = true;

            LerpValue<Shape> shift = new LerpValue<Shape>(currentShape, newShape, time);
            Shape output = new Shape(currentShape);
            while (shift.Continue())
            {
                Shape.Shift(shift.start, shift.end, output, shift.perc).AssignValues(transform);
                yield return null;
            }

            SetShape(newShape);
            shifting = false;
        }
    }
}