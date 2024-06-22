using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerRhythm : MonoBehaviour
{
    private double score = 0;
    private PlayerInput input;
    private PlayerEntity playerEntity;
    private Instrument inRange;
    public Instrument ChosenInstrument {get; private set;}
    public void Start()
    {
        playerEntity = GetComponent<PlayerEntity>();
        input = playerEntity.Player.Input;
        ChosenInstrument = null;
        input.actions["Overworld Interact"].performed += TryInteract;
        Game.Instance.Rhythm.rhythmStartEvent.AddListener(OnRhythmStart);
    }

    private void OnDisable()
    {
        input.actions["Overworld Interact"].performed -= TryInteract;
        Game.Instance.Rhythm.rhythmStartEvent.RemoveListener(OnRhythmStart);
    }

    private void TryInteract(InputAction.CallbackContext ctx)
    {
        if (Game.Instance.GetCurrentPhase() != Game.Phase.ChooseInstrument)
        return;

        if (inRange != null)
        {
            ChosenInstrument = inRange;
            playerEntity.Movement.Disable();
            if (PlayerList.Get().All(player =>
            {
                var interaction = player.Entity.GetComponent<PlayerRhythm>();
                return interaction.ChosenInstrument != null;
            }))
            {
                Game.Instance.StartPlayPhase();
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

    void OnRhythmStart()
    {
        var minigameObj = Instantiate(ChosenInstrument.minigame, transform);
        var minigame = minigameObj.GetComponent<Minigame>();
        minigame.OnRhythmStart(playerEntity.Player, ChosenInstrument.weapon);
    }

    public void AddScore(double score)
    {
        score = Math.Max(score, 0);
        this.score += score;
    }

    public void DecreaseScore (double score)
    {
        score = Math.Max(score, 0);
        this.score -= score;
    }

    public void SaveScoreToTotal()
    {
        playerEntity.Player.data.totalScore += (int)this.score;
    }

    public double GetScore()
    {
        return score;
    }
}
