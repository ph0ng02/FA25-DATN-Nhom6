using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    // Dùng Dictionary để mô phỏng Inventory: Vật phẩm -> Số lượng
    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(ItemData item, int count)
    {
        if (inventory.ContainsKey(item))
            inventory[item] += count;
        else
            inventory.Add(item, count);
    }

    public void RemoveItem(ItemData item, int count)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= count;
            if (inventory[item] <= 0)
                inventory.Remove(item);
        }
    }

    public bool HasItem(ItemData item, int requiredCount)
    {
        return inventory.ContainsKey(item) && inventory[item] >= requiredCount;
    }
}