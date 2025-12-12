using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public PlayerSaveData data = new PlayerSaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // GIỮ LẠI KHI ĐỔI SCENE
        }
        else Destroy(gameObject);
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public int hp = 200;

    // Thêm maxHp bị thiếu
    public int maxHp = 200;

    public int exp = 0;
    public int level = 1;

    // Các skill
    public bool hasCircleSlash = false;
    public bool hasFireball = false;
}

