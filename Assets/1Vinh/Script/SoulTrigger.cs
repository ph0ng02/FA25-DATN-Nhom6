using UnityEngine;

public class SoulTrigger : MonoBehaviour
{
    public SoulPet soulPet;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            soulPet.StartFollow(other.transform);
            gameObject.SetActive(false); // tắt trigger sau khi nhặt
        }
    }
}
