using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerLine : MonoBehaviour
{

    private BoxCollider2D Hitbox;
    public float marginOfError;
    private Vector2 initialPosition;

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

        Hitbox.enabled = false;
        initialPosition = Hitbox.offset;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Debug.Log("Up");          

            
            initialPosition.x = -1.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
            FadeColour(Color.red);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Debug.Log("Down");


            initialPosition.x = -0.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
            FadeColour(Color.blue);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Debug.Log("Left");


            initialPosition.x = 0.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
            FadeColour(Color.yellow);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //Debug.Log("Right");


            initialPosition.x = 1.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
            FadeColour(Color.green);
        }


    }

    

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something got HIT");

        
        audioSource.Play();
        if (collision.tag == "minigame_Note")
        {
            Destroy(collision.gameObject);
            Score += ScoreIncrease;
            print("NOTE");
        }
        
        //kill note
        
        //increment score
        

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
        //subtract score for toggling hitbox.
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

