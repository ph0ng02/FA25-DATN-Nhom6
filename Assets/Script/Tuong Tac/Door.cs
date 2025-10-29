using UnityEngine;

public class Door : Interactable
{
    private bool isOpen = false;
    public Transform doorHinge;

    public override void Interact()
    {
        base.Interact();
        isOpen = !isOpen;
        doorHinge.localRotation = Quaternion.Euler(0, isOpen ? 90 : 0, 0);
    }
}
