using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isCorrectDoor = false;
    public TwoDoorPuzzle puzzle;
    public Renderer rend;

    private void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();
        SetGlow(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puzzle.PickDoor(isCorrectDoor, this);
        }
    }

    public void SetGlow(bool active)
    {
        if (active)
            rend.material.color = Color.yellow;  // cửa đúng sáng
        else
            rend.material.color = Color.white;   // cửa sai tắt sáng
    }
}
