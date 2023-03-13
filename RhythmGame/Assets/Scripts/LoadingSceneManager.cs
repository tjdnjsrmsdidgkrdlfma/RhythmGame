using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
    [SerializeField] RectTransform press_space_to_start;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        Coroutine move_press_space_to_start;

        move_press_space_to_start = StartCoroutine(MoveTextUpAndDown());

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

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                break;
            yield return null;
        }

        StopCoroutine(move_press_space_to_start);

        op.allowSceneActivation = true;
    }

    IEnumerator MoveTextUpAndDown()
    {
        bool up = true;

        float temp;

        float from;
        float to;

        while (true)
        {
            if (up == true)
            {
                from = 0;
                to = 150;
            }
            else
            {
                from = 150;
                to = 0;
            }

            for (float i = 0; i < 30; i++)
            {
                temp = Mathf.Lerp(from, to, i / 30);
                press_space_to_start.offsetMin = new Vector2(press_space_to_start.offsetMin.x, temp);
                yield return null;
                yield return null;
                yield return null;
                yield return null;
                yield return null;
            }

            temp = to;
            press_space_to_start.offsetMin = new Vector2(press_space_to_start.offsetMin.x, temp);

            up = !up;

            //yield return new WaitForSeconds(0.05f);
        }
    }
}