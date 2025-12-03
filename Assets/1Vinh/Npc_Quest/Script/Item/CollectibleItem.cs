using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.Instance.AddCollectedItem(itemName);
            Destroy(gameObject);
        }
    }
}
