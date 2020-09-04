// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.ShapeShifting
{
    /// <summary>Class containing the functionailities pertaining to a shape</summary>
    public class Shape : IShape<Shape>
    {
        private Vector3[] positions;
        private Quaternion[] orientations;
        private Vector3[] scales;

        private GameObject prefabRef;

        /// <summary>The ammount of parts this shape contains. This needs to be equal to the new shape when shifting</summary>
        public int PartCount
        {
            get { return prefabRef.transform.childCount; }
        }

        public ShapeType Type { get; private set; }

        public Shape(Shape shape) : this(shape.prefabRef)
        {
        }

        public Shape(GameObject prefab)
        {
            Transform tf = prefab.transform;
            int childCount = tf.childCount;

            positions = new Vector3[childCount];
            orientations = new Quaternion[childCount];
            scales = new Vector3[childCount];

            for (int ci = 0; ci < childCount; ci++)
            {
                positions[ci] = tf.GetChild(ci).localPosition;
                orientations[ci] = tf.GetChild(ci).localRotation;
                scales[ci] = tf.GetChild(ci).localScale;
            }

            prefabRef = prefab;
            Type = prefab.GetComponent<ShapeShifter>().DefaultShape;
        }

        /// <summary>Assigns shape values to all parts based on given root transform. Use when shifting</summary>
        public void AssignValues(Transform root)
        {
            for (int ci = 0; ci < root.childCount; ci++)
            {
                root.GetChild(ci).localPosition = positions[ci];
                root.GetChild(ci).localRotation = orientations[ci];
                root.GetChild(ci).localScale = scales[ci];
            }
        }

        /// <summary>Returns a new Shape based on the original prefab values</summary>
        public Shape GetTemplate()
        {
            return new Shape(prefabRef);
        }

        /// <summary>Creates a new shape gameobject instance in the scene using the stored prefab values</summary>
        public GameObject Instantiate()
        {
            GameObject instance = GameObject.Instantiate(prefabRef);
            instance.GetComponent<ShapeShifter>().SetShape(this);
            return instance;
        }

        /// <summary>Shifts an output shape by linearly interpolating between start and end shapes by given perc. Make sure to use the returned output reference to assign the new values</summary>
        public static Shape Shift(Shape start, Shape end, Shape output, float perc)
        {
            int count = start.PartCount;

            for (int i = 0; i < count; i++)
            {
                output.positions[i] = Vector3.Lerp(start.positions[i], end.positions[i], perc);
                output.orientations[i] = Quaternion.Lerp(start.orientations[i], end.orientations[i], perc);
                output.scales[i] = Vector3.Lerp(start.scales[i], end.scales[i], perc);
            }

            return output;
        }
    }
}