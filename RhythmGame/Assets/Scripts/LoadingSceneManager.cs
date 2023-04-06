using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public static string next_scene;

    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI press_space_to_start;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        SoundManager.sound_manager.StopBGM();

        next_scene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(next_scene);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        Coroutine move_press_space_to_start;

        press_space_to_start.text = "LOADING...";
        move_press_space_to_start = StartCoroutine(TextFadeInOut());

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f) { progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); if (progressBar.fillAmount >= op.progress) { timer = 0f; } }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f) { break; }
            }
        }

        press_space_to_start.text = "PRESS SPACE TO START";

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                break;
            yield return null;
        }

        StopCoroutine(move_press_space_to_start);

        op.allowSceneActivation = true;
    }

    IEnumerator TextFadeInOut()
    {
        bool fade_in = false;

        float from;
        float to;
        float repeat_time = 20;

        while (true)
        {
            if (fade_in == true)
            {
                from = 0;
                to = 1;
            }
            else
            {
                from = 1;
                to = 0;
            }

            for(int i = 0; i < repeat_time; i++)
            {
                from = Mathf.Lerp(from, to, i / repeat_time);
                press_space_to_start.color = new Color(1, 1, 1, from);

                yield return new WaitForSeconds(0.025f);
            }

            press_space_to_start.color = new Color(1, 1, 1, to);

            fade_in = !fade_in;
        }
    }
}