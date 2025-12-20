using UnityEngine;

public class GoalTile : MonoBehaviour
{
    [Header("Goal Settings")]
    public int goalID;
    public bool isFilledCorrectly;

    [Header("Color Settings")]
    public Color normalColor = Color.white;
    public Color correctColor = Color.green;

    private Renderer rend;
    private PushableRock currentRock;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = normalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Pushable")) return;

        PushableRock rock = other.GetComponent<PushableRock>();
        if (rock == null) return;

        currentRock = rock;

        if (rock.isCorrectRock && rock.correctGoalID == goalID)
        {
            isFilledCorrectly = true;
            SetColor(correctColor);
        }
        else
        {
            isFilledCorrectly = false;
            SetColor(normalColor);
        }

        PuzzleManager.Instance.CheckPuzzle();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Pushable")) return;

        PushableRock rock = other.GetComponent<PushableRock>();

        if (rock == currentRock)
        {
            currentRock = null;
            isFilledCorrectly = false;
            SetColor(normalColor);

            PuzzleManager.Instance.CheckPuzzle();
        }
    }

    void SetColor(Color color)
    {
        rend.material.color = color;
    }
}
