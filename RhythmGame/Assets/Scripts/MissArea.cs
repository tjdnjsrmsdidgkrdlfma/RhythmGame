using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissArea : MonoBehaviour
{
    public NoteManager note_manager;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            if (other.GetComponent<Note>().check == false)
            {
                note_manager.Combo = 0;
                StartCoroutine(note_manager.SetAccuracy((int)NoteManager.Accuracy.Miss));
            }
            Destroy(other.gameObject);
        }
    }
}
