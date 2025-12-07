using UnityEngine;
using TMPro;
using System.Collections;

public class MonsterIntroTrigger : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public Camera introCamera;

    [Header("Monster")]
    public GameObject monsterPrefab;
    public Transform spawnPoint;
    private GameObject spawnedMonster;
    private Animator monsterAnimator;

    [Header("Camera Follow Settings")]
    public Vector3 cameraOffset = new Vector3(0, 3, -5);
    public float followSpeed = 3f;
    public float rotateSpeed = 5f;

    [Header("Intro UI")]
    public TextMeshProUGUI monsterIntroText;
    public string monsterName = "Nightmare Dragon";
    public float textFadeDuration = 0.8f;

    [Header("Intro Settings")]
    public float introDuration = 3f;

    private bool hasPlayed = false;

    private void Start()
    {
        // ✔ Đặt alpha = 0 để chắc chắn text ẩn từ đầu
        if (monsterIntroText != null)
        {
            Color c = monsterIntroText.color;
            c.a = 0;
            monsterIntroText.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayIntro());
        }
    }

    IEnumerator PlayIntro()
    {
        hasPlayed = true;

        // Spawn quái
        spawnedMonster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        monsterAnimator = spawnedMonster.GetComponent<Animator>();

        if (monsterAnimator != null)
            monsterAnimator.SetTrigger("Intro");

        // Chuyển camera
        playerCamera.gameObject.SetActive(false);
        introCamera.gameObject.SetActive(true);

        // ➤ Hiện chữ
        monsterIntroText.text = monsterName;
        StartCoroutine(FadeTextIn(monsterIntroText));

        float timer = 0f;
        while (timer < introDuration)
        {
            if (spawnedMonster != null)
            {
                Vector3 targetPos = spawnedMonster.transform.position + cameraOffset;

                introCamera.transform.position = Vector3.Lerp(
                    introCamera.transform.position,
                    targetPos,
                    Time.deltaTime * followSpeed
                );

                Quaternion lookRot = Quaternion.LookRotation(
                    spawnedMonster.transform.position - introCamera.transform.position
                );

                introCamera.transform.rotation = Quaternion.Slerp(
                    introCamera.transform.rotation,
                    lookRot,
                    Time.deltaTime * rotateSpeed
                );
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Ẩn chữ bằng fade-out
        yield return StartCoroutine(FadeTextOut(monsterIntroText));

        // ✔ Tắt hẳn chữ để không bao giờ hiện lại
        monsterIntroText.gameObject.SetActive(false);

        // Trả lại camera player
        introCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        Destroy(gameObject);
    }

    IEnumerator FadeTextIn(TextMeshProUGUI text)
    {
        float t = 0;
        while (t < textFadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, t / textFadeDuration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
    }

    IEnumerator FadeTextOut(TextMeshProUGUI text)
    {
        float t = 0;
        while (t < textFadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, t / textFadeDuration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
}
