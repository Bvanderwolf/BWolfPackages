using BWolf.Behaviours;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.ShapeShifting
{
    public class ShapeManager : SingletonBehaviour<ShapeManager>
    {
        [SerializeField]
        private GameObject[] shapePrefabs = null;

        private Dictionary<ShapeType, Shape> shapes = new Dictionary<ShapeType, Shape>();

        private void Awake()
        {
            foreach (GameObject shape in shapePrefabs)
            {
                shapes.Add(shape.GetComponent<ShapeShifter>().DefaultShape, new Shape(shape));
            }

            CreateShape(ShapeType.Rectange);
        }

        /// <summary>Creates a new shape of given type and returns the instance</summary>
        public GameObject CreateShape(ShapeType type)
        {
            return shapes[type].Instantiate();
        }

        /// <summary>Returns a new shape template based on given type</summary>
        public Shape GetShapeTemplate(ShapeType type)
        {
            return shapes[type].GetTemplate();
        }
    }

    public enum ShapeType
    {
        Circle,
        Rectange
    }
}