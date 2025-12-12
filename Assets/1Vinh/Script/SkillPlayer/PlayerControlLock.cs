using UnityEngine;

public class PlayerControlLock : MonoBehaviour
{
    public static PlayerControlLock Instance;

    public bool isLocked = false; // TRUE = Player đứng im

    private void Awake()
    {
        Instance = this;
    }
}
