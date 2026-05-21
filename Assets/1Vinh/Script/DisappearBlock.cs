using System.Collections;
using UnityEngine;

public class DisappearBlock : MonoBehaviour
{
    public float standTime = 1.5f;
    public float fadeTime = 1f;
    public float respawnTime = 3f;

    private Renderer rend;
    private Collider col;
    private bool isTriggered;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        // Clone material để không ảnh hưởng object khác
        rend.material = new Material(rend.material);
        originalColor = rend.material.color;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(FadeAndDisappear());
        }
    }

    IEnumerator FadeAndDisappear()
    {
        yield return new WaitForSeconds(standTime);

        // ===== FADE OUT =====
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, t / fadeTime));
            yield return null;
        }

        // Tắt block
        col.enabled = false;
        rend.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // ===== RESPAWN =====
        rend.enabled = true;
        col.enabled = true;

        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0f, 1f, t / fadeTime));
            yield return null;
        }

        isTriggered = false;
    }

    void SetAlpha(float alpha)
    {
        Color c = originalColor;
        c.a = alpha;
        rend.material.color = c;
    }
}
