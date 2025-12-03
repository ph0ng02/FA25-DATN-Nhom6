using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public GoalTile[] player1Goals;
    public GoalTile[] player2Goals;

    public Door door;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckPuzzle()
    {
        if (AllGoalsCorrect(player1Goals) && AllGoalsCorrect(player2Goals))
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
