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

    // ---------- KILL QUEST ----------
    public int requiredKillCount = 1;
    public int currentKillCount = 0;

    // ---------- COLLECT QUEST ----------
    public string requiredItemName;
    public int requiredItemCount = 1;
    public int currentItemCount = 0;

    public GameObject portalToActivate;

    // ===============================
    // ADD KILL
    // ===============================
    public void AddKill()
    {
        if (questType != QuestType.Kill) return;
        if (!isAccepted || isCompleted) return;

        currentKillCount++;

        if (currentKillCount >= requiredKillCount)
            CompleteQuest();
    }

    // ===============================
    // COLLECT ITEM
    // ===============================
    public void CollectItem(string itemName)
    {
        if (questType != QuestType.CollectItem) return;
        if (!isAccepted || isCompleted) return;

        if (itemName != requiredItemName) return;

        currentItemCount++;

        Debug.Log($"📦 Nhặt item: {currentItemCount}/{requiredItemCount}");

        if (currentItemCount >= requiredItemCount)
            CompleteQuest();
    }

    // ===============================
    // COMPLETE QUEST
    // ===============================
    void CompleteQuest()
    {
        isCompleted = true;
        Debug.Log("🎉 QUEST COMPLETED!");

        if (portalToActivate != null)
            portalToActivate.SetActive(true);
    }
}
