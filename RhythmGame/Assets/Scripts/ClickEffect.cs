using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public float area_effect_transparent_time;
    public float click_effect_transparent_time; //4
    public MeshRenderer area_effect_material;
    public MeshRenderer click_effect_material;

    Coroutine click_effect;
    Coroutine area_effect;

    public void OnClickedEffect(bool is_miss)
    {
        if (area_effect != null)
            StopCoroutine(area_effect);
        area_effect = StartCoroutine(ShowAreaEffect());

        if(is_miss == false)
        {
            if (click_effect != null)
                StopCoroutine(click_effect);
            click_effect = StartCoroutine(ShowClickEffect());
        }
    }

    IEnumerator ShowAreaEffect()
    {
        Color color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        area_effect_material.material.color = color;

        while (area_effect_material.material.color.r > 0)
        {
            color.r = color.r - Time.deltaTime * area_effect_transparent_time;
            color.g = color.g - Time.deltaTime * area_effect_transparent_time;
            color.b = color.b - Time.deltaTime * area_effect_transparent_time;
            area_effect_material.material.color = color;
            yield return null;
        }

        area_effect_material.material.color = new Color(0, 0, 0, 0.5f);

        yield break;
    }

    IEnumerator ShowClickEffect()
    {
        Color color = new Color(1, 1, 1, 1);

        while (click_effect_material.material.color.a >= 0)
        {
            color.a = color.a - Time.deltaTime * click_effect_transparent_time;
            click_effect_material.material.color = color;
            yield return null;
        }

        click_effect_material.material.color = new Color(1, 1, 1, 0);
    }
}