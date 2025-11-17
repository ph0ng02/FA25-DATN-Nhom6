using UnityEngine;

public class AnimBoss : MonoBehaviour
{
    public Animator c;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        c.SetBool("Attack", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
