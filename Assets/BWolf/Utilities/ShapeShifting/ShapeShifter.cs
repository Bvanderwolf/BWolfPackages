using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.ShapeShifting
{
    public class ShapeShifter : MonoBehaviour
    {
        [SerializeField]
        private float defaultShiftTime = 1f;

        private Shape currentShape;

        /// <summary>Sets current shape</summary>
        public void SetShape(Shape shape)
        {
            if (shape != currentShape)
            {
                currentShape = shape;
            }
        }

        /// <summary>Starts the shifting progress towards given new shift type</summary>
        private void Shift(ShapeType type)
        {
            Shift(type, defaultShiftTime);
        }

        /// <summary>Starts the shifting progress towards given new shift type in given ammount of time</summary>
        private void Shift(ShapeType type, float time)
        {
            Shape newShape = ShapeManager.Instance.GetShapeTemplate(type);
            if (currentShape == null) { return; }
            if (newShape.PartCount != currentShape.PartCount)
            {
                Debug.LogWarning("Shapes part count isn't corresponding");
                return;
            }

            if (currentShape == newShape)
            {
                return;
            }

            StartCoroutine(ShiftShape(newShape, time));
        }

        /// <summary>Shifts this game object and its children towards given new shape over </summary>
        private IEnumerator ShiftShape(Shape newShape, float time)
        {
            LerpValue<Shape> shift = new LerpValue<Shape>(currentShape, newShape, time);
            Shape output = new Shape(currentShape);
            while (shift.Continue())
            {
                Shape.Shift(shift.start, shift.end, output, shift.perc).AssignValues(transform);
                yield return null;
            }
            SetShape(newShape);
        }
    }
}