using UnityEngine;

public enum QuestState
{
    NotAccepted,
    Accepted,
    Completed,
    Rewarded
}

public class QuestData : MonoBehaviour
{
    public static QuestData instance;

    public QuestState state = QuestState.NotAccepted;

    public int killCount = 0;
    public int killTarget = 1;   // tiêu diệt 1 quái

    void Awake()
    {
        instance = this;
    }
}
