using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerLine : MonoBehaviour
{
    [SerializeField]
    private InputCollider[] colliders;
    public float marginOfError;
    private Vector2 initialPosition;
    private PlayerInput input;

    private SpriteRenderer SpriteRenderer;
    private Color baseColour = Color.white;
    private float fadeTime = 0.15f;

    private Coroutine coroutine;

    private string fmodParameterName;

    public double Score = 0;

    [SerializeField]
    private double ScoreIncrease = 1;
    [SerializeField]
    private double ScoreDecrease = .5;
    private PlayerWorldInteraction player;
    private Game game;
    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        input = transform.root.GetComponent<Player>().Input;
        input.actions["Play Note 1"].performed += OnInput1;
        input.actions["Play Note 2"].performed += OnInput2;
        input.actions["Play Note 3"].performed += OnInput3;
        input.actions["Play Note 4"].performed += OnInput4;
        player = transform.root.GetComponent<Player>().InGameEntity.GetComponent<PlayerWorldInteraction>();
        Debug.Log(player);
        fmodParameterName = gameObject.transform.parent.GetComponentInParent<PlayerWorldInteraction>().ChosenInstrument.fmodParameterName;
        game = Game.Instance;
    }

    private void OnDisable()
    {
        input.actions["Play Note 1"].performed -= OnInput1;
        input.actions["Play Note 2"].performed -= OnInput2;
        input.actions["Play Note 3"].performed -= OnInput3;
        input.actions["Play Note 4"].performed -= OnInput4;
    }

    private void OnInput1(InputAction.CallbackContext ctx)
    {
        initialPosition.x = -1.5f;
        ToggleHitbox(0);
        FadeColour(Color.red);
    }

    private void OnInput2(InputAction.CallbackContext ctx)
    {
        initialPosition.x = -0.5f;
        ToggleHitbox(1);
        FadeColour(Color.blue);
    }

    private void OnInput3(InputAction.CallbackContext ctx)
    {
        initialPosition.x = 0.5f;
        ToggleHitbox(2);
        FadeColour(Color.yellow);
    }

    private void OnInput4(InputAction.CallbackContext ctx)
    {
        initialPosition.x = 1.5f;
        ToggleHitbox(3);
        FadeColour(Color.green);
    }

    public void OnHit(NoteController note)
    {
        player.AddScore(ScoreIncrease);
        game.EnableAudioChannel(fmodParameterName);
        note.Despawn();
    }

    public void OnMiss(NoteController note = null)
    {
        game.DisableAudioChannel(fmodParameterName);
        player.DecreaseScore(ScoreDecrease);
        Score = Math.Max(Score, 0);
        if (note != null)
            note.Despawn();
    }

    private void FadeColour(Color TargetColour)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(c_FadeColour(TargetColour, fadeTime));
    }

    private void ToggleHitbox(int index)
    {
        var hitbox = colliders[index];
        hitbox.enabled = true;

        StartCoroutine(c_DisableHitbox(hitbox));
    }

    private IEnumerator c_DisableHitbox(InputCollider hitbox)
    {
        yield return new WaitForSeconds(marginOfError);
        if (hitbox.enabled && !hitbox.hasCollision)
        {
            OnMiss();
        }
        hitbox.enabled = false;
    }

    private IEnumerator c_FadeColour(Color TargetColour, float fadeTime)
    {
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float time = timer / fadeTime;
            SpriteRenderer.color = Color.Lerp(TargetColour, baseColour, time);

            yield return null;
        }
        SpriteRenderer.color = baseColour;
    }

    public InputCollider GetCollider(int position)
    {
        return colliders[position];
    }

}
[Serializable]
public class InputCollider
{
    [SerializeField]
    public BoxCollider2D collider;
    [SerializeField]
    public bool enabled;
    [SerializeField]
    internal bool hasCollision;
}

