using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
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
    #endregion

    #region 노트 콤보
    [Header("노트 콤보")]
    public TextMeshProUGUI combo_text;

    int combo;

    public void SetCombo(int num)
    {
        combo = num;
        combo_text.text = combo.ToString();
    }
    #endregion

    #region 노트 판정
    [Header("노트 판정")]
    public TextMeshProUGUI note_accuracy_text;
    public float note_accuracy_remove_time;

    Coroutine set_accuracy;

    public IEnumerator SetAccuracy(int num)
    {
        note_accuracy_text.enabled = true;

        switch (num)
        {
            case 0:
                note_accuracy_text.text = "Perfect";
                break;
            case 1:
                note_accuracy_text.text = "Great";
                break;
            case 2:
                note_accuracy_text.text = "Good";
                break;
            case 3:
                note_accuracy_text.text = "Ok";
                break;
            case 4:
                note_accuracy_text.text = "Miss";
                break;
        }

        yield return new WaitForSeconds(note_accuracy_remove_time);
        note_accuracy_text.enabled = false;
    }
    #endregion

    #region 노트 효과
    public ClickEffect[] click_effect = new ClickEffect[4];
    #endregion

    void Awake()
    {
        mask = LayerMask.GetMask("Note");
        combo = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            CheckNote(0);
        if (Input.GetKeyDown(KeyCode.F))
            CheckNote(1);
        if (Input.GetKeyDown(KeyCode.J))
            CheckNote(2);
        if (Input.GetKeyDown(KeyCode.K))
            CheckNote(3);

        if (Input.GetKeyDown(KeyCode.E))
            SpawnNote(0);
        if (Input.GetKeyDown(KeyCode.R))
            SpawnNote(1);
        if (Input.GetKeyDown(KeyCode.U))
            SpawnNote(2);
        if (Input.GetKeyDown(KeyCode.I))
            SpawnNote(3);
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

            SetCombo(combo + 1);
            other[0].gameObject.GetComponent<Note>().check = true;
            other[0].gameObject.GetComponent<Note>().OnClicked();
            click_effect[area].ShowEffect();

            if(set_accuracy != null)
                StopCoroutine(set_accuracy);            
            set_accuracy = StartCoroutine(SetAccuracy(i));

            return;
        }

        SetCombo(0); //4개의 콜라이더를 모두 검사해서 검색된 노트가 없는 경우
        if (set_accuracy != null)
            StopCoroutine(set_accuracy);
        set_accuracy = StartCoroutine(SetAccuracy(4));
    }

    void SpawnNote(int area)
    {
        Vector3 spawn_position = new Vector3(0, 0.05f, 55);

        switch (area)
        {
            case 0:
                spawn_position.x = -3;
                break;
            case 1:
                spawn_position.x = -1;
                break;
            case 2:
                spawn_position.x = 1;
                break;
            case 3:
                spawn_position.x = 3;
                break;
        }

        Instantiate(note_prefab, spawn_position, Quaternion.identity);
    }
}