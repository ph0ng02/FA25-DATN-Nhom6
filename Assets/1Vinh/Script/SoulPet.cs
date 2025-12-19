using UnityEngine;

public class SoulPet : MonoBehaviour
{
    public Transform target;

    [Header("Follow")]
    public Vector3 followOffset = new Vector3(0, 1.5f, -1f);
    public float followSpeed = 5f;

    [Header("Orbit")]
    public float orbitRadius = 1.5f;
    public float orbitHeight = 1.8f;
    public float orbitSpeed = 120f;        // độ / giây
    public float orbitDuration = 2.5f;     // bay quanh bao lâu
    public Vector2 orbitCooldown = new Vector2(4f, 7f); // bao lâu thì bay vòng

    private bool followPlayer = false;
    private bool isOrbiting = false;
    private float orbitAngle = 0f;

    void Start()
    {
        Invoke(nameof(StartOrbit), Random.Range(orbitCooldown.x, orbitCooldown.y));
    }

    void Update()
    {
        if (!followPlayer || target == null) return;

        if (isOrbiting)
        {
            OrbitAroundPlayer();
        }
        else
        {
            FollowPlayer();
        }

        transform.LookAt(target);
    }

    void FollowPlayer()
    {
        Vector3 desiredPos = target.position + followOffset;

        float hover = Mathf.Sin(Time.time * 2f) * 0.2f;
        desiredPos.y += hover;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );
    }

    void OrbitAroundPlayer()
    {
        orbitAngle += orbitSpeed * Time.deltaTime;

        float rad = orbitAngle * Mathf.Deg2Rad;
        Vector3 orbitPos = new Vector3(
            Mathf.Cos(rad) * orbitRadius,
            orbitHeight + Mathf.Sin(Time.time * 3f) * 0.2f,
            Mathf.Sin(rad) * orbitRadius
        );

        transform.position = Vector3.Lerp(
            transform.position,
            target.position + orbitPos,
            followSpeed * Time.deltaTime
        );
    }

    void StartOrbit()
    {
        if (!followPlayer) return;

        isOrbiting = true;
        orbitAngle = Random.Range(0f, 360f);

        Invoke(nameof(StopOrbit), orbitDuration);
    }

    void StopOrbit()
    {
        isOrbiting = false;

        Invoke(nameof(StartOrbit), Random.Range(orbitCooldown.x, orbitCooldown.y));
    }

    public void StartFollow(Transform player)
    {
        target = player;
        followPlayer = true;
    }
}
