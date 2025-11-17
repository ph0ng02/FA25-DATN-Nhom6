using UnityEngine;
using UnityEngine.Events;

public class GoalTile : MonoBehaviour
{
    public UnityEvent OnPuzzleSolved;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            Debug.Log("Puzzle Completed!");
            OnPuzzleSolved.Invoke();
        }
    }
}
