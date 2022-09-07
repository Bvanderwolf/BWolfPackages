using BWolf.MeshSelecting;
using UnityEngine;

public class SelectionToggler : MonoBehaviour
{
    private void Awake()
    {
        //MeshSelection.SelectionCondition = GetIsSelectableObject;
    }

    private bool GetIsSelectableObject(Collider objectCollider)
    {
        return objectCollider.gameObject.layer == LayerMask.NameToLayer("Obstacle");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            MeshSelection.SetActive(!MeshSelection.IsActive);
    }
}
