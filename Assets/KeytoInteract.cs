using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeytoInteract : MonoBehaviour
{
    public bool inRange = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider Microphone)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider Microphone)
    {
        inRange = false;
    }
}
