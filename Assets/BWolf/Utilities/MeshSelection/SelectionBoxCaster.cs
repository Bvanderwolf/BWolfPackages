using System;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    /// <summary>
    /// Can be used to cast a selection mesh from a camera into a scene
    /// to find selected 3D game objects based on a selection box on screen.
    /// </summary>
    public class SelectionBoxCaster
    {
        /// <summary>
        /// Fired when a game object has been selected by the
        /// selection mesh cast.
        /// </summary>
        public event Action<GameObject> Selected;
            
        /// <summary>
        /// The camera used to cast from.
        /// </summary>
        private readonly Camera _camera;

        /// <summary>
        /// The hit points from rays cast into the scene.
        /// </summary>
        private readonly Vector3[] _hitPoints;

        /// <summary>
        /// The ray origins.
        /// </summary>
        private readonly Vector3[] _rayOrigins;

        /// <summary>
        /// The vertices of the to be generated selection mesh.
        /// </summary>
        private readonly Vector3[] _vertices;
        
        /// <summary>
        /// The triangles of the to be generated selection mesh.
        /// </summary>
        private readonly int[] triangles = new int[]
        {
            0, 2, 1,
            0, 3, 2,
            2, 3, 4,
            2, 4, 5,
            1, 2, 5,
            1, 5, 6,
            0, 7, 4,
            0, 4, 3,
            5, 4, 7,
            5, 7, 6,
            0, 6, 7,
            0, 1, 6
        };
        
        /// <summary>
        /// Creates a new instance of this class given a camera to
        /// cast from.
        /// </summary>
        /// <param name="camera">The camera used to cast from.</param>
        public SelectionBoxCaster(Camera camera)
        { 
            _camera = camera;
            _hitPoints = new Vector3[4];
            _rayOrigins = new Vector3[4];
            _vertices = new Vector3[8];
        }

        /// <summary>
        /// Casts the selection mesh into the scene based on the given corners
        /// of the selection box on screen. Returns whether the selection mesh
        /// could be created. 
        /// </summary>
        /// <param name="selectionBoxCorners">The corners of the selection box on screen.</param>
        /// <returns>Whether the selection mesh could be created.</returns>
        public bool Cast(Vector2[] selectionBoxCorners)
        {
            if (!CollectSelectionData(selectionBoxCorners))
                return false;
            
            GenerateSelectionMesh();
            return true;
        }

        /// <summary>
        /// Sets up the selection mesh data by ray casting from the corners
        /// of the selection box into the scene and storing the results. Returns
        /// whether all ray casts hit.
        /// </summary>
        /// <param name="selectionBoxCorners">The corners of the selection box on screen.</param>
        /// <returns>Whether all ray casts hit.</returns>
        private bool CollectSelectionData(Vector2[] selectionBoxCorners)
        {
            for (int i = 0; i < selectionBoxCorners.Length; i++)
            {
                Ray ray = _camera.ScreenPointToRay(selectionBoxCorners[i]);
                if (!Physics.Raycast(ray, out RaycastHit hit))
                    return false;

                _hitPoints[i] = hit.point;
                _rayOrigins[i] = ray.origin;
            }

            return true;
        }

        /// <summary>
        /// Generates the selection mesh based on the selection data collected.
        /// </summary>
        private void GenerateSelectionMesh()
        {
            for (int i = 0; i < _hitPoints.Length; i++)
                _vertices[i] = _hitPoints[i];

            for (int i = 4; i < _vertices.Length; i++)
                _vertices[i] = _rayOrigins[i - 4];

            SelectionMesh.Generate(_vertices, triangles, Selected);
        }
    }
}
