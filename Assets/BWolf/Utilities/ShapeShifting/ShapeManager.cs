using BWolf.Behaviours;
using UnityEngine;

namespace BWolf.Utilities.ShapeShifting
{
    public class ShapeManager : SingletonBehaviour<ShapeManager>
    {
        [SerializeField]
        private GameObject circleShapePrefab = null;

        [SerializeField]
        private GameObject rectangleShapePrefab = null;

        private Shape circle;
        private Shape rectangle;

        private void Awake()
        {
            circle = new Shape(circleShapePrefab);
            rectangle = new Shape(rectangleShapePrefab);

            CreateShape(ShapeType.Rectange);
        }

        /// <summary>Creates a new shape of given type and returns the instance</summary>
        public GameObject CreateShape(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Circle:
                    return circle.Instantiate();

                case ShapeType.Rectange:
                    return rectangle.Instantiate();

                default:
                    return null;
            }
        }

        /// <summary>Returns a new shape template based on given type</summary>
        public Shape GetShapeTemplate(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Circle:
                    return circle.GetTemplate();

                case ShapeType.Rectange:
                    return rectangle.GetTemplate();

                default:
                    return null;
            }
        }
    }

    public enum ShapeType
    {
        Circle,
        Rectange
    }
}