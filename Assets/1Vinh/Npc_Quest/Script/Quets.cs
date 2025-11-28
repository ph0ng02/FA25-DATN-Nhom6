using UnityEngine;

public class Quets : MonoBehaviour
{
    public GameObject textPanel;

    public float triggerDistance = 3f;

    private Transform player;
    private bool isTalking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        textPanel.SetActive(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist > triggerDistance)
        {
            textPanel.SetActive(false);
            isTalking = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isTalking = !isTalking;
            textPanel.SetActive(isTalking);

            if (isTalking)
                HandleQuest();
        }
    }

    void HandleQuest()
    {
        var quest = QuestData.instance;

        if (quest.state == QuestState.NotAccepted)
        {
            Debug.Log("NPC: Hãy tiêu diệt 3 con quái vật!");
            quest.state = QuestState.Accepted;
        }
        else if (quest.state == QuestState.Accepted)
        {
            Debug.Log($"NPC: Tiến độ: {quest.killCount}/{quest.killTarget}");
        }
        else if (quest.state == QuestState.Completed)
        {
            Debug.Log("NPC: Tuyệt vời! Bạn đã hoàn thành nhiệm vụ.");
            quest.state = QuestState.Rewarded; // trả nhiệm vụ
        }
        else if (quest.state == QuestState.Rewarded)
        {
            Debug.Log("NPC: Bạn đã nhận thưởng rồi!");
        }
    }
}
