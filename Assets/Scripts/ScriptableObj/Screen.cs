using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenType
{
    Full,
    Transparent,
}

[CreateAssetMenu()]
public class Screen : ScriptableObject
{
    public Sprite background;
    public ScreenType screenType;
    

}
