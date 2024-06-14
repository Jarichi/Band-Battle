using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MicWeapon : Weapon
{
    protected override WeaponPositioning UpPosition()
    {
        return new WeaponPositioning(new Vector2(0, .5f), new Vector3(0, 0, 90));
    }
    protected override WeaponPositioning RightPosition()
    {
        return new WeaponPositioning(new Vector2(1, 0), Vector3.zero);
    }
    protected override WeaponPositioning DownPosition()
    {
        return new WeaponPositioning(new Vector2(0, -1.2f), new Vector3(0, 0, -90));
    }
    protected override WeaponPositioning LeftPosition()
    {
        return new WeaponPositioning(new Vector2(-1, 0), new Vector3(0, 0, -180));
    }
}

