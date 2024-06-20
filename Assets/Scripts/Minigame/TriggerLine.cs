using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Rhythm;

public class TriggerLine : MonoBehaviour
{

    [SerializeField]
    private InputCollider[] colliders;
    public float marginOfError;
    private PlayerInput input;

    private SpriteRenderer SpriteRenderer;
    private Color baseColour = Color.white;
    private readonly float fadeTime = 0.15f;

    private Coroutine coroutine;

    private string instrumentId;

    public double Score = 0;

    [SerializeField]
    private double ScoreIncrease = 1;
    [SerializeField]
    private double ScoreDecrease = .5;
    private PlayerRhythm player;
    private Game game;

    //event
    public UnityEvent<Note> NoteCollideEvent;
    public UnityEvent NoCollisionEvent;
    public UnityEvent<Note, bool> NotePassEvent;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        input = transform.root.GetComponent<Player>().Input;
        input.actions["Play Note 1"].performed += OnInput1;
        input.actions["Play Note 2"].performed += OnInput2;
        input.actions["Play Note 3"].performed += OnInput3;
        input.actions["Play Note 4"].performed += OnInput4;
        player = transform.root.GetComponent<Player>().InGameEntity.GetComponent<PlayerRhythm>();
        Debug.Log(player);
        instrumentId = gameObject.transform.parent.GetComponentInParent<PlayerRhythm>().ChosenInstrument.id;
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
        ToggleHitbox(0);
        FadeColour(Color.red);
    }

    private void OnInput2(InputAction.CallbackContext ctx)
    {
        ToggleHitbox(1);
        FadeColour(Color.blue);
    }

    private void OnInput3(InputAction.CallbackContext ctx)
    {
        ToggleHitbox(2);
        FadeColour(Color.yellow);
    }

    private void OnInput4(InputAction.CallbackContext ctx)
    {
        ToggleHitbox(3);
        FadeColour(Color.green);
    }

    public void OnNoteCollision(Note note, Collider2D collidedWith)
    {
        var correctCollider = GetCollider(note.GetDirection());
        correctCollider.hasCollision = true;
        if (correctCollider.enabled && correctCollider.collider == collidedWith) // the direction of the note matches the collider AND the collider is pressed.
        {
            correctCollider.enabled = false;
            NoteCollideEvent?.Invoke(note);
            note.Despawn();
        }
    }

    public void OnNoteExit(Note note)
    {
        var collider = GetCollider(note.GetDirection());
        collider.hasCollision = false;

        NotePassEvent?.Invoke(note, HasAnyActiveCollider());
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
            NoCollisionEvent?.Invoke();
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

    public InputCollider GetCollider(NoteDirection direction)
    {
        return colliders.FirstOrDefault((col) => col.position == direction);
    }

    public bool HasAnyActiveCollider()
    {
        return colliders.Any(c => c.enabled);
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
    public NoteDirection position;
    [SerializeField]
    internal bool hasCollision;
}

