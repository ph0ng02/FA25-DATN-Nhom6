using UnityEngine;

public class GoalTile : MonoBehaviour
{
    public int goalID = 0;
    public bool isFilledCorrectly = false;
    public GameObject slightGlow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            var rock = other.GetComponent<PushableRock>();

            if (rock != null && rock.isCorrectRock && rock.correctGoalID == goalID)
            {
                isFilledCorrectly = true;
            }
            if (rock != null && rock.correctGoalID == goalID)
            { 
                slightGlow.SetActive(true); 
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
