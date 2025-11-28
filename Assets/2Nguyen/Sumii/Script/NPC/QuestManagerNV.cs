using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class QuestManagerNV : MonoBehaviour
{
    // Cần gán các đối tượng UI này trong Inspector
    [Header("UI")]
    public QuestPanelController questPanelController; // assign in inspector
    public GameObject questCompletePanel;             // small popup (assign)
    public float completePopupDuration = 2f;

    // call this to show a quest (e.g., when NPC gives quest)
    public void ShowQuestPanel(QuestNV quest, System.Action onAccepted = null)
    {
        if (questPanelController == null)
        {
            Debug.LogWarning("[QuestManagerNV] questPanelController not set.");
            return;
        }

        // show panel with info
        questPanelController.Show(quest, showAccept: true);

        // set callbacks
        questPanelController.onAccepted = () =>
        {
            // when accepted, possibly mark as 'active' or set state
            Debug.Log("[QuestManagerNV] Player accepted quest: " + quest.questName);

            // if you want to mark quest active, do: quest.isActive = true; (if you have that field)
            onAccepted?.Invoke();
        };

        questPanelController.onClosed = () =>
        {
            Debug.Log("[QuestManagerNV] Quest panel closed for quest: " + quest.questName);
        };
    }

    // Called by QuestPickupObject when quest completed
    public void OnQuestCompleted(QuestNV quest)
    {
        Debug.Log("[QuestManagerNV] Quest completed: " + quest.questName);
        UpdateQuestLogUI(quest); // if implemented already

        if (questCompletePanel != null)
        {
            StartCoroutine(ShowCompletePopup());
        }
    }

    IEnumerator ShowCompletePopup()
    {
        questCompletePanel.SetActive(true);
        yield return new WaitForSeconds(completePopupDuration);
        questCompletePanel.SetActive(false);
    }

    // Existing method placeholder - update quest log UI
    public void UpdateQuestLogUI(QuestNV q)
    {
        // implement your quest log list update if you have a list
        Debug.Log("[QuestManagerNV] UpdateQuestLogUI called for " + q.questName);
    }
    // Thêm vào QuestManagerNV class
    public void StartQuestFromNPC(QuestNV quest)
    {
        // Nếu bạn có hàm show panel hiện tại là ShowQuestPanel, gọi nó:
        ShowQuestPanel(quest, null);

        // version TMP thay thế, thay bằng:
        // ShowQuestPanelTMP(quest);

        Debug.Log("[QuestManagerNV] StartQuestFromNPC called for quest: " + (quest != null ? quest.questName : "null"));
    }


}
