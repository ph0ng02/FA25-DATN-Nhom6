using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogueNV1 : MonoBehaviour
{
    [Header("Dialogue (Only talk)")]
    public string[] dialogueLines;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    // 0 = manual (chỉ gọi bằng Continue())
    // >0 = tự chuyển sau X giây
    public float autoLineDelay = 0f;

    private int currentIndex = 0;
    private Coroutine autoCoroutine;

    void Awake()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.Log("[NV1] No dialogue lines");
            return;
        }

        currentIndex = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentIndex];

        if (autoLineDelay > 0f)
        {
            autoCoroutine = StartCoroutine(AutoNext());
        }
    }

    public void Continue()
    {
        if (autoCoroutine != null)
        {
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }

        currentIndex++;

        if (currentIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentIndex];

            if (autoLineDelay > 0)
                autoCoroutine = StartCoroutine(AutoNext());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator AutoNext()
    {
        yield return new WaitForSecondsRealtime(autoLineDelay);
        Continue();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public bool IsActive()
    {
        return dialoguePanel != null && dialoguePanel.activeSelf;
    }
}
