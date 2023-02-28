using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject menu_buttons;
    public GameObject settings_buttons;
    public GameObject default_menu_buttons;
    public GameObject default_settings_buttons;
    public GameObject bgm_disable_line;
    public GameObject sfx_disable_line;
    public EventSystem event_system;

    bool is_menu_buttons_on;
    bool is_bgm_disable_line_on;
    bool is_sfx_disable_line_on;

    void Awake()
    {
        is_menu_buttons_on = true;
        is_bgm_disable_line_on = false;
        is_sfx_disable_line_on = false;
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OnSettingsButtonClicked()
    {
        menu_buttons.SetActive(false);
        settings_buttons.SetActive(true);

        event_system.SetSelectedGameObject(default_settings_buttons);

        is_menu_buttons_on = false;
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

        is_menu_buttons_on = true;
    }
}
