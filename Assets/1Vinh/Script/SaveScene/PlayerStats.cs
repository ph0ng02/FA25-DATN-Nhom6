using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int exp = 0;

    public float maxHP = 100f;
    public float currentHP = 100f;

    void Start()
    {
        // LOAD khi vào game (ép kiểu về float nếu dữ liệu là int)
        currentHP = PlayerDataManager.Instance.data.hp;
        maxHP = PlayerDataManager.Instance.data.maxHp;
        level = PlayerDataManager.Instance.data.level;
        exp = PlayerDataManager.Instance.data.exp;
    }

    public void SaveStats()
    {
        PlayerDataManager.Instance.data.hp = (int)currentHP;      // ÉP KIỂU
        PlayerDataManager.Instance.data.maxHp = (int)maxHP;       // ÉP KIỂU
        PlayerDataManager.Instance.data.level = level;
        PlayerDataManager.Instance.data.exp = exp;
    }
}
