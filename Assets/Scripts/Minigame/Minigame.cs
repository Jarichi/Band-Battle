using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Minigame : SongHandler
{
    private bool active;

    //temp, might not be nessecary
    //private bool started = false;
    
    private PlayerMovement movement;
    private PlayerCombat combat;
    private GameObject weapon;
    private SpriteRenderer ui;
    [SerializeField]
    [Range(0f, 3f)]
    private float startupAnimationLength;

    private void Start()
    {
        print("start : Minigame");
    }
    private void Update()
    {
        //print(active);
        //print(active);
        if (active) {
            //check if you want to enter combat mode
            if (Input.GetKeyDown(KeyCode.C))
            {
                EngageCombat();
                print(gameObject);
                Destroy(gameObject); gameObject.SetActive(false);

            }

            //run the minigame only when in playing mode
            //print("currently active!");
            RunRhythmGame();

        }

        //print("inactive");
    }

    public void StartMinigame(GameObject player, GameObject weapon)
    {
        print("StartMinigame called");
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();

        print("movement and combat set");

        
        

        ui = player.transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.weapon = weapon;

        print("weapon set");
        active = true;
        print("active set to " + active);
        //ui.enabled = true;
        movement.Disable();
        print("movement disablked");
    }

    private void EngageCombat()
    {
        print("EngageCombat called");
        ui.enabled= false;
        active = false;
        combat.Engage(weapon, movement, GetCombatAnimationName(), startupAnimationLength);
        GetComponent<Instrument>().enabled = false;

    }

    protected abstract string GetCombatAnimationName();
}
