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
    private float timeDelta;

    public int velocity {  private get; set; }

    [SerializeField]
    private float yMove;



    private void Start()
    {
        timeStart = Time.time;

        
    }

    private void FixedUpdate()
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
        timeDelta = currentTime - timeStart;

        //timeDelta = (timeDelta + Time.deltaTime*yMove);

        if (timeDelta > lifetime)
        {
            Destroy(gameObject);
            return;
        }


    }

    private void MoveNote()
    {
        transform.localPosition -= new Vector3(0f, yMove * Time.deltaTime, 0f);
    }

}

     