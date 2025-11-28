using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestPanelController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panelRoot;          // QuestPanel
    public TMP_Text titleText;            // Quest title
    public TMP_Text descriptionText;      // Quest description
    public TMP_Text objectiveLabelText;   // “Nhiệm vụ:” (optional)

    public Button acceptButton;
    public Button closeButton;

    public System.Action onAccepted;
    public System.Action onClosed;

    void Awake()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        if (acceptButton != null)
            acceptButton.onClick.AddListener(OnAcceptPressed);

        if (closeButton != null)
            closeButton.onClick.AddListener(OnClosePressed);
    }

    public void Show(QuestNV quest, bool showAccept = true)
    {
        if (panelRoot == null) return;

        panelRoot.SetActive(true);

        if (titleText != null)
            titleText.text = $"Quest: {quest.questName}";

        if (descriptionText != null)
            descriptionText.text = quest.objectiveDescription;

        if (objectiveLabelText != null)
            objectiveLabelText.text = "Nhiệm vụ:";

        if (acceptButton != null)
            acceptButton.gameObject.SetActive(showAccept);
    }

    public void Hide()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    void OnAcceptPressed()
    {
        onAccepted?.Invoke();
        Hide();
    }

    void OnClosePressed()
    {
        onClosed?.Invoke();
        Hide();
    }
    public void AcceptQuestButton()
    {
        OnAcceptPressed();
    }

    public void CloseQuestButton()
    {
        OnClosePressed();
    }
}
