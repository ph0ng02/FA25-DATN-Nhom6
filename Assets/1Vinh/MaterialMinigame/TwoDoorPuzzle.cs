using UnityEngine;

public class TwoDoorPuzzle : MonoBehaviour
{
    public DoorTrigger correctDoor;
    public DoorTrigger wrongDoor;

    public Animator correctDoorAnimator;

    public void PickDoor(bool isCorrect, DoorTrigger door)
    {
        if (isCorrect)
        {
            door.SetGlow(true);
            correctDoorAnimator.SetTrigger("Open");
            Debug.Log("Đúng cửa rồi!");
        }
        else
        {
            door.SetGlow(false);
            wrongDoor.SetGlow(false);
            Debug.Log("Sai cửa!");
        }
    }
}
