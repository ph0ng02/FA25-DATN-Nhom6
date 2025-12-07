using UnityEngine;

public class ChestMons : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>(); 
    }

    public void Move(bool isMoving)
    {
        anim.SetBool("IsRunning", isMoving); 
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void TriggerDie()
    {
        anim.SetTrigger("Die");
    }
}