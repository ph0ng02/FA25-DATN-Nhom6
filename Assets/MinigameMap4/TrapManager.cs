using UnityEngine;
using System.Collections;

public class TrapManager : MonoBehaviour
{
    public float surviveTime = 30f;
    public GameObject[] traps;
    public GameObject exitGate;

    bool started = false;

    public void StartMiniGame()
    {
        if (started) return;
        started = true;

        Debug.Log("MINI GAME START");
        

        foreach (GameObject trap in traps)
{
    trap.SetActive(true);
}

        Invoke(nameof(Win), surviveTime);
        
    }

    void Win()
    {
        foreach (GameObject trap in traps)
            trap.SetActive(false);

        exitGate.SetActive(false);
        Debug.Log("MINI GAME WIN");
    }
}
