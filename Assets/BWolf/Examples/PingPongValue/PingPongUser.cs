using System.Collections;
using System.Collections.Generic;
using BWolf.Utilities;
using UnityEngine;

public class PingPongUser : MonoBehaviour
{
    [SerializeField]
    private PingPong _shaking;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnumerateShake());
    }

    private IEnumerator EnumerateShake()
    {
        yield return _shaking.Yield(value =>
        {
            Vector3 position = transform.position;
            position.x = value;
            transform.position = position;
        });
    }
}
