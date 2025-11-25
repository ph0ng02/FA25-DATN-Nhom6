using UnityEngine;
using TMPro;

public class NPCDialogueNV2 : MonoBehaviour
{
    // ... Các trường hội thoại (giữ nguyên) ...
    public string npcName = "NPC";
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    // --- Bổ sung cho Hệ thống Nhiệm vụ ---

    public QuestNV questToGive;

    // Thay QuestManager bằng QuestManagerNV
    private QuestManagerNV questManager;
    private bool hasGivenQuest = false;

    // ... Các biến trạng thái (giữ nguyên) ...
    private int currentLine = 0;
    private bool isPlayerInside = false;
    private bool isTalking = false;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        
        questManager = FindFirstObjectByType<QuestManagerNV>();

        if (questManager == null)
        {
            Debug.LogError("QuestManagerNV không được tìm thấy trong Scene. Nhiệm vụ sẽ không được giao.");
        }
    }


    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isTalking = false;
        currentLine = 0;

        // ⭐️ LOGIC GIAO NHIỆM VỤ ⭐️
       
        if (questManager != null && questToGive != null && !hasGivenQuest)
        {
       
            questManager.StartQuestFromNPC(questToGive);

            hasGivenQuest = true;
            Debug.Log($"Nhiệm vụ: {questToGive.questName} đã được giao thành công!");
        }
    }
}