using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sequencer : MonoBehaviour
{
    public float interval = 0.0f;

    [SerializeField]
    private UnityEvent[] _actions;
}
