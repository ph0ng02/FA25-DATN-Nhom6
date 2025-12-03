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
    public string requiredItemName;     // tên item cần nhặt
    public bool hasCollectedItem = false;
    public GameObject portalToActivate;

    public void AddKill()
    {
        if (questType != QuestType.Kill) return;

        if (isAccepted && !isCompleted)
        {
            currentKillCount++;
            if (currentKillCount >= requiredKillCount)
                isCompleted = true;
        }
    }

    public void CollectItem(string itemName)
    {
        if (questType != QuestType.CollectItem) return;

        if (!isAccepted || isCompleted) return;

        if (itemName == requiredItemName)
        {
            hasCollectedItem = true;
            isCompleted = true;

            // Kích hoạt portal nếu có
            if (portalToActivate != null)
                portalToActivate.SetActive(true);
        }
    }
}
