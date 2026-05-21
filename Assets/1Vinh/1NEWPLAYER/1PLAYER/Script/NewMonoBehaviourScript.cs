using UnityEngine;

public class CircleSlashTester : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Ấn F rồi nè -> SetTrigger(CircleSlash)");
            anim.SetTrigger("CircleSlash");
        }
    }
}
