using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMana : MonoBehaviour
{
    public Slider manaSlider;
    public TMP_Text manaText; // ← Thêm dòng này

    public float maxMana = 100f;
    public float currentMana = 100f;

    void Start()
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
        UpdateManaText(); // ← Thêm dòng này
    }

    void Update()
    {
        RecoverManaOverTime();
    }

    void RecoverManaOverTime()
    {
        RecoverMana(10f * Time.deltaTime); // 10 mana mỗi giây
    }

    public void UseMana(float amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manaSlider.value = currentMana;
        UpdateManaText(); // ← Cập nhật text
    }

    public void RecoverMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manaSlider.value = currentMana;
        UpdateManaText(); // ← Cập nhật text
    }

    void UpdateManaText()
    {
        if (manaText != null)
            manaText.text = Mathf.RoundToInt(currentMana) + " / " + Mathf.RoundToInt(maxMana);
    }
}


