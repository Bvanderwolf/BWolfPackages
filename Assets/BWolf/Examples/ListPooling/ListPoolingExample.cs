using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BWolf.Utilities.ListPooling;

namespace BWolf.Examples.ListPooling
{
    public class ListPoolingExample : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            UseListOfGameObjects();
            UseListOfTransforms();

            UseListOfGameObjectsWithCapacity();
            UseListOfTransformsWithCapacity();

            UseListOfGameObjectsWithCollection();
            UseListOfTransformsWithCollection();
        }

        private void UseListOfGameObjects()
        {
            List<GameObject> gameObjectList = ListPoolService.Create<GameObject>();

            foreach (GameObject gameObject in gameObjectList)
            {
                //apply action on gameobject
            }

            ListPoolService.Dispose(gameObjectList);
        }

        private void UseListOfTransforms()
        {
            List<Transform> transformList = ListPoolService.Create<Transform>();

            foreach (Transform _transform in transformList)
            {
                //apply action on transform
            }

            ListPoolService.Dispose(transformList);
        }

        private void UseListOfGameObjectsWithCapacity()
        {
            List<GameObject> gameObjectList = ListPoolService.Create<GameObject>(5);

            foreach (GameObject gameObject in gameObjectList)
            {
                //apply action on gameobject
            }

            ListPoolService.Dispose(gameObjectList);
        }

        private void UseListOfTransformsWithCapacity()
        {
            List<Transform> transformList = ListPoolService.Create<Transform>(5);

            foreach (Transform _transform in transformList)
            {
                //apply action on gameobject
            }

            ListPoolService.Dispose(transformList);
        }

        private void UseListOfGameObjectsWithCollection()
        {
            //use collection of gameobjects found with tag using linq to queuery
            IEnumerable<GameObject> gameObjectCollection = GameObject.FindGameObjectsWithTag("MainCamera").Where(go => go.name == "Main Camera");
            List<GameObject> gameObjectList = ListPoolService.Create(gameObjectCollection);

            foreach (GameObject gameObject in gameObjectList)
            {
                //apply action on gameobject
            }

            ListPoolService.Dispose(gameObjectList);
        }

        private void UseListOfTransformsWithCollection()
        {
            //use ienumerable interface of transform to get a collection of the children of this object
            IEnumerable<Transform> transformCollection = transform.Cast<Transform>();
            List<Transform> transformList = ListPoolService.Create(transformCollection);

            foreach (Transform _transform in transformList)
            {
                //apply action on gameobject
            }

            ListPoolService.Dispose(transformList);
        }
    }
}