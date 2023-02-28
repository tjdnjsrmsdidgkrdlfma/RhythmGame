using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager game_manager;

    public bool is_bgm_on;
    public bool is_sfx_on;
    public float note_speed;

    void Awake()
    {
        if (game_manager == null)
        {
            game_manager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        is_bgm_on = true;
        is_sfx_on = true;
        note_speed = 1;
    }
}
