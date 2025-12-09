using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MonsterIntroTrigger : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public Camera introCamera;

    [Header("Monster")]
    public GameObject monsterPrefab;
    public Transform spawnPoint;

    [Header("Monster Spawn Settings")]
    public int spawnCount = 3;

    [Header("Random Spawn Settings")]
    public float spawnRadius = 5f;   // ✔ Spawn random trong bán kính này

    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private Animator firstMonsterAnimator;

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

        // ✔ Spawn nhiều quái ngẫu nhiên
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPos = new Vector3(
                spawnPoint.position.x + randomPos.x,
                spawnPoint.position.y,
                spawnPoint.position.z + randomPos.y
            );

            GameObject m = Instantiate(monsterPrefab, spawnPos, spawnPoint.rotation);
            spawnedMonsters.Add(m);
        }

        // Chọn con đầu tiên cho intro
        firstMonsterAnimator = spawnedMonsters[0].GetComponent<Animator>();
        if (firstMonsterAnimator != null)
            firstMonsterAnimator.SetTrigger("Intro");

        // Camera switch
        playerCamera.gameObject.SetActive(false);
        introCamera.gameObject.SetActive(true);

        // Hiện chữ
        monsterIntroText.text = monsterName;
        StartCoroutine(FadeTextIn(monsterIntroText));

        float timer = 0f;
        GameObject targetMonster = spawnedMonsters[0];

        while (timer < introDuration)
        {
            if (targetMonster != null)
            {
                Vector3 targetPos = targetMonster.transform.position + cameraOffset;
                introCamera.transform.position = Vector3.Lerp(introCamera.transform.position, targetPos, Time.deltaTime * followSpeed);

                Quaternion lookRot = Quaternion.LookRotation(targetMonster.transform.position - introCamera.transform.position);
                introCamera.transform.rotation = Quaternion.Slerp(introCamera.transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Ẩn chữ
        yield return StartCoroutine(FadeTextOut(monsterIntroText));
        monsterIntroText.gameObject.SetActive(false);

        // Trả camera
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
