using System.Collections.Generic;
using UnityEngine;

public class QuestMarkerManager : MonoBehaviour
{
    public GameObject markerPrefab;
    private Dictionary<QuestItem, GameObject> activeMarkers = new();

    [Header("Fireball dẫn đường")]
    public GameObject fireballPrefab;   // Prefab VFX Fireball
    private GameObject activeFireball;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void ShowMarker(QuestItem quest)
    {
        if (quest.questLocation == null || activeMarkers.ContainsKey(quest)) return;

        // spawn marker trên vị trí NPC/Quest
        Vector3 spawnPos = quest.questLocation.position;

        Collider col = quest.questLocation.GetComponentInChildren<Collider>();
        if (col != null)
        {
            spawnPos.y = col.bounds.max.y;
        }
        else
        {
            spawnPos += Vector3.up * 1.5f;
        }

        GameObject marker = Instantiate(markerPrefab, spawnPos, Quaternion.identity);
        marker.transform.SetParent(quest.questLocation);
        activeMarkers[quest] = marker;

        // 🟢 Spawn Fireball dẫn đường
        if (fireballPrefab != null && player != null)
        {
            if (activeFireball != null) Destroy(activeFireball);

            // Spawn ngay tại player
            activeFireball = Instantiate(fireballPrefab, player.position, Quaternion.identity);

            // Gắn script FireballMover (đã chỉnh sửa ở trên)
            FireballMover mover = activeFireball.GetComponent<FireballMover>();
            if (mover == null) mover = activeFireball.AddComponent<FireballMover>();

            mover.questTarget = quest.questLocation; // mục tiêu cuối cùng (NPC hoặc location)
        }
    }

    public void HideMarker(QuestItem quest)
    {
        if (activeMarkers.ContainsKey(quest))
        {
            Destroy(activeMarkers[quest]);
            activeMarkers.Remove(quest);
        }

        if (activeFireball != null)
        {
            Destroy(activeFireball);
            activeFireball = null;
        }
    }

    public void HideAll()
    {
        foreach (var marker in activeMarkers.Values)
        {
            Destroy(marker);
        }
        activeMarkers.Clear();

        if (activeFireball != null)
        {
            Destroy(activeFireball);
            activeFireball = null;
        }
    }


    public void HideMarkerByTarget(Transform target)
    {
        QuestItem keyToRemove = null;

        foreach (var kvp in activeMarkers)
        {
            if (kvp.Value != null && kvp.Value.transform.parent == target)
            {
                Destroy(kvp.Value);
                keyToRemove = kvp.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            activeMarkers.Remove(keyToRemove);
        }

        // Tắt luôn Fireball dẫn đường
        if (activeFireball != null)
        {
            Destroy(activeFireball);
            activeFireball = null;
        }
    }
}
