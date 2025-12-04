using UnityEngine;
using TMPro;

public class DialogueQuestNPC : MonoBehaviour
{
    [Header("Dialogue (Before Quest)")]
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public string[] speakerLines;

    [Header("Dialogue After Completing Quest")]
    [TextArea(3, 10)]
    public string[] postQuestDialogue;
    public string[] postQuestSpeaker;

    private int dialogueIndex = 0;

    [Header("Quest")]
    public Quest quest;

    [Header("UI")]
    public GameObject uiPanel;
    public TextMeshProUGUI uiText;
    public TextMeshProUGUI speakerText;

    private bool playerInside = false;
    private bool isTalking = false;
    private bool hasTalked = false;

    private bool isPostQuestTalking = false;   // <--- trạng thái nói thoại sau khi hoàn thành

    void Start()
    {
        uiPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            uiPanel.SetActive(true);

            if (!hasTalked)
                uiText.text = "Nhấn E để trò chuyện";
            else
                uiText.text = "Nhấn E để xem nhiệm vụ";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            isTalking = false;
            isPostQuestTalking = false;
            dialogueIndex = 0;
            uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Nếu đã hoàn thành nhiệm vụ → vào chế độ thoại hậu nhiệm vụ
            if (quest.isCompleted)
            {
                HandlePostQuestDialogue();
                return;
            }

            // Nếu đã nói thoại đầu rồi → vào nhiệm vụ
            if (hasTalked)
            {
                HandleQuest();
                return;
            }

            // Chưa nói -> nói thoại đầu
            if (!isTalking)
                StartDialogue();
            else
                NextDialogue();
        }
    }

    // ========== THOẠI TRƯỚC NHIỆM VỤ ==========
    void StartDialogue()
    {
        isTalking = true;
        dialogueIndex = 0;
        ShowDialogue();
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogueLines.Length)
        {
            ShowDialogue();
        }
        else
        {
            hasTalked = true;
            HandleQuest();
        }
    }

    void ShowDialogue()
    {
        uiText.text = dialogueLines[dialogueIndex];
        speakerText.text = speakerLines[dialogueIndex];
    }

    // ========== NHIỆM VỤ ==========
    void HandleQuest()
    {
        speakerText.text = "NPC";

        if (!quest.isAccepted)
        {
            uiText.text =
                $"Nhiệm vụ: {quest.questName}\n\n{quest.description}\n\nNhấn E để nhận nhiệm vụ";

            quest.isAccepted = true;
            QuestManager.Instance.currentQuest = quest;
            return;
        }

        if (!quest.isCompleted)
        {
            if (quest.questType == QuestType.Kill)
            {
                uiText.text =
                    $"Tiến độ: {quest.currentKillCount}/{quest.requiredKillCount}\nHãy tiêu diệt đủ quái!";
            }
            else if (quest.questType == QuestType.CollectItem)
            {
                uiText.text =
                    quest.hasCollectedItem
                        ? "Bạn đã nhặt được vật phẩm, quay lại đưa cho tôi!"
                        : $"Hãy tìm và nhặt vật phẩm: {quest.requiredItemName}";
            }
            return;
        }

        // Khi vừa hoàn thành → kích hoạt thoại hậu nhiệm vụ
        dialogueIndex = 0;
        isPostQuestTalking = true;
    }

    // ========== THOẠI SAU KHI HOÀN THÀNH NHIỆM VỤ ==========
    void HandlePostQuestDialogue()
    {
        if (!isPostQuestTalking)
        {
            // Bắt đầu thoại sau nhiệm vụ
            isPostQuestTalking = true;
            dialogueIndex = 0;
            ShowPostQuestDialogue();
            return;
        }

        // Next line
        dialogueIndex++;
        if (dialogueIndex < postQuestDialogue.Length)
        {
            ShowPostQuestDialogue();
        }
        else
        {
            // kết thúc thoại hậu nhiệm vụ
            EndPostQuestEffect();
        }
    }

    void ShowPostQuestDialogue()
    {
        uiText.text = postQuestDialogue[dialogueIndex];
        speakerText.text = postQuestSpeaker[dialogueIndex];
    }

    void EndPostQuestEffect()
    {
        speakerText.text = "NPC";

        if (quest.questType == QuestType.Kill)
        {
            uiText.text =
                $"🎉 Bạn đã hoàn thành nhiệm vụ tiêu diệt quái!\nĐã mở khóa skill Circle Slash!";
            SkillManager.Instance.UnlockCircleSlash();
        }
        else if (quest.questType == QuestType.CollectItem)
        {
            uiText.text =
                $"🎉 Bạn đã giao vật phẩm thành công!\nCổng dịch chuyển đã xuất hiện!";
        }

        isPostQuestTalking = false;
    }
}
