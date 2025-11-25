using UnityEngine;

public class QuestFollowTarget : MonoBehaviour
{
    public Transform target;
    private Transform player;
    public float hideDistance = 10f; // khoảng cách ẩn/hiện
    public float extraHeight = 0.5f;
    private Canvas canvas;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        canvas = GetComponentInChildren<Canvas>();
    }

    void Update()
    {
        if (target == null || player == null) return;


        float height = 2f;
        Collider col = target.GetComponentInChildren<Collider>();
        if (col != null)
        {
            height = col.bounds.size.y;
        }

        transform.position = target.position + Vector3.up * (height + extraHeight);
        transform.LookAt(Camera.main.transform);


        float distance = Vector3.Distance(transform.position, player.position);
        canvas.enabled = distance > hideDistance;

    }
}
