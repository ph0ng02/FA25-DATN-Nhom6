using UnityEngine;

public class GoalTile : MonoBehaviour
{
    public int goalID = 0;
    public bool isFilledCorrectly = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            var rock = other.GetComponent<PushableRock>();

            if (rock != null && rock.isCorrectRock && rock.correctGoalID == goalID)
            {
                isFilledCorrectly = true;
            }
            else
            {
                isFilledCorrectly = false;
            }

            PuzzleManager.Instance.CheckPuzzle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            isFilledCorrectly = false;
            PuzzleManager.Instance.CheckPuzzle();
        }
    }
}
