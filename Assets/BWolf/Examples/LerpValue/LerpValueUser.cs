using System;
using System.Collections;
using BWolf.Utilities;
using UnityEngine;
public class LerpValueUser : MonoBehaviour
{
    private enum MethodToUse
    {
        [InspectorName("Extension Method")]
        EXTENSION_METHOD,
        
        [InspectorName("Static Method")]
        STATIC_METHOD
    }
    
    [SerializeField]
    private Vector3Lerp moving;

    [SerializeField]
    private MethodToUse _methodToUse;
    
    private void Awake()
    {
        moving.easingFunction = EasingFunctions.easeInSine;
    }

    void Start()
    {
        switch (_methodToUse)
        {
            case MethodToUse.EXTENSION_METHOD:
                IEnumerator extensionMethod = moving.Await(newPosition => transform.position = newPosition);
                StartCoroutine(extensionMethod);
                break;
            
            case MethodToUse.STATIC_METHOD:
                Vector3 initial = moving.initial;
                Vector3 target = moving.target;
                float time = moving.TotalTime;
                
                IEnumerator staticMethod = LerpOf<Vector3>.Await(initial, target, Vector3.Lerp, newPosition =>
                {
                    transform.position = newPosition;
                }, time);
                
                StartCoroutine(staticMethod);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
}
