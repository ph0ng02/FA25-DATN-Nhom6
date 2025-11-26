using UnityEngine;
using TMPro;

public class NPCDialogueNV2 : MonoBehaviour
{
    // --- Các trường Hội thoại và UI ---
    public string npcName = "NPC";
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    // --- Bổ sung cho Hệ thống Nhiệm vụ ---
    public QuestNV questToGive;
    private QuestManagerNV questManager; // Đảm bảo class này tồn tại và tên khớp
    private bool hasGivenQuest = false;

    // --- Các biến trạng thái ---
    private int currentLine = 0;
    private bool isPlayerInside = false;
    private bool isTalking = false;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        // Sử dụng FindFirstObjectByType để tìm QuestManagerNV
        questManager = FindFirstObjectByType<QuestManagerNV>();

        if (questManager == null)
        {
            Debug.LogError("QuestManagerNV không được tìm thấy. Đảm bảo tên file và class khớp.");
        }
    }

    // ⭐️ SỬ DỤNG ONTRIGGERSTAY ĐỂ KIỂM TRA LIÊN TỤC ⭐️
    // Giúp xử lý các lỗi vật lý khi Player chạm vào
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            // (Bạn không cần Debug.Log ở đây, vì nó sẽ spam Console)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (isTalking)
            {
                EndDialogue();
            }
        }
    }

    void Update()
    {
        // Kích hoạt khi Player ở bên trong và nhấn E
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        if (dialogueLines.Length == 0) return;

        isTalking = true;
        currentLine = 0;
        dialogueUI.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
    }

    // ⭐️ PHẢI LÀ PUBLIC ĐỂ NÚT CONTINUE BUTTON CÓ THỂ GỌI ĐƯỢC ⭐️
    public void NextLine()
    {
        if (!isTalking) return; // Bảo vệ hàm NextLine

        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueLines[currentLine];
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isTalking = false;
        currentLine = 0;

        // LOGIC GIAO NHIỆM VỤ
        if (questManager != null && questToGive != null && !hasGivenQuest)
        {
            questManager.StartQuestFromNPC(questToGive);
            hasGivenQuest = true;
            Debug.Log($"Nhiệm vụ: {questToGive.questName} đã được giao thành công!");
        }
    }
}