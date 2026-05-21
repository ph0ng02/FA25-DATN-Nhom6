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

    // Typing effect
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
        if (!playerInside) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                isTyping = false;
                uiText.text = currentFullText;
                return;
            }

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
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    // =====================
    // DIALOGUE
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
    // QUEST LOGIC
    // =====================
    void HandleQuest()
    {
        speakerText.text = "NPC";

        if (!quest.isAccepted)
        {
            quest.isAccepted = true;
            QuestManager.Instance.currentQuest = quest;

            ShowTyping(
                $"Nhiệm vụ: {quest.questName}\n\n{quest.description}\n\nNhấn E để nhận nhiệm vụ"
            );
            return;
        }

        if (!quest.isCompleted)
        {
            if (quest.questType == QuestType.Kill)
            {
                ShowTyping(
                    $"Tiến độ: {quest.currentKillCount}/{quest.requiredKillCount}\nHãy tiêu diệt đủ quái!"
                );
            }
            else if (quest.questType == QuestType.CollectItem)
            {
                if (quest.currentItemCount >= quest.requiredItemCount)
                {
                    ShowTyping("Bạn đã thu thập đủ vật phẩm!\nQuay lại gặp tôi!");
                    quest.isCompleted = true;
                }
                else
                {
                    ShowTyping(
                        $"Vật phẩm cần: {quest.requiredItemName}\n" +
                        $"Tiến độ: {quest.currentItemCount}/{quest.requiredItemCount}"
                    );
                }
            }
            return;
        }

        dialogueIndex = 0;
        isPostQuestTalking = true;
    }

    // =====================
    // POST QUEST DIALOGUE
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
            ShowTyping("🎉 Bạn đã hoàn thành nhiệm vụ tiêu diệt quái!");
            SkillManager.Instance.UnlockCircleSlash();
        }
        else if (quest.questType == QuestType.CollectItem)
        {
            ShowTyping("🎉 Bạn đã giao đủ vật phẩm!\nCảm ơn bạn!");
        }

        isPostQuestTalking = false;
    }
}
