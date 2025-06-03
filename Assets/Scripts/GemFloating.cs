using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemFloating : MonoBehaviour
{
    public float floatDistance = 0.5f;
    public float floatDuration = 1f;

    void Start()
    {
        StartCoroutine("FloatPhaseUp");
    }

    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    IEnumerator FloatPhaseUp()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * floatDistance;
        float elapsed = 0f;

        while (elapsed < floatDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / floatDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        StartCoroutine(FloatPhaseDown());
    }

    IEnumerator FloatPhaseDown()
    {
        Vector3 start = transform.position;
        Vector3 end = start - Vector3.up * floatDistance;
        float elapsed = 0f;

        while (elapsed < floatDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / floatDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        StartCoroutine(FloatPhaseUp());
    }
}
