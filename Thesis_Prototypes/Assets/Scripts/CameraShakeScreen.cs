using System.Collections;
using UnityEngine;

public class CameraShakeScreen : MonoBehaviour
{
    public float duration = 0.5f;
    public AnimationCurve curve;

    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPos;
    }

    public void ShakeScreen()
    {
        StartCoroutine(Shaking());
    }
}
