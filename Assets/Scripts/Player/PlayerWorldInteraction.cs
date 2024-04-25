using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWorldInteraction : MonoBehaviour
{
    private PlayerCombat combat;

    public void Start()
    {
        combat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
