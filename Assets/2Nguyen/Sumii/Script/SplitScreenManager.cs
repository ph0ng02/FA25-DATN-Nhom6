using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public Camera cam1, cam2;
    public Transform player1, player2;
    public float mergeDistance = 10f;

    void Update()
    {
        float dist = Vector3.Distance(player1.position, player2.position);

        if (dist < mergeDistance)
        {
            // Hợp 2 camera lại (1 full screen)
            cam1.rect = new Rect(0, 0, 1, 1);
            cam2.enabled = false;
        }
        else
        {
            // Chia đôi màn hình
            cam1.rect = new Rect(0, 0, 0.5f, 1);
            cam2.rect = new Rect(0.5f, 0, 0.5f, 1);
            cam2.enabled = true;
        }
    }
}
