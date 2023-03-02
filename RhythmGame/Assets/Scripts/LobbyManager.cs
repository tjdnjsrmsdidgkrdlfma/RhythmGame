using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public float music_move_speed;
    public GameObject music_infomations;
    public GameObject music_infomation_prefab;
    public GameObject note_speed_setter;
    public TextMeshProUGUI note_speed_text;
    public GameObject decrease_button;
    public EventSystem event_system;

    bool is_moving;
    bool is_note_speed_setter_on;
    int current_music;
    float note_speed;

    readonly float min_note_speed = 0.5f;
    readonly float max_note_speed = 6;

    List<Dictionary<string, object>> music_data;

    void Awake()
    {
        MusicDataInit();

        is_moving = false;
        is_note_speed_setter_on = false;
        current_music = 0;
        note_speed = 1;
    }

    void MusicDataInit()
    {
        StringBuilder music_data_path = new StringBuilder();
        music_data_path.Append(Application.persistentDataPath);
        music_data_path.Append("/MusicList.csv");

        music_data = CSVReader.Read(music_data_path.ToString());
        string music_name_gameobject = "MusicName";
        string music_thumbnail_gameobject = "MusicThumbnail";

        for (int i = 0; i < music_data.Count; i++)
        {
            StringBuilder path = new StringBuilder();
            path.Append(Application.persistentDataPath);
            path.Append('/');
            path.Append(music_data[i]["Name"]);
            path.Append(".png");

            byte[] image_binary = System.IO.File.ReadAllBytes(path.ToString());
            Texture2D texture = new Texture2D(1, 1);

            texture.LoadImage(image_binary);

            StringBuilder music_name = new StringBuilder();
            music_name.Append(music_data[i]["Name"]);

            GameObject music_infomation = Instantiate(music_infomation_prefab, music_infomations.transform);
            music_infomation.transform.Find(music_name_gameobject).GetComponent<TextMeshProUGUI>().text = music_name.ToString();
            music_infomation.transform.Find(music_thumbnail_gameobject).GetComponent<RawImage>().texture = texture;
            RectTransform rt = music_infomation.GetComponent<RectTransform>();
            rt.offsetMin = new Vector2(i * 5120, rt.offsetMin.y);
        }
    }

    void Update()
    {
        MoveMusic();
        ChooseStage();
    }

    void MoveMusic()
    {
        if (is_moving == true || is_note_speed_setter_on == true) //이미 움직이고 있거나 이미 선택했으면
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal == 0) //입력이 없다면
            return;

        if (horizontal == -1 && current_music != 0) //왼쪽 입력이 들어왔을 때 왼쪽 끝이 아니라면
            StartCoroutine(RealMoveMusic(true));
        else if (horizontal == 1 && current_music != music_data.Count - 1) //오른쪽 입력이 들어왔을 때 오른쪽 끝이 아니라면
            StartCoroutine(RealMoveMusic(false));
    }

    IEnumerator RealMoveMusic(bool left)
    {
        is_moving = true;

        RectTransform rt = music_infomations.GetComponent<RectTransform>();
        Vector2 destination;

        if (left == true)
        {
            current_music--;
            destination = new Vector2(rt.offsetMin.x + 5120, rt.offsetMin.y);

            while(rt.offsetMin.x <= destination.x)
            {
                rt.offsetMin = new Vector2(rt.offsetMin.x + music_move_speed, rt.offsetMin.y);
                yield return null;
            }
        }
        else
        {
            current_music++;
            destination = new Vector2(rt.offsetMin.x - 5120, rt.offsetMin.y);

            while (rt.offsetMin.x >= destination.x)
            {
                rt.offsetMin = new Vector2(rt.offsetMin.x - music_move_speed, rt.offsetMin.y);
                yield return null;
            }
        }

        rt.offsetMin = destination;
        is_moving = false;
    }

    void ChooseStage()
    {
        if (is_moving == true)
            return;

        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            if (is_note_speed_setter_on == true)
            {
                string music_name = music_data[current_music]["Name"].ToString();

                GameManager.game_manager.LoadIngameScene(music_name);
            }
            else
            {
                is_note_speed_setter_on = !is_note_speed_setter_on;
                note_speed_setter.SetActive(is_note_speed_setter_on);
                event_system.SetSelectedGameObject(decrease_button);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if (is_note_speed_setter_on == true)
            {
                is_note_speed_setter_on = !is_note_speed_setter_on;
                note_speed_setter.SetActive(is_note_speed_setter_on);
            }
        }
    }

    public void OnDecreaseButtonClicked()
    {
        note_speed = Mathf.Clamp(note_speed - 0.5f, min_note_speed, max_note_speed);
        SetNoteSpeedText(note_speed);
    }

    public void OnIncreaseButtonClicked()
    {
        note_speed = Mathf.Clamp(note_speed + 0.5f, min_note_speed, max_note_speed);
        SetNoteSpeedText(note_speed);
    }

    void SetNoteSpeedText(float note_speed)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Note Speed\n");
        sb.Append("x");
        sb.Append(string.Format("{0:0.0}", note_speed));
        note_speed_text.text = sb.ToString();
    }
}
