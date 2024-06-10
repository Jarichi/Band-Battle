using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    public Color HPBarColour;

    private const string path = "Sprites/healthBar_32x16"; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void SetColour(Color color)
    {
        spriteRenderer.color = color;
    }
}
