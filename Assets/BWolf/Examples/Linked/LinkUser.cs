using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkUser : MonoBehaviour
{
    private Linked<string> _dialogue;
    
    // Start is called before the first frame update
    void Start()
    {
        _dialogue = new Linked<string>().AddLink("Hello stranger.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
