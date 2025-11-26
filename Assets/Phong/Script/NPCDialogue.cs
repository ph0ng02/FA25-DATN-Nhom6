using UnityEngine;
using TMPro;
using System;

public class NPCDialogue : MonoBehaviour
{
    [Header("Cấu hình NPC")]
    public string npcName = "NPC";

    [Header("Nội dung hội thoại")]
    [TextArea(3, 10)]
    public string[] dialogueLines;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;

    private int currentLineIndex = 0;
    private bool isPlayerInRange = false;

    // biến để Nira_NPC hoặc script cũ check
    public bool isTalking { get; private set; } = false;

    // Event callback khi kết thúc thoại
    public Action OnDialogueEnd;

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue()
    {
        isTalking = true;
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;

        if (nameText != null) nameText.text = npcName;

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isTalking = false;
        currentLineIndex = 0;
        Debug.Log("Kết thúc hội thoại");

        // Gọi event cho các script khác
        OnDialogueEnd?.Invoke();

        // Kiểm tra quest nếu cần
        CheckForQuest();
    }

    void CheckForQuest()
    {
        // Xử lý nhận nhiệm vụ ở đây nếu muốn
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Đã vào vùng NPC");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialoguePanel.SetActive(false);
            isTalking = false;
        }
    }
}
