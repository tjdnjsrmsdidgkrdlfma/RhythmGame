using UnityEngine;

public class Note : MonoBehaviour
{
    #region 노트 이동 속도
    [Header("노트 이동 속도")]
    [SerializeField] float speed;
    #endregion

    #region 노트 클릭 여부 확인
    [Header("노트 클릭 여부 확인")]
    public bool check;
    #endregion

    #region 파티클
    [Header("파티클")]
    [SerializeField] float destroy_time;
    [SerializeField] GameObject particle;
    #endregion

    MeshRenderer mesh_renderer;
    BoxCollider box_collider;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh_renderer = GetComponent<MeshRenderer>();
        box_collider = GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        check = false;

        mesh_renderer.enabled = true;
        box_collider.enabled = true;

        particle.SetActive(false);

        rb.velocity = -Vector3.forward * speed * GameManager.game_manager.note_speed;
    }

    public void OnClicked()
    {
        rb.velocity = Vector3.zero;

        mesh_renderer.enabled = false;
        box_collider.enabled = false;
        
        particle.SetActive(true);

        Destroy(gameObject, destroy_time);
    }
}
