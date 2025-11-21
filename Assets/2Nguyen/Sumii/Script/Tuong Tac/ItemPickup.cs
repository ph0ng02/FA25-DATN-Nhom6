using UnityEngine;

public class ItemPickup : Interactable
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Đã nhặt vật phẩm!");
        Destroy(gameObject);
    }
}
