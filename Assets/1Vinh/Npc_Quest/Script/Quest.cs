using UnityEngine;

public enum QuestType
{
    Kill,
    CollectItem
}

[System.Serializable]
public class Quest
{
    public string questName;
    [TextArea(3, 10)]
    public string description;

    public QuestType questType = QuestType.Kill;

    public bool isAccepted = false;
    public bool isCompleted = false;

    // ---- Kill Quest ----
    public int requiredKillCount = 1;
    public int currentKillCount = 0;

    // ---- Collect Quest ----
    public string requiredItemName;
    public bool hasCollectedItem = false;
    public GameObject portalToActivate;

    // ===========================================================
    // KILL QUEST
    // ===========================================================
    public void AddKill()
    {
        if (questType != QuestType.Kill) return;
        if (!isAccepted || isCompleted) return;

        currentKillCount++;

        if (currentKillCount >= requiredKillCount)
        {
            CompleteQuest();
        }
    }

    // ===========================================================
    // COLLECT QUEST
    // ===========================================================
    public void CollectItem(string itemName)
    {
        if (questType != QuestType.CollectItem) return;
        if (!isAccepted || isCompleted) return;

        if (itemName == requiredItemName)
        {
            hasCollectedItem = true;

            CompleteQuest();
        }
    }

    // ===========================================================
    // HOÀN THÀNH NHIỆM VỤ
    // ===========================================================
    private void CompleteQuest()
    {
        isCompleted = true;

        Debug.Log("Nhiệm vụ hoàn thành!");

        // Kích hoạt portal nếu có (dành cho Collect Item)
        if (portalToActivate != null)
            portalToActivate.SetActive(true);

        // MỞ KHÓA SKILL Ở ĐÂY
        if (!SkillManager.Instance.hasCircleSlash)
        {
            SkillManager.Instance.UnlockCircleSlash();
            Debug.Log("📌 Skill Circle Slash đã được mở khóa thông qua nhiệm vụ!");
        }
    }

    public void CheckComplete()
    {
        if (questType == QuestType.Kill)
        {
            if (currentKillCount >= requiredKillCount)
            {
                isCompleted = true;
                Debug.Log("Quest completed!");
            }
        }

        if (questType == QuestType.CollectItem)
        {
            if (hasCollectedItem)
            {
                isCompleted = true;
                Debug.Log("Quest completed!");
            }
        }
    }
}
