using BWolf.Utilities.ShapeShifting;
using UnityEngine;

namespace BWolf.Examples.ShapeShifting
{
    public class ShapeShiftTester : MonoBehaviour
    {
        [SerializeField]
        private float shiftTime = 1f;

        private ShapeShifter shifter;

        private void Start()
        {
            shifter = ShapeManager.Instance.CreateShape(ShapeType.Circle).GetComponent<ShapeShifter>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ShiftToCircle();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ShiftToRectangle();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                ShiftToTriangle();
            }
        }

        public void ShiftToCircle()
        {
            shifter.Shift(ShapeType.Circle, shiftTime);
        }

        public void ShiftToRectangle()
        {
            shifter.Shift(ShapeType.Rectange, shiftTime);
        }

        public void ShiftToTriangle()
        {
            shifter.Shift(ShapeType.Triangle, shiftTime);
        }
    }
}