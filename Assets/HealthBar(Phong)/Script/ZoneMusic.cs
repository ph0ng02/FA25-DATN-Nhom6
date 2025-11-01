using UnityEngine;

public class ZoneMusic : MonoBehaviour
{
    public AudioSource musicSource; // Gắn AudioSource của khu vực này

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            musicSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            musicSource.Stop();
        }
    }
}


