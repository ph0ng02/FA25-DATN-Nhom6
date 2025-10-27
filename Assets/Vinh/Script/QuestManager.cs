using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            Debug.Log("Đã thêm nhiệm vụ mới: " + quest.questName);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            quest.isCompleted = true;
            Debug.Log("Đã hoàn thành nhiệm vụ: " + quest.questName);
        }
    }

    public bool IsQuestActive(string questName)
    {
        return activeQuests.Exists(q => q.questName == questName);
    }

    public bool IsQuestCompleted(string questName)
    {
        return completedQuests.Exists(q => q.questName == questName);
    }
}
