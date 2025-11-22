using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;

    public GameObject panel;
    public TextMeshProUGUI questText;
    public Button acceptButton;

    private Quest currentQuest;

    private void Awake()
    {
        Instance = this;

        if (panel != null)
            panel.SetActive(false);

        if (acceptButton != null)
            acceptButton.onClick.AddListener(OnAcceptButtonClicked);
    }

    public void ShowQuest(Quest quest)
    {
        currentQuest = quest;
        if (panel != null)
            panel.SetActive(true);

        UpdateUI();
        if (quest.isAccepted)
            acceptButton.gameObject.SetActive(false); // ẩn nút nếu đã nhận
        else
            acceptButton.gameObject.SetActive(true);  // hiện nút nếu chưa nhận
    }

    public void UpdateUI()
    {
        if (currentQuest != null && questText != null)
        {
            if (!currentQuest.isAccepted)
                questText.text = $"Nhiệm vụ: {currentQuest.questName}\n{currentQuest.description}";
            else
                questText.text = $"Nhiệm vụ: {currentQuest.questName}\nTiến độ: {currentQuest.currentKillCount}/{currentQuest.requiredKillCount}";
        }
    }

    private void OnAcceptButtonClicked()
    {
        if (currentQuest != null)
        {
            currentQuest.isAccepted = true;
            QuestManager.Instance.currentQuest = currentQuest;
            acceptButton.gameObject.SetActive(false);
        }
    }

    public void HideQuest()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
