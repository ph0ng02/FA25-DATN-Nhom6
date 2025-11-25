using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static NPC;

public class NPC : MonoBehaviour
{
    // panel NPC và tự động gắn
    public GameObject npcChatPanel;
    public TextMeshProUGUI chatText;
    [HideInInspector] public bool isChating;
    Coroutine coroutine;
    public Button yesButton;
    public NpcChatSetup panelSetup;
    public PlayerController playerController;

    [Header("NPC tiếp theo")]
    public NPC nextNPC;

    public Transform npcLookTarget; // điểm mà camera sẽ nhìn khi nói chuyện
    private Transform originalCamFollow;
    private Transform originalCamLookAt;
    public CinemachineCamera dialogueCam;

    private bool questGiven = false;
    private bool isSkipping = false;
    private bool isLineFullyDisplayed = false;
    // phân loại nhiệm vụ 
    public enum NpcType
    {
        MainQuest,
        SideQuest,
        Merchant,
    }
    public NpcType npcType;
    // dựa chọn phản hồi
    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        [TextArea(2, 5)]
        public List<string> followUpLines;
    }


    // đoạn chat
    [System.Serializable]
    public class QuestDialogue
    {
        [TextArea(2, 5)]
        public List<string> lines;
    }
    // đoạn thoại lựa chọn
    [Header("Lựa chọn phản hồi")]
    public List<DialogueChoice> dialogueChoices = new();
    // đoạn thoại cốt truyện
    [Header("Đoạn thoại theo từng nhiệm vụ")]
    public List<QuestDialogue> questChats = new List<QuestDialogue>();
    // nhiệm vụ
    public List<QuestItem> questList;
    private int currentQuestIndex = 0;
    private QuestItem CurrentQuest => questList[currentQuestIndex];

    //Player
    public PlayerQuest playerQuests;

    private void Awake()
    {
        panelSetup = FindAnyObjectByType<NpcChatSetup>();
    }
    private void Start()
    {

        npcChatPanel = panelSetup.ChatPanel;
        chatText = panelSetup.ChatText.GetComponent<TextMeshProUGUI>();
        yesButton = panelSetup.YesBtn.GetComponent<Button>();

        if (dialogueCam != null)
            dialogueCam.Priority = 0;

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerQuests = other.gameObject.GetComponent<PlayerQuest>();
            playerController = other.gameObject.GetComponent<PlayerController>();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Chỉ cập nhật player nếu chưa gán
            if (playerQuests == null)
                playerQuests = other.GetComponent<PlayerQuest>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            yesButton.gameObject.SetActive(false);

            if (isChating)
            {
                StopCoroutine(coroutine); // Dừng đoạn chat 
                coroutine = null;
                isChating = false;
            }

            npcChatPanel.SetActive(false);
        }
    }


    IEnumerator ReadChat()
    {
        List<string> currentChat = (questChats != null && currentQuestIndex < questChats.Count && questChats[currentQuestIndex] != null)
            ? questChats[currentQuestIndex].lines
            : new List<string> { $"Bạn có nhiệm vụ: {CurrentQuest.QuetsItemName}" };

        playerController.canMove = false;

        foreach (var line in currentChat)
        {
            chatText.text = "";
            isSkipping = false;

            // Gõ từng ký tự
            for (int i = 0; i < line.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isSkipping = true;
                }

                if (isSkipping)
                {
                    chatText.text = line;
                    break;
                }

                chatText.text += line[i];
                yield return new WaitForSeconds(0.05f);
            }

            // Đợi click để tiếp tục sang dòng tiếp theo
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        yield return new WaitForSeconds(0.3f);

        if (dialogueChoices.Count > 0)
        {
            ShowDialogueChoices();
            yield break;
        }
        else
        {
            yesButton.gameObject.SetActive(true);
            yesButton = GameObject.FindWithTag("YesBtn").GetComponent<Button>();
            yesButton.onClick.RemoveAllListeners();

            yesButton.onClick.AddListener(() =>
            {
                if (playerQuests.HasCompletedQuest(CurrentQuest))
                {
                    QuestItem finishedQuest = CurrentQuest;
                    yesButton.gameObject.SetActive(false);
                    StartCoroutine(ShowAfterCompleteDialogue(finishedQuest));
                }
                else if (!playerQuests.questItems.Contains(CurrentQuest))
                {
                    // Nếu là nhiệm vụ cuối -> chỉ cho nhận khi tất cả nhiệm vụ khác đã hoàn thành
                    if (CurrentQuest.isFinalQuest)
                    {
                        bool hasOtherQuest = playerQuests.questItems.Any(q => !q.isFinalQuest);
                        if (hasOtherQuest)
                        {
                            chatText.text = "Bạn cần hoàn thành hết các nhiệm vụ khác trước khi nhận nhiệm vụ đặc biệt!";
                            yesButton.gameObject.SetActive(false);
                            Invoke(nameof(HidePanel), 2f);
                            return;
                        }
                    }

                    playerQuests.markerManager.HideMarkerByTarget(this.transform);

                    CurrentQuest.questGiverLocation = this.transform;
                    playerQuests.markerManager.HideMarkerByTarget(this.transform);
                    playerQuests.TakeQuest(CurrentQuest);
                    chatText.text = $"Bạn đã nhận nhiệm vụ: {CurrentQuest.QuetsItemName}";
                    questGiven = true;
                    yesButton.gameObject.SetActive(false);
                    Invoke(nameof(HidePanel), 2f);
                }
                else
                {
                    chatText.text = $"Bạn vẫn chưa hoàn thành nhiệm vụ: {CurrentQuest.QuetsItemName}";
                    yesButton.gameObject.SetActive(false);
                    Invoke(nameof(HidePanel), 2f);
                }
            });
        }

        isChating = false;


    }

    IEnumerator CompleteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        playerQuests.CompleteQuest(CurrentQuest);
        currentQuestIndex++;
        questGiven = false;

        if (currentQuestIndex >= questList.Count)
        {
            chatText.text = "Bạn đã hoàn thành tất cả nhiệm vụ rồi. Cảm ơn bạn.";
        }

        yesButton.gameObject.SetActive(false);
        Invoke(nameof(HidePanel), 2f);
    }

    IEnumerator ShowAfterCompleteDialogue(QuestItem finishedQuest)
    {
        yield return new WaitForSeconds(1f);

        string rewardsSummary = $"<color=yellow>+{finishedQuest.rewardAmount} vàng</color>";

        //if (finishedQuest.rewardItems != null && finishedQuest.rewardItems.Count > 0)
        //{
        //    var itemNames = string.Join(", ", finishedQuest.rewardItems.ConvertAll(item => item.itemName));
        //    rewardsSummary += $", <color=#00FF00>+{itemNames}</color>";
        //}

        //if (finishedQuest.rewardIngredients != null && finishedQuest.rewardIngredients.Count > 0)
        //{
        //    var ingrNames = string.Join(", ", finishedQuest.rewardIngredients.ConvertAll(i => i.ingredientName));
        //    rewardsSummary += $", <color=#87CEFA>+{finishedQuest.rewardIngredientCount}x {ingrNames}</color>";
        //}
        chatText.text = rewardsSummary;

        yield return new WaitForSeconds(2.5f);

        playerQuests.CompleteQuest(finishedQuest);

        if (finishedQuest.isFinalQuest)
        {
            // gọi hiển thị WinGame SAU khi panel NPC đóng
            StartCoroutine(ShowWinGameAfterDialogue());
        }

        currentQuestIndex++;
        questGiven = false;

        // 🟢 Nếu vẫn còn nhiệm vụ thì tự động giao luôn
        if (currentQuestIndex < questList.Count)
        {
            QuestItem nextQuest = questList[currentQuestIndex];
            nextQuest.questGiverLocation = this.transform;

            playerQuests.TakeQuest(nextQuest);
            chatText.text = $"Bạn đã nhận nhiệm vụ mới: {nextQuest.QuetsItemName}";
            yield return new WaitForSeconds(2f);
        }
        else
        {
            chatText.text = "Bạn đã hoàn thành tất cả nhiệm vụ ở đây.";
            if (nextNPC != null)
            {
                QuestItem guideQuest = new QuestItem
                {
                    QuetsItemName = $"Tìm {nextNPC.name}",
                    questTargetAmount = 1,
                    currentAmount = 0,
                    questLocation = nextNPC.transform,
                };

                playerQuests.markerManager.ShowMarker(guideQuest);
                chatText.text += $"\nHãy đến gặp {nextNPC.name} để nhận nhiệm vụ tiếp theo!";
            }

            yield return new WaitForSeconds(2f);
        }

        HidePanel();
    }
    public void ManualTrigger()
    {
        if (isChating) return;
        if (playerQuests != null && playerQuests.markerManager != null)
        {
            playerQuests.markerManager.HideMarkerByTarget(this.transform);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerController != null)
        {
            playerController.canMove = false;
            playerController.isTalkingWithNPC = true;

            if (playerController.freeLookCam != null && npcLookTarget != null)
            {
                originalCamFollow = playerController.freeLookCam.Follow;
                originalCamLookAt = playerController.freeLookCam.LookAt;

                playerController.freeLookCam.Follow = npcLookTarget;
                playerController.freeLookCam.LookAt = npcLookTarget;
            }
        }

        npcChatPanel.SetActive(true);
        if (dialogueCam != null)
        {
            dialogueCam.Priority = 20;
        }

        if (currentQuestIndex >= questList.Count)
        {
            chatText.text = "Bạn đã hoàn thành tất cả nhiệm vụ rồi. Cảm ơn bạn";
            Invoke(nameof(HidePanel), 2f);
            return;
        }

        if (playerQuests.HasCompletedQuest(CurrentQuest))
        {
            yesButton.gameObject.SetActive(false);
            QuestItem finishedQuest = CurrentQuest;
            StartCoroutine(ShowAfterCompleteDialogue(finishedQuest));
        }
        else if (!questGiven)
        {
            isChating = true;
            coroutine = StartCoroutine(ReadChat());
        }
        else
        {
            chatText.text = $"Nhiệm vụ chưa hoàn thành: {CurrentQuest.QuetsItemName}";
            Invoke(nameof(HidePanel), 2f);
        }
    }

    void ShowDialogueChoices()
    {
        for (int i = 0; i < panelSetup.choiceButtons.Count; i++)
        {
            if (i < dialogueChoices.Count)
            {
                panelSetup.choiceButtons[i].gameObject.SetActive(true);
                panelSetup.choiceTexts[i].text = dialogueChoices[i].choiceText;

                int index = i;
                panelSetup.choiceButtons[i].onClick.RemoveAllListeners();
                panelSetup.choiceButtons[i].onClick.AddListener(() =>
                {
                    HideAllChoiceButtons();
                    StartCoroutine(PlayFollowUpDialogue(dialogueChoices[index].followUpLines));
                });
            }
            else
            {
                panelSetup.choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }
    void HideAllChoiceButtons()
    {
        foreach (var btn in panelSetup.choiceButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }
    IEnumerator PlayFollowUpDialogue(List<string> lines)
    {
        foreach (var line in lines)
        {
            chatText.text = "";
            for (int i = 0; i < line.Length; i++)
            {
                chatText.text += line[i];
                yield return new WaitForSeconds(0.03f);
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }


        yesButton.gameObject.SetActive(true);
        yesButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            if (playerQuests.HasCompletedQuest(CurrentQuest))
            {
                QuestItem finishedQuest = CurrentQuest;
                yesButton.gameObject.SetActive(false);
                StartCoroutine(ShowAfterCompleteDialogue(finishedQuest));
            }
            else if (!playerQuests.questItems.Contains(CurrentQuest))
            {
                CurrentQuest.questGiverLocation = this.transform;
                playerQuests.TakeQuest(CurrentQuest);
                chatText.text = $"Bạn đã nhận nhiệm vụ: {CurrentQuest.QuetsItemName}";
                questGiven = true;
                yesButton.gameObject.SetActive(false);
                Invoke(nameof(HidePanel), 2f);
            }
            else
            {
                chatText.text = $"Bạn vẫn chưa hoàn thành nhiệm vụ: {CurrentQuest.QuetsItemName}";
                yesButton.gameObject.SetActive(false);
                Invoke(nameof(HidePanel), 2f);
            }
        });
    }

    private IEnumerator ShowWinGameAfterDialogue()
    {
        yield return new WaitForSeconds(1f);
        HidePanel();
        yield return new WaitForSeconds(0.5f);

        playerQuests.ShowWinGame();
    }

    // Nhận nhiệm vụ và đóng bảng chat
    public void HidePanel()
    {
        npcChatPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerController != null)
        {
            playerController.canMove = true;
            playerController.isTalkingWithNPC = false;
            if (dialogueCam != null)
            {
                dialogueCam.Priority = 0;
            }
            if (playerController.freeLookCam != null && originalCamFollow != null && originalCamLookAt != null)
            {
                playerController.freeLookCam.Follow = originalCamFollow;
                playerController.freeLookCam.LookAt = originalCamLookAt;
            }

            if (playerController.freeLookCam != null)
            {
                playerController.freeLookCam.Priority = 20;
            }
        }

        if (playerQuests != null)
        {
            playerQuests.ShowPendingRewards();
        }

    }
}