using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "TestItem";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("🔥 TRIGGER HIT WITH: " + other.name);

        if (!other.CompareTag("Player")) return;

        Debug.Log("✅ PLAYER ENTER TRIGGER");

        if (QuestManager.Instance == null)
        {
            Debug.LogError("❌ QuestManager NULL");
            return;
        }

        if (QuestManager.Instance.currentQuest == null)
        {
            Debug.LogError("❌ currentQuest NULL");
            return;
        }

        QuestManager.Instance.AddCollectedItem(itemName);
        Destroy(gameObject);
    }
}
