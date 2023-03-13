using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] GameObject menu_button;
    [SerializeField] GameObject setting_button;
    [SerializeField] GameObject menu_buttons;
    [SerializeField] GameObject settings_buttons;
    [SerializeField] GameObject default_menu_buttons;
    [SerializeField] GameObject default_settings_buttons;
    [SerializeField] GameObject bgm_disable_line;
    [SerializeField] GameObject sfx_disable_line;
    [SerializeField] EventSystem event_system;

    bool is_bgm_disable_line_on;
    bool is_sfx_disable_line_on;

    void Awake()
    {
        is_bgm_disable_line_on = false;
        is_sfx_disable_line_on = false;
    }

    public void OnStartButtonClicked()
    {
        LoadingSceneManager.LoadScene("Lobby");
    }

    public void OnSettingsButtonClicked()
    {
        menu_buttons.SetActive(false);
        settings_buttons.SetActive(true);

        event_system.SetSelectedGameObject(default_settings_buttons);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnBGMButtonClicked()
    {
        GameManager.game_manager.is_bgm_on = !GameManager.game_manager.is_bgm_on;
        is_bgm_disable_line_on = !is_bgm_disable_line_on;
        bgm_disable_line.SetActive(is_bgm_disable_line_on);
    }

    public void OnSFXButtonClicked()
    {
        GameManager.game_manager.is_sfx_on = !GameManager.game_manager.is_sfx_on;
        is_sfx_disable_line_on = !is_sfx_disable_line_on;
        sfx_disable_line.SetActive(is_sfx_disable_line_on);
    }

    public void OnMenuButtonClicked()
    {
        menu_buttons.SetActive(true);
        settings_buttons.SetActive(false);

        event_system.SetSelectedGameObject(default_menu_buttons);
    }
}