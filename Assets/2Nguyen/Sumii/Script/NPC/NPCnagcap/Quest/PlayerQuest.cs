using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerQuest : MonoBehaviour
{
    // sử dụng cho nhiều nhiệm vụ
    public List<QuestItem> questItems = new List<QuestItem>();
    public PaneQuest playerQuestPanel;
    // Nhận nhiệm vụ 
    public GameObject winGamePanel;

    private List<System.Action> pendingRewards = new List<System.Action>();
    // chỉ dẫn nhiệm vụ
    public QuestMarkerManager markerManager;

    private void Start()
    {

        if (playerQuestPanel == null)
        {
            playerQuestPanel = FindAnyObjectByType<PaneQuest>();
        }

        if (markerManager == null)
        {
            markerManager = FindAnyObjectByType<QuestMarkerManager>();
        }

        if (winGamePanel == null)
        {
            winGamePanel = GameObject.Find("Panel Victory");
            if (winGamePanel == null)
                Debug.LogWarning("⚠ Không tìm thấy Panel Victory trong scene!");
        }

        if (winGamePanel != null)
        {
            winGamePanel.SetActive(false);
        }
    }
    public void TakeQuest(QuestItem questItem)
    {

        var check = questItems
                    .FirstOrDefault(x => x.QuetsItemName ==
                                questItem.QuetsItemName);

        if (check == null)
            questItems.Add(questItem);

        if (questItem.questLocation != null)
        {
            markerManager.ShowMarker(questItem);
        }

        playerQuestPanel.ShowAllQuestItem(questItems);
    }

    // Cập nhật tiến trình nhiệm vụ
    public void UpdateQuest(string tag)
    {

        foreach (var quest in questItems)
        {
            if (quest.TargetItemtag == tag && !quest.IsComplete())
            {
                quest.UpdateQuestProgress();
                Debug.Log($"Tiến trình nhiệm vụ {quest.QuetsItemName}: {quest.currentAmount}/{quest.questTargetAmount}");

                playerQuestPanel.ShowAllQuestItem(questItems);

                if (quest.IsComplete())
                {
                    Debug.Log($"Hoàn thành nhiệm vụ: {quest.QuetsItemName}!");

                    if (quest.questLocation != null)
                        markerManager.HideMarker(quest);


                    if (quest.questGiverLocation != null)
                    {
                        quest.questLocation = quest.questGiverLocation;
                        markerManager.ShowMarker(quest);
                    }
                }
            }
        }
    }


    public bool HasCompletedQuest(QuestItem questItem)
    {
        return questItems.Contains(questItem) && questItem.IsComplete();
    }


    public void CompleteQuest(QuestItem questItem)
    {
        if (HasCompletedQuest(questItem))
        {
            questItems.Remove(questItem);

            if (questItem.questLocation != null)
            {
                markerManager.HideMarker(questItem);
            }

            Debug.Log($"Đã trả nhiệm vụ: {questItem.QuetsItemName}, nhận {questItem.rewardAmount} vàng");

            // 🟢 Thay vì hiện ngay, ta LƯU lại hành động hiển thị thưởng
            pendingRewards.Add(() => {
                FindAnyObjectByType<Cor>().IncreaseCor(questItem.rewardAmount);
                Item coinItem = Resources.Load<Item>("Items/GoldItem");
                if (coinItem != null)
                    PickupMessenger.Instance.ShowPickupMessage(coinItem, questItem.rewardAmount);
            });

            foreach (var item in questItem.rewardItems)
            {
                int amount = item.isCurrency ? questItem.rewardAmount : 1;
                InventoryManager.Instance.Add(item);

                pendingRewards.Add(() => {
                    PickupMessenger.Instance.ShowPickupMessage(item, amount);
                });
            }

            foreach (var ingr in questItem.rewardIngredients)
            {
                InventoryManager.Instance.AddIngredients(ingr, questItem.rewardIngredientCount);

                pendingRewards.Add(() => {
                    PickupMessenger.Instance.ShowPickupIngredientMessage(ingr, questItem.rewardIngredientCount);
                });
            }

            playerQuestPanel.ShowAllQuestItem(questItems);
        }
    }

    public void ShowPendingRewards()
    {
        foreach (var rewardAction in pendingRewards)
        {
            rewardAction.Invoke();
        }
        pendingRewards.Clear();
    }

    public void ShowWinGame()
    {
        if (winGamePanel == null) return;

        StartCoroutine(ShowWinGameWithDelay(4f)); // chờ 1.5 giây sau khi NPC panel tắt
    }

    private IEnumerator ShowWinGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        winGamePanel.SetActive(true);

        // Khóa điều khiển
        var playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.canMove = false;
            playerController.isTalkingWithNPC = true;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

}
