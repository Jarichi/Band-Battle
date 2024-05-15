using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWorldInteraction : MonoBehaviour
{
    private PlayerCombat combat;
    private PlayerInputController input;
    private Instrument inRange;

    public void Start()
    {
        combat = GetComponent<PlayerCombat>();
        input = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        if (input.RestartPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (input.InteractPressed)
        {
            if (inRange!= null)
            {
                inRange.Interact(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Instrument>(out var inRangeOfInstrument))
        {
            inRange = inRangeOfInstrument;
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        inRange = null;
    }
}
