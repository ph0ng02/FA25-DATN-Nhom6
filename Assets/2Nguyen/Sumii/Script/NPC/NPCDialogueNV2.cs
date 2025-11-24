//using UnityEngine;
//using TMPro;

//public class NPCDialogueNV2 : MonoBehaviour
//{
//    // --- Các trường Hội thoại ---
//    public string npcName = "NPC";
//    [TextArea(3, 10)]
//    public string[] dialogueLines;    // Các câu hội thoại

//    public GameObject dialogueUI;
//    public TextMeshProUGUI dialogueText;

//    // --- Bổ sung cho Hệ thống Nhiệm vụ ---
//    public Quest questToGive; // Dữ liệu nhiệm vụ cần giao

//    private QuestManager questManager;
//    private bool hasGivenQuest = false; // Tránh giao lại

//    // --- Các biến trạng thái ---
//    private int currentLine = 0;
//    private bool isPlayerInside = false;
//    private bool isTalking = false;

//    void Start()
//    {
//        if (dialogueUI != null)
//            dialogueUI.SetActive(false);

//        // Khắc phục lỗi thời và tự động tìm QuestManager
//        questManager = FindFirstObjectByType<QuestManager>();

//        if (questManager == null)
//        {
//            Debug.LogError("QuestManager không được tìm thấy trong Scene. Nhiệm vụ sẽ không được giao.");
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isPlayerInside = true;
//            Debug.Log("Player entered NPC zone");
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isPlayerInside = false;
//            if (isTalking)
//            {
//                EndDialogue();
//            }
//        }
//    }

//    void Update()
//    {
//        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
//        {
//            if (!isTalking)
//            {
//                StartDialogue();
//            }
//            else
//            {
//                NextLine();
//            }
//        }
//    }

//    void StartDialogue()
//    {
//        if (dialogueLines.Length == 0) return;

//        isTalking = true;
//        currentLine = 0;
//        dialogueUI.SetActive(true);
//        dialogueText.text = dialogueLines[currentLine];
//    }

//    void NextLine()
//    {
//        currentLine++;

//        if (currentLine >= dialogueLines.Length)
//        {
//            EndDialogue();
//            return;
//        }

//        dialogueText.text = dialogueLines[currentLine];
//    }

//    void EndDialogue()
//    {
//        dialogueUI.SetActive(false);
//        isTalking = false;
//        currentLine = 0;

//        // ⭐️ LOGIC GIAO NHIỆM VỤ ⭐️
//        if (questManager != null && questToGive != null && !hasGivenQuest)
//        {
//            questManager.StartQuestFromNPC(questToGive);

//            hasGivenQuest = true;
//            Debug.Log($"Nhiệm vụ: {questToGive.questName} đã được giao thành công!");
//        }
//    }
//}