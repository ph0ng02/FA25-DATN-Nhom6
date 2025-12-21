using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName = "Mảnh Bùa Gãy";
    public bool isKeyItem = true;
}