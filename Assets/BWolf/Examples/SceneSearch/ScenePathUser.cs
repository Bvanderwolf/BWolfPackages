using BWolf.SceneSearch;
using UnityEngine;

public class ScenePathUser : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            using (new Timer(true))
                Debug.Log(ScenePath.Find("Child (3)"));
        }
       
        if (Input.GetKeyDown(KeyCode.F))
        {
            using (new Timer(true))
                Debug.Log(GameObject.Find("Child (3)"));
        }
    }
}
