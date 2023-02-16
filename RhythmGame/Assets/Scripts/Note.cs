using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed;
    public float destroy_time;
    public bool check;

    public GameObject particle;

    MeshRenderer mesh_renderer;
    BoxCollider box_collider;
    Rigidbody rb;

    void Awake()
    {
        check = false;

        rb = GetComponent<Rigidbody>();
        mesh_renderer = GetComponent<MeshRenderer>();
        box_collider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        rb.velocity = -Vector3.forward * speed;
    }

    public void OnClicked()
    {
        mesh_renderer.enabled = false;
        box_collider.enabled = false;
        rb.velocity = Vector3.zero;

        particle.SetActive(true);

        Destroy(gameObject, destroy_time);
    }
}
