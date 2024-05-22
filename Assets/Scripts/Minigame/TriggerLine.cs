using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerLine : MonoBehaviour
{

    private BoxCollider2D Hitbox;
    public float marginOfError;
    private Vector2 initialPosition;
    private PlayerInputController input;

    private SpriteRenderer SpriteRenderer;
    private Color baseColour = Color.white;
    private float fadeTime = 0.15f;

    private Coroutine coroutine;

    AudioSource audioSource;

    
    public int Score = 0;

    [SerializeField]
    private int ScoreIncrease = 5;

    private void Start()
    {
        Hitbox =  GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        input = transform.root.GetComponent<PlayerInputController>();
        input.PlayInput1 += OnInput1;
        input.PlayInput2 += OnInput2;
        input.PlayInput3 += OnInput3;
        input.PlayInput4 += OnInput4;

        Hitbox.enabled = false;
        initialPosition = Hitbox.offset;
    }

    private void OnInput1(InputAction.CallbackContext ctx)
    {
        initialPosition.x = -1.5f;
        Hitbox.offset = initialPosition;
        ToggleHitbox();
        FadeColour(Color.red);
    }

    private void OnInput2(InputAction.CallbackContext ctx)
    {
        initialPosition.x = -0.5f;
        Hitbox.offset = initialPosition;
        ToggleHitbox();
        FadeColour(Color.blue);
    }

    private void OnInput3(InputAction.CallbackContext ctx)
    {
        initialPosition.x = 0.5f;
        Hitbox.offset = initialPosition;
        ToggleHitbox();
        FadeColour(Color.yellow);
    }

    private void OnInput4(InputAction.CallbackContext ctx)
    {
        initialPosition.x = 1.5f;
        Hitbox.offset = initialPosition;
        ToggleHitbox();
        FadeColour(Color.green);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.Play();
        if (collision.tag == "minigame_Note")
        {
            Destroy(collision.gameObject);
            Score += ScoreIncrease;
        }
    }

    public void ClearScore()
    {
        Score = 0;
    }

    private void FadeColour(Color TargetColour)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(c_FadeColour(TargetColour, fadeTime));
    }

    private void ToggleHitbox()
    {
        Hitbox.enabled = true;
        Score--;
        StartCoroutine(c_DisableHitbox());
    }


    private IEnumerator c_DisableHitbox()
    {
        yield return new WaitForSeconds(marginOfError);
        Hitbox.enabled = false;
    }

    private IEnumerator c_FadeColour(Color TargetColour, float fadeTime)
    {       
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float time = timer/fadeTime;
            SpriteRenderer.color = Color.Lerp(TargetColour, baseColour, time);

            yield return null;
        }
        SpriteRenderer.color = baseColour;       
    }


}

