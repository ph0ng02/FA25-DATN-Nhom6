using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    [Header("Quest Info")]
    public Quest quest;

    [Header("UI References")]
    public GameObject questPanel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button acceptButton;

    private bool playerInRange = false;

    void Start()
    {
        questPanel.SetActive(false);
        acceptButton.onClick.AddListener(AcceptQuest);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!quest.isAccepted)
            {
                OpenQuestPanel();
            }
        }
    }

    void OpenQuestPanel()
    {
        questPanel.SetActive(true);
        titleText.text = quest.questName;
        descriptionText.text = quest.description;
    }

    void AcceptQuest()
    {
        quest.isAccepted = true;
        QuestManager.Instance.AddQuest(quest);
        questPanel.SetActive(false);
        Debug.Log("Người chơi đã nhận nhiệm vụ: " + quest.questName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            questPanel.SetActive(false);
        }
    }
}
