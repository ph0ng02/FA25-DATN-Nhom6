using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public Quest currentQuest;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // =========================
    // KILL QUEST
    // =========================
    public void AddKill()
    {
        if (currentQuest == null) return;
        if (currentQuest.questType != QuestType.Kill) return;

        currentQuest.AddKill();

        Debug.Log($"☠ Kill: {currentQuest.currentKillCount}/{currentQuest.requiredKillCount}");
    }

    // =========================
    // COLLECT ITEM QUEST
    // =========================
    public void AddCollectedItem(string itemName)
    {
        if (currentQuest == null)
        {
            Debug.LogWarning("❌ Không có quest nào đang active");
            return;
        }

        if (currentQuest.questType != QuestType.CollectItem)
        {
            Debug.LogWarning("❌ Quest hiện tại không phải Collect Quest");
            return;
        }

        Debug.Log("📦 Nhận item từ world: " + itemName);

        currentQuest.CollectItem(itemName);
    }
}
