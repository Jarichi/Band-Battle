using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    private float lifetime;

    private float timeStart;
    private float currentTime;
    private float timeDelta;

    public float velocity;


    //temp
    Vector3 targetPosition;
    
    private void Start()
    {
        timeStart = Time.time;

        targetPosition = transform.localPosition + new Vector3(0f, -6f, 0f);  

    }

    //@50fps
    private void FixedUpdate()
    {
        if (gameObject != null)
        {
            MoveNote();
            Lifetime();
        }
    }

    private void Lifetime()
    {
        currentTime = Time.time;
        timeDelta = currentTime - timeStart;

        if (timeDelta > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void MoveNote()
    {

        //velocity = songHandler.bpm;

        var step = velocity * Time.fixedDeltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
    }


}

     