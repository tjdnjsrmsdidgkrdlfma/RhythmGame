using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public float transparent_time;

    Material material;
    Coroutine change_color;

    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public void ShowEffect()
    {
        if(change_color != null)
            StopCoroutine(change_color);
        change_color = StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        Color color = new Color(1, 1, 1, 1);

        while(material.color.a >= 0)
        {
            color.a = color.a - Time.deltaTime * transparent_time;
            material.color = color;
            yield return null;
        }

        material.color = new Color(1, 1, 1, 0);
    }
}