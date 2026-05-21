using UnityEngine;

public class DamageTrap : MonoBehaviour
{
    public float damage = 10f;
    public float interval = 1f;

    [Header("VFX")]
    public ParticleSystem redVFX;

    float timer;

    void Start()
    {
        // Đảm bảo lúc load scene thì VFX TẮT
        if (redVFX != null)
            redVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void OnEnable()
    {
        // 🔥 TRAP BẬT → PLAY VFX
        if (redVFX != null)
        {
            redVFX.Clear();
            redVFX.Play();
        }
    }

    void OnDisable()
    {
        // 🔥 TRAP TẮT → STOP VFX
        if (redVFX != null)
            redVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        HealthManagement hp = other.GetComponent<HealthManagement>();
        if (hp == null) return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            hp.TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
    }
}
