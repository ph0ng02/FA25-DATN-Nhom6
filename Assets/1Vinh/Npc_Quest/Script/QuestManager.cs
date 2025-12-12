using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public Quest currentQuest;

    private void Awake()
    {
        Instance = this;
    }

    public void AddKill()
    {
        if (currentQuest == null) return;
        if (currentQuest.questType != QuestType.Kill) return;

        currentQuest.currentKillCount++;

        Debug.Log("Kill added to quest! " + currentQuest.currentKillCount + "/" + currentQuest.requiredKillCount);

        currentQuest.CheckComplete();
    }

    public void AddCollectedItem(string itemName)
    {
        if (currentQuest != null)
        {
            currentQuest.CollectItem(itemName);
        }
    }
}
