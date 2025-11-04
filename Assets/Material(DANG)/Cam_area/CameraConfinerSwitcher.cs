using UnityEngine;
using Unity.Cinemachine;

public class CameraConfinerSwitcher : MonoBehaviour
{
    public CinemachineConfiner3D confiner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConfinerArea"))
        {
            confiner.BoundingVolume = other;
            Debug.Log("Chuyển vùng camera sang: " + other.name);
        }
    }
}
