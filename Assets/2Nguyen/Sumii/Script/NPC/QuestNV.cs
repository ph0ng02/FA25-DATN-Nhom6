using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewQuestNV", menuName = "Quest System/QuestNV")]
public class QuestNV : ScriptableObject
{
    public string questName = "Tên nhiệm vụ mặc định";

    [TextArea(3, 10)]
    public string objectiveDescription = "Mục tiêu: Hoàn thành nhiệm vụ.";

    public bool isCompleted = false;
}