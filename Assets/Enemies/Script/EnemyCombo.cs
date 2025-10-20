using UnityEngine;
using System.Collections;

public class EnemyCombo : MonoBehaviour
{
    [System.Serializable]
    public class AttackData
    {
        public float windUp = 0.2f;
        public float activeTime = 0.3f;
        public float recovery = 0.4f;
        public float damage = 10f;
    }

    public AttackData[] comboAttacks =
    {
        new AttackData { windUp = 0.2f, activeTime = 0.3f, recovery = 0.4f, damage = 8f },
        new AttackData { windUp = 0.25f, activeTime = 0.35f, recovery = 0.4f, damage = 10f }
    };

    public bool IsInCombo { get; private set; }

    private Animator anim;

    void Awake() => anim = GetComponent<Animator>();

    public int StartComboSequence(Transform player)
    {
        if (!IsInCombo)
        {
            StartCoroutine(ComboFlow(player));
            return Random.Range(1, 3); // Attack1 hoặc Attack2
        }
        return 0;
    }

    IEnumerator ComboFlow(Transform player)
    {
        IsInCombo = true;

        foreach (var attack in comboAttacks)
        {
            yield return new WaitForSeconds(attack.windUp + attack.activeTime + attack.recovery);
        }

        IsInCombo = false;
    }
}