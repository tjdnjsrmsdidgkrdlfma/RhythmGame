using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager sound_manager;

    public Sound[] bgm;
    public Sound[] sfx;

    public AudioSource bgm_player;
    public AudioSource sfx_player;

    void Awake()
    {
        if (sound_manager == null)
        {
            sound_manager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayBGM(string bgm_name)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgm_name.Equals(bgm[i].name))
            {
                bgm_player.clip = bgm[i].clip;
                bgm_player.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgm_player.Stop();
    }

    public void PlaySFX(string sfx_name)
    {
        for(int i = 0; i < sfx.Length; i++)
        {
            if (sfx_name.Equals(sfx[i].name))
            {
                sfx_player.PlayOneShot(sfx[i].clip);
            }
        }
    }
}
