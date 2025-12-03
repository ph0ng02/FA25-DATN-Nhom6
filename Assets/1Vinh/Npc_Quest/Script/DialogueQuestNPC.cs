using UnityEngine;
using TMPro;

public class DialogueQuestNPC : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public string[] speakerLines;
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
            dialogueIndex = 0;
            uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (hasTalked)
            {
                HandleQuest();
                return;
            }

            if (!isTalking)
                StartDialogue();
            else
                NextDialogue();
        }
    }

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

    void HandleQuest()
    {
        speakerText.text = "NPC";

        // 1. Chưa nhận nhiệm vụ
        if (!quest.isAccepted)
        {
            uiText.text =
                $"Nhiệm vụ: {quest.questName}\n\n{quest.description}\n\nNhấn E để nhận nhiệm vụ";

            quest.isAccepted = true;
            QuestManager.Instance.currentQuest = quest;
            return;
        }

        // 2. Đang làm nhiệm vụ
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

        // 3. Đã hoàn thành nhiệm vụ
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
    }
}
