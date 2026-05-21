using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ShamanAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public enum AIState { Idle, CastSpell }
    public AIState currentState = AIState.Idle;

    private int isCastingHash;

    [Header("Target & Cast Settings")]
    public Transform playerTarget;
    public float castRange = 8f;
    public float castTime = 1.5f;
    public float castCooldown = 3f;

    [Header("Spell Settings")]
    public GameObject spellPrefab;
    public Transform spellOrigin;

    private bool isCasting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null) agent.isStopped = true;

        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        isCastingHash = Animator.StringToHash("IsCasting");

        StartCoroutine(FSM());
    }

    void Update()
    {
        if (agent != null) agent.isStopped = true;
    }

    IEnumerator FSM()
    {
        while (true)
        {
            switch (currentState)
            {
                case AIState.Idle:
                    yield return HandleIdle();
                    break;

                case AIState.CastSpell:
                    yield return HandleCastSpell();
                    break;
            }
            yield return null;
        }
    }

    IEnumerator HandleIdle()
    {
        while (currentState == AIState.Idle)
        {
            if (playerTarget != null)
            {
                currentState = AIState.CastSpell;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator HandleCastSpell()
    {
        while (currentState == AIState.CastSpell)
        {
            if (playerTarget == null)
            {
                currentState = AIState.Idle;
                yield break;
            }

            float distance = Vector3.Distance(transform.position, playerTarget.position);

            if (distance > castRange)
            {
                currentState = AIState.Idle;
                yield break;
            }

            if (!isCasting)
            {
                isCasting = true;

                // mặt quay về người chơi
                Vector3 dir = (playerTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

                animator.SetBool(isCastingHash, true);

                // thời gian niệm phép
                yield return new WaitForSeconds(castTime);

                // bắn phép
                CastProjectile();

                animator.SetBool(isCastingHash, false);
                isCasting = false;

                yield return new WaitForSeconds(castCooldown);
            }

            yield return null;
        }
    }

    void CastProjectile()
    {
        if (spellPrefab == null || spellOrigin == null || playerTarget == null)
            return;

        GameObject spell = Instantiate(spellPrefab, spellOrigin.position, transform.rotation);

        Vector3 targetPos = playerTarget.position + Vector3.up * 1f;

        SpellProjectile sp = spell.GetComponent<SpellProjectile>();
        if (sp != null)
            sp.Fire(targetPos);

        Debug.Log("Shaman bắn phép!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = other.transform;
            currentState = AIState.CastSpell;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = null;
            currentState = AIState.Idle;
        }
    }
}
