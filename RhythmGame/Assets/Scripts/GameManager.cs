using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoxCollider[] d_check_collider = new BoxCollider[4];
    public BoxCollider[] f_check_collider = new BoxCollider[4];
    public BoxCollider[] j_check_collider = new BoxCollider[4];
    public BoxCollider[] k_check_collider = new BoxCollider[4];

    LayerMask mask;

    public GameObject note_prefab;

    void Awake()
    {
        mask = LayerMask.GetMask("Note");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            CheckNote(d_check_collider);
        if(Input.GetKeyDown(KeyCode.F))
            CheckNote(f_check_collider);
        if (Input.GetKeyDown(KeyCode.J))
            CheckNote(j_check_collider);
        if (Input.GetKeyDown(KeyCode.K))
            CheckNote(k_check_collider);

        if (Input.GetKeyDown(KeyCode.E))
            SpawnNote(0);
        if (Input.GetKeyDown(KeyCode.R))
            SpawnNote(1);
        if (Input.GetKeyDown(KeyCode.U))
            SpawnNote(2);
        if (Input.GetKeyDown(KeyCode.I))
            SpawnNote(3);
    }

    void CheckNote(BoxCollider[] check_collider)
    {
        Collider[] other;

        for (int i = 0; i < check_collider.Length; i++)
        {
            other = Physics.OverlapBox(check_collider[i].center,
                                       check_collider[i].size / 2,
                                       Quaternion.identity,
                                       mask);

            if (other.Length == 0)
                continue; //검색된 노트가 없으면 다음 콜라이더로 넘어간다

            switch(i)
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
    }

    void SpawnNote(int area)
    {
        Vector3 spawn_position = new Vector3(0, 0.75f, 55);

        switch(area)
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