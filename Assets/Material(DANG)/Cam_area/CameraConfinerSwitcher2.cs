using UnityEngine;
using Unity.Cinemachine; // nếu Unity 6

public class CameraConfinerSwitcher2 : MonoBehaviour
{
    public CinemachineConfiner3D confiner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConfinerArea"))
        {
            confiner.BoundingVolume = other;
            Debug.Log("Player1 đổi vùng camera sang: " + other.name);
        }
    }
}
