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

    [SerializeField] bool is_bgm_on;
    public bool IsBGMOn
    {
        get { return is_bgm_on; }
        set
        {
            is_bgm_on = value;
            SetBGMOnOff(is_bgm_on);
        }
    }
    void SetBGMOnOff(bool is_bgm_on)
    {
        if(is_bgm_on == true)
            bgm_player.volume = 1;
        else
            bgm_player.volume = 0;
    }

    [SerializeField] bool is_sfx_on;
    public bool IsSFXOn
    {
        get { return is_sfx_on; }
        set
        {
            is_sfx_on = value;
            SetSFXOnOff(is_sfx_on);
        }
    }
    void SetSFXOnOff(bool is_sfx_on)
    {
        if (is_sfx_on == true)
            sfx_player.volume = 0.1f;
        else
            sfx_player.volume = 0;
    }

    public Sound[] bgm;
    public Sound[] sfx;

    public AudioSource bgm_player;
    public AudioSource sfx_player;

    void Awake()
    {
        if (sound_manager == null)
        {
            sound_manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        IsBGMOn = true;
        IsSFXOn = true;
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
