using UnityEngine;
using System.Collections;

public class NPCDialogueTrigger : MonoBehaviour
{
    public string npcName = "NPC";

    [Header("Dialogue Settings")]
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public string[] speakerLines;

    [Header("UI")]
    public GameObject dialogueUI;
    public TMPro.TextMeshProUGUI dialogueText;
    public TMPro.TextMeshProUGUI speakerText;

    [Header("Typing Effect")]
    public float typingSpeed = 0.02f; // tốc độ đánh chữ
    private Coroutine typingCoroutine;

    private int currentLine = 0;
    private bool isPlayerInside = false;
    private bool isTalking = false;
    private bool isTyping = false; // đang chạy từng chữ

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            EndDialogue();
        }
    }

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                // Nếu đang gõ chữ → skip và hiện nguyên câu
                if (isTyping)
                {
                    SkipTyping();
                }
                else
                {
                    NextLine();
                }
            }
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;
        dialogueUI.SetActive(true);
        ShowLine();
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        dialogueText.text = "";

        string speaker = (currentLine < speakerLines.Length) ? speakerLines[currentLine] : npcName;
        if (speakerText != null)
            speakerText.text = speaker;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialogueLines[currentLine]));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialogueLines[currentLine];
        isTyping = false;
    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueUI.SetActive(false);
        isTalking = false;
        currentLine = 0;
        isTyping = false;
    }
}
