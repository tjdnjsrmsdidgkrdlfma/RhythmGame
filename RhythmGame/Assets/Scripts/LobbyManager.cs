using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    #region 음악 정보
    [Header("음악 정보")]
    [SerializeField] GameObject music_infomation_prefab;
    [SerializeField] GameObject music_infomations;

    List<Dictionary<string, object>> music_data;
    #endregion

    #region 음악 속도
    [Header("음악 속도")]
    [SerializeField] GameObject note_speed_setter;
    [SerializeField] TextMeshProUGUI note_speed_text;
    [SerializeField] GameObject decrease_button;

    float note_speed;
    float NoteSpeed
    {
        get { return note_speed; }
        set
        {
            note_speed = value;

            if (note_speed < min_note_speed)
                note_speed = min_note_speed;
            if (note_speed > max_note_speed)
                note_speed = max_note_speed;

            SetNoteSpeedText(note_speed);
        }
    }

    readonly float min_note_speed = 0.5f;
    readonly float max_note_speed = 6;
    #endregion

    #region 음악 이동
    [Header("음악 이동")]
    [SerializeField] float music_swipe_speed;

    bool is_moving;
    bool is_note_speed_setter_on;
    int current_music_index;
    #endregion

    [Space(10)]
    [SerializeField] EventSystem event_system;

    void Awake()
    {
        MusicDataInit();

        NoteSpeed = 1;

        is_moving = false;
        is_note_speed_setter_on = false;
        current_music_index = 0;
    }

    void MusicDataInit()
    {
        //파일 경로 지정
        StringBuilder music_data_path = new StringBuilder();
        music_data_path.Append(Application.persistentDataPath);
        music_data_path.Append("/MusicList.csv");

        //파일 읽기
        music_data = CSVReader.Read(music_data_path.ToString());

        string music_name_gameobject = "MusicName";
        string music_thumbnail_gameobject = "MusicThumbnail";

        for (int i = 0; i < music_data.Count; i++)
        {
            //음악 썸네일 경로 지정
            StringBuilder path = new StringBuilder();
            path.Append(Application.persistentDataPath);
            path.Append('/');
            path.Append(music_data[i]["Name"]);
            path.Append(".png");
            
            //음악 썸네일 파일 읽기
            byte[] image_binary = System.IO.File.ReadAllBytes(path.ToString());
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(image_binary);

            //음악 이름 지정
            StringBuilder music_name = new StringBuilder();
            music_name.Append(music_data[i]["Name"]);

            //음악 정보 설정
            GameObject music_infomation = Instantiate(music_infomation_prefab, music_infomations.transform);
            music_infomation.transform.Find(music_name_gameobject).GetComponent<TextMeshProUGUI>().text = music_name.ToString();
            music_infomation.transform.Find(music_thumbnail_gameobject).GetComponent<RawImage>().texture = texture;
            RectTransform rt = music_infomation.GetComponent<RectTransform>();
            rt.offsetMin = new Vector2(i * 5120, rt.offsetMin.y);
        }
    }

    void Start()
    {
        SoundManager.sound_manager.PlayBGM("sans");
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

        if (horizontal == -1 && current_music_index != 0) //왼쪽 입력이 들어왔을 때 왼쪽 끝이 아니라면
            StartCoroutine(RealMoveMusic(0)); //왼쪽으로 이동
        else if (horizontal == 1 && current_music_index != music_data.Count - 1) //오른쪽 입력이 들어왔을 때 오른쪽 끝이 아니라면
            StartCoroutine(RealMoveMusic(1)); //오른쪽으로 이동
    }

    IEnumerator RealMoveMusic(int direction)
    {
        is_moving = true;

        SoundManager.sound_manager.PlaySFX("PageSlide");

        RectTransform rt = music_infomations.GetComponent<RectTransform>();
        Vector2 destination;

        if (direction == 0)
        {
            current_music_index--;
            destination = new Vector2(rt.offsetMin.x + 5120, rt.offsetMin.y);

            while(rt.offsetMin.x <= destination.x)
            {
                rt.offsetMin = new Vector2(rt.offsetMin.x + music_swipe_speed, rt.offsetMin.y);
                yield return null;
            }
        }
        else
        {
            current_music_index++;
            destination = new Vector2(rt.offsetMin.x - 5120, rt.offsetMin.y);

            while (rt.offsetMin.x >= destination.x)
            {
                rt.offsetMin = new Vector2(rt.offsetMin.x - music_swipe_speed, rt.offsetMin.y);
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
                string music_name = music_data[current_music_index]["Name"].ToString();

                GameManager.game_manager.note_speed = NoteSpeed;

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
                NoteSpeed = 1;
            }
        }
    }

    public void OnDecreaseButtonClicked()
    {
        NoteSpeed -= 0.5f;

        SoundManager.sound_manager.PlaySFX("ButtonClick");
    }

    public void OnIncreaseButtonClicked()
    {
        NoteSpeed += 0.5f;

        SoundManager.sound_manager.PlaySFX("ButtonClick");
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
