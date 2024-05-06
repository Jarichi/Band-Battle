using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.TerrainTools;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    private float lifetime;

    private float timeStart;
    private float currentTime;

    [SerializeField]
    private float yMove;

    

    private void Start()
    {
        timeStart = Time.time;
    }

    private void Update()
    {

        if (gameObject != null)
        {
            MoveNote();
            Lifetime();
        }
        else return;
        
    }


    private void Lifetime()
    {
        //if difference between timeStart and currentTime > lifetime, destroy.

        currentTime = Time.time;
        float timeDelta = currentTime - timeStart;
     
        if (timeDelta > lifetime)
        {
            Destroy(gameObject);
            return;
        }
        

    }

    private void MoveNote()
    {
        transform.localPosition -= new Vector3(0f, yMove, 0f);
    }

}
     