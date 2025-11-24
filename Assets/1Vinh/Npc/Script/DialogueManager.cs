using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float typingSpeed = 0.03f;   // tốc độ chạy chữ
    public float nextLineDelay = 1.2f;  // thời gian chờ trước khi sang câu tiếp theo
    public float autoCloseDelay = 1.5f; // thời gian chờ sau câu cuối cùng

    private string[] lines;
    private int index;
    private Coroutine typingCoroutine;

    public static DialogueManager Instance;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        index = 0;

        dialoguePanel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        while (index < lines.Length)
        {
            dialogueText.text = "";

            // Chạy chữ từng ký tự
            foreach (char c in lines[index].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            // Chờ x giây rồi sang câu tiếp
            yield return new WaitForSeconds(nextLineDelay);

            index++;
        }

        // Khi hết câu → tự tắt
        yield return new WaitForSeconds(autoCloseDelay);
        dialoguePanel.SetActive(false);
    }
}
