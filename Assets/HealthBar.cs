using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;


public class HealthBar : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;
    private Coroutine coroutine;

    public float fadeTime = 1.5f;
    private float activeTime = 1f;

    public Color HPBarColour;

    private const string path = "Sprites/healthBar_32x16"; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        print("start called. Spriterenderer = " + spriteRenderer);
        sprites = new Sprite[5];

        sprites[0] = LoadSubsprite(path, "healthBar_32x16_0");
        sprites[1] = LoadSubsprite(path, "healthBar_32x16_1");
        sprites[2] = LoadSubsprite(path, "healthBar_32x16_2");
        sprites[3] = LoadSubsprite(path, "healthBar_32x16_3");
        sprites[4] = LoadSubsprite(path, "healthBar_32x16_4");

        spriteRenderer.sprite = sprites[0];

    }

    private Sprite LoadSubsprite(string path, string subspriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>(path);
        foreach (var s in all)
        {
            if (s.name == subspriteName)
            {
                return s;
            }
        }
        return null;

    }

    public void UpdateHealthbar(int maxHP, int currentHP)
    {
        float[] thresholds = { 1f, 0.75f, 0.5f, 0.25f, 0f };
        

        FadeHealthbar();

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (currentHP == maxHP * thresholds[i])
            {
                spriteRenderer.sprite = sprites[i];
                break;
            }
        }
        return;
    }

    private void UpdateHealthbar()
    {
        FadeHealthbar();
    }

    public void SetColour(Color color)
    {
        spriteRenderer.color = color;
        UpdateHealthbar();
    }

    private void FadeHealthbar()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(c_FadeOpacity(fadeTime));
    }

    private IEnumerator c_FadeOpacity(float fadeTime)
    {
        float timer = 0f;
        Color baseColour = spriteRenderer.color;
        baseColour.a = 1;

        Color targetColour = spriteRenderer.color;
        targetColour.a = 0;

        spriteRenderer.color = baseColour;

        yield return new WaitForSeconds(activeTime);

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float time = timer / fadeTime;
            spriteRenderer.color = Color.Lerp(baseColour, targetColour, time);

            yield return null;
        }
    }
}
