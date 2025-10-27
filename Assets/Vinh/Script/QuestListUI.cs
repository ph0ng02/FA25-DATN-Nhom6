using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    public TMP_Text questListText;

    void Update()
    {
        questListText.text = "";
        foreach (var quest in FindObjectOfType<QuestManager>().GetType()
                 .GetField("activeQuests", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                 .GetValue(QuestManager.Instance) as List<Quest>)
        {
            questListText.text += $"{quest.questName} - {(quest.isCompleted ? "Hoàn thành" : "Đang làm")}\n";
        }
    }
}
