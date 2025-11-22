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
        if (currentQuest != null)
        {
            currentQuest.AddKill();
        }
    }
}
