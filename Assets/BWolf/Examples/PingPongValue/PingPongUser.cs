using System.Collections;
using BWolf.Utilities;
using UnityEngine;

public class PingPongUser : MonoBehaviour
{
    [SerializeField]
    private PingPong _shaking;

    // Start is called before the first frame update
    void Start()
    {
        IEnumerator routine = _shaking.Await(SetXPosition);
        StartCoroutine(routine);
    }

    private void SetXPosition(float newXPosition)
    {
        Vector3 position = transform.position;
        position.x = newXPosition;
        transform.position = position;
    }
}
