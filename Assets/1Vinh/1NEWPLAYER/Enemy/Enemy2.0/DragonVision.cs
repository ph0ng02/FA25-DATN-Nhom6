using UnityEngine;

public class DragonVision : MonoBehaviour
{
    private NightmareDragon dragon;

    private void Awake()
    {
        dragon = GetComponentInParent<NightmareDragon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dragon.PlayerDetected(other.transform);
            Debug.Log("👀 Dragon phát hiện Player!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dragon.LosePlayer();
            Debug.Log("❌ Player rời khỏi vùng tầm nhìn");
        }
    }
}
