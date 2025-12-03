using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public string questName;
    public int requiredAmount = 5;
    private int currentAmount = 0;

    public void CollectItem()
    {
        currentAmount++;
        Debug.Log($"Đã thu thập {currentAmount}/{requiredAmount} vật phẩm cho {questName}");

        if (currentAmount >= requiredAmount)
        {
            Quest quest = FindQuest();
            if (quest != null)
            {
                QuestManager.Instance.CompleteQuest(quest);
            }
        }
    }

    private Quest FindQuest()
    {
        foreach (var q in FindObjectsOfType<QuestGiver>())
        {
            if (q.quest.questName == questName)
                return q.quest;
        }
        return null;
    }
}
