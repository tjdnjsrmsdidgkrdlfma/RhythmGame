using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public enum Accuracy
    {
        Perfect, Great, Good, Ok, Miss
    }

    enum Area
    {
        D, F, J, K
    }

    #region 노트 검사
    [Serializable]
    public class check_collider_1d
    {
        public BoxCollider[] check_collider = new BoxCollider[4];
    }

    [Header("노트 검사")]
    public check_collider_1d[] check_collider_2d = new check_collider_1d[4];

    LayerMask mask;
    #endregion

    #region 노트 생성
    [Header("노트 생성")]
    public GameObject note_prefab;

    readonly int[] note_x_position = new int[4] { -3, -1, 1, 3 };
    List<Dictionary<string, object>> note_data;
    #endregion

    #region 노트 콤보
    [Header("노트 콤보")]
    public TextMeshProUGUI combo_text;

    int combo;
    public int Combo
    {
        get { return combo; } 
        set
        {
            combo = value;
            combo_text.text = combo.ToString();
        }
    }
    #endregion

    #region 노트 판정
    [Header("노트 판정")]
    public TextMeshProUGUI note_accuracy_text;
    public float note_accuracy_remove_time;

    Coroutine set_accuracy;
    #endregion

    #region 노트 효과
    public ClickEffect[] click_effect = new ClickEffect[4];
    #endregion

    #region 점수
    public TextMeshProUGUI grade;
    public Image score_bar;
    public TextMeshProUGUI score_text;

    readonly int basic_score_per_note = 10;
    readonly int score_multiplier_by_combo = 100; //콤보가 0 ~ 99일 때는 점수가 1배 100 ~ 199일 때는 2배
    readonly int[] score_by_accuracy = new int[4] { 5, 3, 2, 1 };
    int possible_max_score;
    int score;
    #endregion

    void Awake()
    {
        mask = LayerMask.GetMask("Note");
        note_data = CSVReader.Read("NoteTest");
        note_data.Sort(new SortComparer());
        combo = 0;
        ScoreInit();
    }

    void ScoreInit()
    {
        grade.text = "D";
        score_bar.fillAmount = 0;
        score_text.text = "000000";

        int note_count = note_data.Count;
        possible_max_score = 0;

        while(note_count > score_multiplier_by_combo)
        {
            possible_max_score += basic_score_per_note * score_by_accuracy[(int)Accuracy.Perfect] * (note_count / score_multiplier_by_combo + 1);
            note_count -= score_multiplier_by_combo;
        }
        possible_max_score += basic_score_per_note * score_by_accuracy[(int)Accuracy.Perfect] * note_count;

        score = 0;
    }

    void Start()
    {
        StartCoroutine(SpawnNote());
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

        for (int i = 0; i < 4; i++)
        {
            other = Physics.OverlapBox(check_collider_2d[area].check_collider[i].center,
                                       check_collider_2d[area].check_collider[i].size / 2,
                                       Quaternion.identity,
                                       mask);

            if (other.Length == 0)
                continue; //검색된 노트가 없으면 다음 콜라이더로 넘어간다

            Combo = Combo + 1;
            other[0].gameObject.GetComponent<Note>().check = true;
            other[0].gameObject.GetComponent<Note>().OnClicked();
            click_effect[area].ShowEffect();

            if(set_accuracy != null)
                StopCoroutine(set_accuracy);            
            set_accuracy = StartCoroutine(SetAccuracy(i));

            ScoreTemp(i);

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

        yield return new WaitForSeconds(note_accuracy_remove_time);
        note_accuracy_text.enabled = false;
    }

    IEnumerator SpawnNote()
    {
        int i = 0;
        float time = 0;
        Vector3 spawn_position = new Vector3(0, 0.05f, 55);

        while (i < note_data.Count)
        {
            time += Time.deltaTime;

            if (time > float.Parse(note_data[i]["Time"].ToString()))
            {
                spawn_position.x = note_x_position[int.Parse(note_data[i]["Area"].ToString())];

                Instantiate(note_prefab, spawn_position, Quaternion.identity);
                i++;
            }

            yield return null;
        }
    }

    void ScoreTemp(int accuracy)
    {
        score += basic_score_per_note * score_by_accuracy[accuracy] * (Combo / score_multiplier_by_combo + 1);
        score_text.text = score.ToString();

        float temp = (float)score / (float)possible_max_score;
        score_bar.fillAmount = temp;
        Debug.Log(temp);

        if (temp >= 90)
            grade.text = "S";
        else if (temp >= 80)
            grade.text = "A";
        else if (temp >= 60)
            grade.text = "B";
        else if (temp >= 40)
            grade.text = "C";
        else
            grade.text = "D";
    }
}

public class SortComparer : IComparer<Dictionary<string, object>>
{
    public int Compare(Dictionary<string, object> a, Dictionary<string, object> b)
    {
        return float.Parse(a["Time"].ToString()) < float.Parse(b["Time"].ToString()) ? -1 : 1;
    }
}