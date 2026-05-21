using UnityEngine;

public class SlashVFX : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
