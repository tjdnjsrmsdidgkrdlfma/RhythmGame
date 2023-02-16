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
                note_manager.SetCombo(0);
                StartCoroutine(note_manager.SetAccuracy(4));
            }
            Destroy(other.gameObject);
        }
    }
}
