using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroManager : MonoBehaviour
{
    #region 전환 버튼
    [Header("전환 버튼")]
    [SerializeField] GameObject menu_buttons;
    [SerializeField] GameObject settings_buttons;
    #endregion

    #region 전환 시 기본 버튼
    [Header("전환 시 기본 버튼")]
    [SerializeField] GameObject default_menu_buttons;
    [SerializeField] GameObject default_settings_buttons;
    #endregion

    #region 작대기
    [Header("작대기")]
    [SerializeField] GameObject bgm_disable_line;
    [SerializeField] GameObject sfx_disable_line;

    bool is_bgm_disable_line_on;
    bool is_sfx_disable_line_on;
    #endregion

    [Space(10)]
    [SerializeField] EventSystem event_system;
    
    void Awake()
    {
        is_bgm_disable_line_on = false;
        is_sfx_disable_line_on = false;
    }

    void Start()
    {
        SoundManager.sound_manager.PlayBGM("OnceUponATime");
    }

    public void OnStartButtonClicked()
    {
        SoundManager.sound_manager.PlaySFX("ButtonClick");

        LoadingSceneManager.LoadScene("Lobby");
    }

    public void OnSettingsButtonClicked()
    {
        menu_buttons.SetActive(false);
        settings_buttons.SetActive(true);

        event_system.SetSelectedGameObject(default_settings_buttons);

        SoundManager.sound_manager.PlaySFX("ButtonClick");
    }

    public void OnQuitButtonClicked()
    {
        SoundManager.sound_manager.PlaySFX("ButtonClick");

        Application.Quit();
    }

    public void OnBGMButtonClicked()
    {
        SoundManager.sound_manager.IsBGMOn = !SoundManager.sound_manager.IsBGMOn;

        is_bgm_disable_line_on = !is_bgm_disable_line_on;
        bgm_disable_line.SetActive(is_bgm_disable_line_on);

        SoundManager.sound_manager.PlaySFX("ButtonClick");
    }

    public void OnSFXButtonClicked()
    {
        SoundManager.sound_manager.IsSFXOn = !SoundManager.sound_manager.IsSFXOn;

        is_sfx_disable_line_on = !is_sfx_disable_line_on;
        sfx_disable_line.SetActive(is_sfx_disable_line_on);

        SoundManager.sound_manager.PlaySFX("ButtonClick");
    }

    public void OnFilePathButtonClicked()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);

        SoundManager.sound_manager.PlaySFX("ButtonClick");
    }

    public void OnBackButtonClicked()
    {
        menu_buttons.SetActive(true);
        settings_buttons.SetActive(false);

        event_system.SetSelectedGameObject(default_menu_buttons);

        SoundManager.sound_manager.PlaySFX("ButtonClick");
    }
}