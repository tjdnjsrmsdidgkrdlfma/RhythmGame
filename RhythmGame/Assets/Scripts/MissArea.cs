using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissArea : MonoBehaviour
{
    InGameManager in_game_manager;

    void Awake()
    {
        in_game_manager = FindObjectOfType<InGameManager>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            if (other.GetComponent<Note>().check == false)
            {
                in_game_manager.Combo = 0;
                StartCoroutine(in_game_manager.SetAccuracy((int)InGameManager.Accuracy.Miss));
            }
            
            other.gameObject.SetActive(false);
        }
    }
}