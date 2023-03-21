using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    #region 노트 검사
    [Serializable]
    class check_collider_1d
    {
        public BoxCollider[] check_collider = new BoxCollider[4];
    }

    enum Area
    {
        D, F, J, K
    }

    [Header("노트 검사")]
    [SerializeField] check_collider_1d[] check_collider_2d = new check_collider_1d[4];

    LayerMask mask;
    #endregion

    #region 노트 생성
    [Header("노트 생성")]
    public List<GameObject> note_pool;

    [SerializeField] GameObject note_prefab;
    [SerializeField] GameObject note_pool_object;

    readonly int[] note_x_position = new int[4] { -3, -1, 1, 3 };

    List<Dictionary<string, object>> note_data;
    #endregion

    #region 노트 콤보
    [Header("노트 콤보")]
    [SerializeField] TextMeshProUGUI combo_text;

    int combo;
    public int Combo
    {
        get { return combo; }
        set
        {
            combo = value;
            combo_text.text = combo.ToString();

            if (combo > max_combo)
                max_combo = combo;
        }
    }
    #endregion

    #region 노트 판정
    public enum Accuracy
    {
        Perfect, Great, Good, Ok, Miss
    }

    [Header("노트 판정")]
    [SerializeField] TextMeshProUGUI note_accuracy_text;
    [SerializeField] float note_accuracy_remove_time;

    Coroutine set_accuracy;
    #endregion

    #region 노트 효과
    [Header("노트 효과")]
    [SerializeField] ClickEffect[] click_effect = new ClickEffect[4];
    #endregion

    #region 점수
    enum Grade
    {
        S, A, B, C, D
    }

    [SerializeField] TextMeshProUGUI grade_text;
    [SerializeField] Image score_bar;
    [SerializeField] TextMeshProUGUI score_text;

    readonly int basic_score_per_note = 10;
    readonly int score_multiplier_by_combo = 100; //콤보가 0 ~ 99일 때는 점수가 1배 100 ~ 199일 때는 2배
    readonly int[] score_by_accuracy = new int[4] { 5, 3, 2, 1 };
    int possible_max_score;
    int score;

    Grade grade = Grade.D;
    #endregion

    #region 결과창
    [Header("노트 검사")]
    [SerializeField] float time_before_after_game;

    [SerializeField] GameObject result_screen;
    [SerializeField] GameObject lobby_button;
    [SerializeField] TextMeshProUGUI max_combo_text;
    [SerializeField] TextMeshProUGUI final_score_text;
    [SerializeField] TextMeshProUGUI final_grade_text;
    [SerializeField] TextMeshProUGUI hit_time_by_accuracy;

    bool is_start;
    bool is_end;
    int max_combo = 0;
    int[] hit_times_by_accuracy = new int[5];
    #endregion

    [Space(10)]
    [SerializeField] EventSystem event_system;

    void Awake()
    {
        mask = LayerMask.GetMask("Note");

        StringBuilder csv_path = new StringBuilder();
        csv_path.Append(Application.persistentDataPath);
        csv_path.Append("/");
        csv_path.Append(GameManager.game_manager.music_name);
        csv_path.Append(".csv");

        note_data = CSVReader.Read(csv_path.ToString());
        note_data.Sort(new InGameDataComparer());
        combo = 0;

        is_start = false;
        is_end = false;

        ScoreInit();
    }

    void ScoreInit()
    {
        grade_text.text = "D";
        score_bar.fillAmount = 0;
        score_text.text = "000000";

        int note_count = note_data.Count;
        possible_max_score = 0;

        while (note_count > score_multiplier_by_combo) //가능한 최고 점수를 구하는 코드
        {
            possible_max_score += basic_score_per_note * score_by_accuracy[(int)Accuracy.Perfect] * (note_count / score_multiplier_by_combo + 1);
            note_count -= score_multiplier_by_combo;
        }
        possible_max_score += basic_score_per_note * score_by_accuracy[(int)Accuracy.Perfect] * note_count;

        score = 0;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(time_before_after_game);

        is_start = true;

        StartCoroutine(SpawnNote());

        SoundManager.sound_manager.PlayBGM("Asgore");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            CheckNote((int)Area.D);
        if (Input.GetKeyDown(KeyCode.F))
            CheckNote((int)Area.F);
        if (Input.GetKeyDown(KeyCode.J))
            CheckNote((int)Area.J);
        if (Input.GetKeyDown(KeyCode.K))
            CheckNote((int)Area.K);
    }

    void CheckNote(int area)
    {
        Collider[] other;

        click_effect[area].OnClickedEffect(true);

        SoundManager.sound_manager.PlaySFX("AreaClick");

        if (is_start == false || is_end == true)
            return;

        for (int i = 0; i < 4; i++)
        {
            other = Physics.OverlapBox(check_collider_2d[area].check_collider[i].center,
                                       check_collider_2d[area].check_collider[i].size / 2,
                                       Quaternion.identity,
                                       mask);

            if (other.Length == 0)
                continue; //검색된 노트가 없으면 다음 콜라이더로 넘어간다

            Combo = Combo + 1;
            foreach (Collider temp in other)
            {
                temp.GetComponent<Note>().check = true;
                temp.GetComponent<Note>().OnClicked();
            }
            click_effect[area].OnClickedEffect(false);

            if (set_accuracy != null)
                StopCoroutine(set_accuracy);
            set_accuracy = StartCoroutine(SetAccuracy(i));

            ChangeScore(i);

            return;
        }

        Combo = 0; //4개의 콜라이더를 모두 검사해서 검색된 노트가 없는 경우
        if (set_accuracy != null)
            StopCoroutine(set_accuracy);
        set_accuracy = StartCoroutine(SetAccuracy((int)Accuracy.Miss));
    }

    public IEnumerator SetAccuracy(int num)
    {
        note_accuracy_text.enabled = true;

        note_accuracy_text.text = Enum.GetName(typeof(Accuracy), num);

        hit_times_by_accuracy[num]++;

        yield return new WaitForSeconds(note_accuracy_remove_time);
        note_accuracy_text.enabled = false;
    }

    IEnumerator SpawnNote()
    {
        int i = 0;
        float time = 0;
        Vector3 spawn_position = new Vector3(0, 0.05f, 55);
        GameObject temp;

        while (i < note_data.Count)
        {
            time += Time.deltaTime;

            if (time > float.Parse(note_data[i]["Time"].ToString()))
            {
                spawn_position.x = note_x_position[int.Parse(note_data[i]["Area"].ToString())];

                temp = Instantiate(note_prefab, spawn_position, Quaternion.identity);
                temp.transform.parent = note_pool_object.transform;
                i++;
            }

            yield return null;
        }

        StartCoroutine(ShowResult());
    }

    IEnumerator ShowResult()
    {
        while (note_pool_object.transform.childCount != 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(time_before_after_game);

        is_end = true;

        StringBuilder sb = new StringBuilder();

        sb.Append("Max Combo: ");
        sb.Append(max_combo);
        max_combo_text.text = sb.ToString();
        sb.Clear();

        sb.Append("Score: ");
        sb.Append(score);
        final_score_text.text = sb.ToString();
        sb.Clear();

        sb.Append("Grade: ");
        switch((int)grade)
        {
            case 0:
                sb.Append("S");
                break;
            case 1:
                sb.Append("A");
                break;
            case 2:
                sb.Append("B");
                break;
            case 3:
                sb.Append("C");
                break;
            case 4:
                sb.Append("D");
                break;
        }
        final_grade_text.text = sb.ToString();
        sb.Clear();

        sb.Append("Perfect: ");
        sb.Append(hit_times_by_accuracy[(int)Accuracy.Perfect]);
        sb.Append("\n");

        sb.Append("Great: ");
        sb.Append(hit_times_by_accuracy[(int)Accuracy.Great]);
        sb.Append("\n");

        sb.Append("Good: ");
        sb.Append(hit_times_by_accuracy[(int)Accuracy.Good]);
        sb.Append("\n");

        sb.Append("Ok: ");
        sb.Append(hit_times_by_accuracy[(int)Accuracy.Ok]);
        sb.Append("\n");

        sb.Append("Miss: ");
        sb.Append(hit_times_by_accuracy[(int)Accuracy.Miss]);
        sb.Append("\n");
        hit_time_by_accuracy.text = sb.ToString();
        sb.Clear();

        event_system.SetSelectedGameObject(lobby_button);

        result_screen.SetActive(true);
    }

    void ChangeScore(int accuracy)
    {
        score += basic_score_per_note * score_by_accuracy[accuracy] * (Combo / score_multiplier_by_combo + 1);
        score_text.text = score.ToString();

        float temp = (float)score / (float)possible_max_score;
        score_bar.fillAmount = temp;

        if (temp >= 0.9)
        {
            grade_text.text = "S";
            grade = Grade.S;
        }
        else if (temp >= 0.8)
        {
            grade_text.text = "A";
            grade = Grade.A;
        }
        else if (temp >= 0.6)
        {
            grade_text.text = "B";
            grade = Grade.B;
        }
        else if (temp >= 0.4)
        {
            grade_text.text = "C";
            grade = Grade.C;
        }
        else
        {
            grade_text.text = "D";
            grade = Grade.D;
        }
    }

    public void RestartButton()
    {
        SoundManager.sound_manager.StopBGM();
        SoundManager.sound_manager.PlaySFX("ButtonClick");

        GameManager.game_manager.LoadIngameScene(GameManager.game_manager.music_name);
    }

    public void LobbyButton()
    {
        SoundManager.sound_manager.StopBGM();
        SoundManager.sound_manager.PlaySFX("ButtonClick");

        LoadingSceneManager.LoadScene("Lobby");
    }
}