using UnityEngine;
using TMPro;
using System.Collections;

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
    private bool isPostQuestTalking = false;

    private Coroutine typingCoroutine;

    // --- Typing effect flags ---
    private bool isTyping = false;
    private string currentFullText = "";

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

            ShowTyping(!hasTalked ? "Nhấn E để trò chuyện" : "Nhấn E để xem nhiệm vụ");
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
            // Nếu đang gõ chữ → skip chữ, KHÔNG qua câu mới
            if (isTyping)
            {
                isTyping = false;
                uiText.text = currentFullText;
                return;
            }

            // Sau khi chữ gõ xong mới xử lý logic thoại/nhiệm vụ
            if (quest.isCompleted)
            {
                HandlePostQuestDialogue();
                return;
            }

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

    // =====================
    // TYPING EFFECT
    // =====================
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        currentFullText = text;
        uiText.text = "";

        foreach (char c in text)
        {
            if (!isTyping)
            {
                uiText.text = currentFullText;
                yield break;
            }

            uiText.text += c;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
    }

    void ShowTyping(string text)
    {
        // Nếu đang gõ chữ và nhấn E → skip
        if (isTyping)
        {
            isTyping = false;
            uiText.text = currentFullText;
            return;
        }

        // Nếu không gõ → bắt đầu gõ câu mới
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    // =====================
    // THOẠI TRƯỚC NHIỆM VỤ
    // =====================
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
        speakerText.text = speakerLines[dialogueIndex];
        ShowTyping(dialogueLines[dialogueIndex]);
    }

    // =====================
    // XỬ LÝ NHIỆM VỤ
    // =====================
    void HandleQuest()
    {
        speakerText.text = "NPC";

        if (!quest.isAccepted)
        {
            ShowTyping($"Nhiệm vụ: {quest.questName}\n\n{quest.description}\n\nNhấn E để nhận nhiệm vụ");
            quest.isAccepted = true;
            QuestManager.Instance.currentQuest = quest;
            return;
        }

        if (!quest.isCompleted)
        {
            if (quest.questType == QuestType.Kill)
            {
                ShowTyping($"Tiến độ: {quest.currentKillCount}/{quest.requiredKillCount}\nHãy tiêu diệt đủ quái!");
            }
            else if (quest.questType == QuestType.CollectItem)
            {
                ShowTyping(
                    quest.hasCollectedItem
                    ? "Bạn đã nhặt được vật phẩm, quay lại đưa cho tôi!"
                    : $"Hãy tìm và nhặt vật phẩm: {quest.requiredItemName}"
                );
            }
            return;
        }

        // Bắt đầu thoại hậu nhiệm vụ
        dialogueIndex = 0;
        isPostQuestTalking = true;
    }

    // =====================
    // THOẠI SAU KHI HOÀN THÀNH
    // =====================
    void HandlePostQuestDialogue()
    {
        if (!isPostQuestTalking)
        {
            isPostQuestTalking = true;
            dialogueIndex = 0;
            ShowPostQuestDialogue();
            return;
        }

        dialogueIndex++;

        if (dialogueIndex < postQuestDialogue.Length)
        {
            ShowPostQuestDialogue();
        }
        else
        {
            EndPostQuestEffect();
        }
    }

    void ShowPostQuestDialogue()
    {
        speakerText.text = postQuestSpeaker[dialogueIndex];
        ShowTyping(postQuestDialogue[dialogueIndex]);
    }

    void EndPostQuestEffect()
    {
        speakerText.text = "NPC";

        if (quest.questType == QuestType.Kill)
        {
            ShowTyping("🎉 Bạn đã hoàn thành nhiệm vụ tiêu diệt quái!\nĐã mở khóa skill Circle Slash!");
            SkillManager.Instance.UnlockCircleSlash();
        }
        else if (quest.questType == QuestType.CollectItem)
        {
            ShowTyping("🎉 Bạn đã giao vật phẩm thành công!\nCổng dịch chuyển đã xuất hiện!");
        }

        isPostQuestTalking = false;
    }
}
