using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    #region 이펙트 값
    [Header("이펙트 값")]
    [SerializeField] float area_effect_transparent_value;
    [SerializeField] float click_effect_transparent_value;
    #endregion

    #region 이펙트 머티리얼
    [Header("이펙트 머티리얼")]
    [SerializeField] MeshRenderer area_effect_material;
    [SerializeField] MeshRenderer click_effect_material;
    #endregion

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
            color.r = color.r - Time.deltaTime * area_effect_transparent_value;
            color.g = color.g - Time.deltaTime * area_effect_transparent_value;
            color.b = color.b - Time.deltaTime * area_effect_transparent_value;
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
            color.a = color.a - Time.deltaTime * click_effect_transparent_value;
            click_effect_material.material.color = color;
            yield return null;
        }

        click_effect_material.material.color = new Color(1, 1, 1, 0);
    }
}