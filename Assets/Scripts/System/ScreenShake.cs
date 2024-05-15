using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float duration = 1.0f;
    public AnimationCurve animationCurve;
    [InspectorButton("Shake")]
    public bool shakeScreen;

    public void Shake()
    {
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime+= Time.deltaTime;
            float strength = animationCurve.Evaluate(elapsedTime / duration);
            var newPos = transform.position + Random.insideUnitSphere * strength;
            newPos.z = startPosition.z;
            transform.position = newPos;
            yield return null;
        }

        //transform.position = startPosition;
    }
}
