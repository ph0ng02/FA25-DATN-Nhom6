using NUnit.Framework.Interfaces;
using UnityEngine;

public class QuestItemPickup : MonoBehaviour
{
    public ItemData brokenAmuletItem; // ScriptableObject của Mảnh Bùa Gãy

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem người chơi có chạm vào không
        if (other.CompareTag("Player"))
        {
            // 

            // Chỉ nhặt nếu nhiệm vụ đang ở trạng thái Active_FindItem
            if (QuestManagerr.NiraQuestState == QuestState.Active_FindItem)
            {
                // Thêm vật phẩm vào inventory
                ItemManager.Instance.AddItem(brokenAmuletItem, 1);

                // Cập nhật trạng thái nhiệm vụ (Không bắt buộc, nhưng tốt cho UI quest log)
                QuestManagerr.UpdateNiraQuestState(QuestState.HasItem_ReturnToNira);

                // Hiển thị thông báo nhặt
                Debug.Log("Bạn đã nhặt được Mảnh Bùa Gãy.");

                // Xóa vật phẩm khỏi Scene
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Đây là Mảnh Bùa Gãy, nhưng bạn không biết nó dùng để làm gì.");
            }
        }
    }
}