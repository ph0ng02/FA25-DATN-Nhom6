using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public int buttonID;        // 0, 1, 2
    public OrderPuzzle puzzle;
    public Renderer rend;

    private void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puzzle.PressButton(buttonID);
        }
    }

    public void SetColor(Color c)
    {
        rend.material.color = c;
    }
}
