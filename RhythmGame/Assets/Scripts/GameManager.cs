using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager game_manager;

    public float note_speed;
    public string music_name;

    void Awake()
    {
        if (game_manager == null)
        {
            game_manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        note_speed = 1;
    }

    public void LoadIngameScene(string stage_name)
    {
        music_name = stage_name;

        LoadingSceneManager.LoadScene("InGame");
    }
}

public class InGameDataComparer : IComparer<Dictionary<string, object>>
{
    public int Compare(Dictionary<string, object> a, Dictionary<string, object> b)
    {
        return float.Parse(a["Time"].ToString()) < float.Parse(b["Time"].ToString()) ? -1 : 1;
    }
}