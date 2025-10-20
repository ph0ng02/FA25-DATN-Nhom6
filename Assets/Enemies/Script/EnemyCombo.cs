using UnityEngine;

public class EnemyCombo : MonoBehaviour
{
    public bool IsInCombo { get; private set; }

    public int StartComboSequence(Transform player)
    {
        IsInCombo = true;
        int attackIndex = Random.Range(1, 3); // 1 hoặc 2
        Invoke(nameof(ResetCombo), 1.2f); // Sau khi đánh xong 1.2s
        return attackIndex;
    }

    void ResetCombo()
    {
        IsInCombo = false;
    }
}