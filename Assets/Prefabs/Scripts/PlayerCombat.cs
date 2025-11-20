using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) DoAttack(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) DoAttack(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) DoAttack(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) DoAttack(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) DoAttack(5);

        if (Input.GetKeyDown(KeyCode.Q)) DoCombo1();
        if (Input.GetKeyDown(KeyCode.E)) DoCombo2();

        if (Input.GetKeyDown(KeyCode.K)) Die();
    }

    void DoAttack(int id)
    {
        anim.SetTrigger("Attack1" + id);
        anim.SetBool("isAttacking", true);
    }

    void DoCombo1()
    {
        anim.SetTrigger("Combo1");
        anim.SetBool("isCombo1", true);
    }

    void DoCombo2()
    {
        anim.SetTrigger("Combo2");
        anim.SetBool("isCombo2", true);
    }

    void Die()
    {
        anim.SetBool("isDead", true);
    }
}
