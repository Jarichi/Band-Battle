using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuitarWeapon : Weapon
{
    protected override WeaponPositioning DownPosition()
    {
        return WeaponPositioning.zero;
    }

    protected override WeaponPositioning LeftPosition()
    {
        return new WeaponPositioning(new Vector2(-0.30f, transform.localPosition.y), new Vector3(0, 180, 0));
    }

    protected override WeaponPositioning RightPosition()
    {
        return new WeaponPositioning(new Vector2(0, transform.localPosition.y), Vector3.zero);
    }

    protected override WeaponPositioning UpPosition()
    {
        return WeaponPositioning.zero;
    }
}
