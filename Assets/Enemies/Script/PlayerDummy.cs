using UnityEngine;

public class PlayerDummy : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        if (move != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(move);
    }
}