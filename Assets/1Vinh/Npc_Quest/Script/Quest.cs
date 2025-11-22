using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    [TextArea(3, 10)]
    public string description;

    public bool isAccepted = false;
    public bool isCompleted = false;

    public int requiredKillCount = 3;
    public int currentKillCount = 0;

    public void AddKill()
    {
        if (isAccepted && !isCompleted)
        {
            currentKillCount++;
            if (currentKillCount >= requiredKillCount)
                isCompleted = true;
        }
    }
}
