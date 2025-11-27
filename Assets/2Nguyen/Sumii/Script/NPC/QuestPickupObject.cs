using UnityEngine;

public class QuestPickupObject : MonoBehaviour
{
    public string objectID = "ItemA";      // ID của object
    public QuestNV quest;                  // Gán đúng Quest cần hoàn thành

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Kiểm tra quest có yêu cầu nhặt đúng object hay không
            if (quest != null && quest.requiresPickupObject && quest.objectID == objectID)
            {
                quest.isCompleted = true;
                Debug.Log($"Quest hoàn thành khi nhặt: {objectID}");

                // Ẩn vật phẩm
                gameObject.SetActive(false);
            }
        }
    }
}