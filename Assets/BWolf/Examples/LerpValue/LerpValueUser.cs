using System.Collections;
using BWolf.Utilities;
using UnityEngine;
public class LerpValueUser : MonoBehaviour
{
    [SerializeField]
    private Vector3Lerp moving;

    void Start()
    {
        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp()
    {
        yield return moving.Yield(newPosition => transform.position = newPosition);
    }
}
