using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class check_collider_1d
    {
        public BoxCollider[] check_collider = new BoxCollider[4];
    }

    public check_collider_1d[] check_collider_2d = new check_collider_1d[4];

    //public BoxCollider[,] check_collider = new BoxCollider[4, 4];
    //public BoxCollider[] d_check_collider = new BoxCollider[4]; //2���� �迭�� ���� CheckNote�Լ��� ���ڸ� ���ڷ� �� �� �ֵ���
    //public BoxCollider[] f_check_collider = new BoxCollider[4];
    //public BoxCollider[] j_check_collider = new BoxCollider[4];
    //public BoxCollider[] k_check_collider = new BoxCollider[4];

    LayerMask mask;

    public GameObject note_prefab;

    void Awake()
    {
        mask = LayerMask.GetMask("Note");
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
                continue; //�˻��� ��Ʈ�� ������ ���� �ݶ��̴��� �Ѿ��

            switch (i)
            {
                case 0:
                    Debug.Log("Perfect");
                    break;
                case 1:
                    Debug.Log("Great");
                    break;
                case 2:
                    Debug.Log("Good");
                    break;
                case 3:
                    Debug.Log("Ok");
                    break;
            }

            return;
        }

        Debug.Log("Nothing"); //4���� �ݶ��̴��� ��� �˻��ؼ� �˻��� ��Ʈ�� ���� ���
    }

    void SpawnNote(int area)
    {
        Vector3 spawn_position = new Vector3(0, 0.75f, 55);

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