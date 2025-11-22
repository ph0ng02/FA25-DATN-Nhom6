using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("Goal tiles của Player")]
    public GoalTile[] playerGoals;   // 👉 CHỈ 1 PLAYER

    public Door door;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckPuzzle()
    {
        if (AllGoalsCorrect(playerGoals))
        {
            Debug.Log("Puzzle Completed!");
            door.OpenDoor();
        }
    }

    bool AllGoalsCorrect(GoalTile[] goals)
    {
        foreach (var g in goals)
        {
            if (!g.isFilledCorrectly)
                return false;
        }
        return true;
    }
}
