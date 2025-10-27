using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Dive");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Sit walk");
        }
    }
}
