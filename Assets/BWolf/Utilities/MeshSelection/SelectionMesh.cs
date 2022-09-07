using System;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    /// <summary>
    /// A behaviour that generates a collision mesh to callback trigger interactions.
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public class SelectionMesh : MonoBehaviour
    {
        /// <summary>
        /// Fired when a game object has triggered the mesh.
        /// </summary>
        private Action<GameObject> _selected;

        /// <summary>
        /// A predicate function to determine whether to select.
        /// </summary>
        private Func<GameObject, bool> _predicate;

        /// <summary>
        /// The mesh collider reference.
        /// </summary>
        private MeshCollider _collider;

        /// <summary>
        /// The default lifespan of the behaviour.
        /// </summary>
        private const float DEFAULT_LIFESPAN = 0.02f;

        /// <summary>
        /// Sets up the collider reference.
        /// </summary>
        private void Awake() => _collider = GetComponent<MeshCollider>();

        /// <summary>
        /// Draws the mesh in the scene.
        /// </summary>
        private void OnDrawGizmos()
        {
            using (new GizmoColor(Color.red))
                Gizmos.DrawMesh(_collider.sharedMesh);
        }

        /// <summary>
        /// Fires the selected event if no predicate is set or the predicate
        /// resolves to a true value.
        /// </summary>
        /// <param name="other">The other collider.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (_predicate == null && _predicate.Invoke(other.gameObject))
                _selected.Invoke(other.gameObject);
        }

        public static void Generate(
            Vector3[] vertices, 
            int[] triangles, 
            float lifeSpan,
            Func<GameObject, bool> predicate, 
            Action<GameObject> selected)
        {
            Mesh mesh = new Mesh { vertices = vertices, triangles = triangles };
            mesh.Optimize();
            mesh.RecalculateNormals();
            
            GameObject gameObject = new GameObject("~SelectionMesh");
            SelectionMesh instance = gameObject.AddComponent<SelectionMesh>();
            instance._collider.sharedMesh = mesh;
            instance._collider.convex = true;
            instance._collider.isTrigger = true;
            instance._predicate = predicate;
            instance._selected = selected;
            
            Destroy(gameObject, lifeSpan);
        }
        
        public static void Generate(Vector3[] vertices, int[] triangles, float lifeSpan, Action<GameObject> selected)
            => Generate(vertices, triangles,lifeSpan, null, selected);

        public static void Generate(
            Vector3[] vertices,
            int[] triangles,
            Func<GameObject, bool> predicate,
            Action<GameObject> selected)
            => Generate(vertices, triangles,DEFAULT_LIFESPAN, predicate, selected);

        public static void Generate(Vector3[] vertices, int[] triangles, Action<GameObject> selected)
            => Generate(vertices, triangles, DEFAULT_LIFESPAN, null, selected);
    }
}
