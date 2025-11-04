using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    [Tooltip("Thời gian delay trước khi gây damage (tính bằng giây)")]
    public float attackDelay = 0.3f;

    [Tooltip("Có gây damage nhiều lần khi ở trong state không?")]
    public bool repeatDamage = false;

    [Tooltip("Khoảng thời gian giữa mỗi lần gây damage nếu repeatDamage = true")]
    public float repeatRate = 0.5f;

    private PlayerAttack playerAttack;
    private float nextDamageTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Lấy component PlayerAttack trên nhân vật
        playerAttack = animator.GetComponent<PlayerAttack>();

        // Gây damage 1 lần sau delay
        animator.GetComponent<MonoBehaviour>().StartCoroutine(AttackAfterDelay(animator, attackDelay));
    }

    private System.Collections.IEnumerator AttackAfterDelay(Animator animator, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerAttack != null)
        {
            playerAttack.DealDamage();
            nextDamageTime = Time.time + repeatRate;
        }

        // Nếu repeatDamage = true, tiếp tục gây damage định kỳ trong state
        while (repeatDamage && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            if (Time.time >= nextDamageTime)
            {
                playerAttack.DealDamage();
                nextDamageTime = Time.time + repeatRate;
            }
            yield return null;
        }
    }
}
