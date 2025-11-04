using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Content")]
    [TextArea(2, 5)] public string[] dialogueLines;

    [Header("Settings")]
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    private int currentLine = 0;
    private bool isTalking = false;
    private Transform player;

    void Start()
    {
        // Tìm tất cả player có tag "Player1" và "Player2"
        GameObject[] players1 = GameObject.FindGameObjectsWithTag("Player1");
        GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player2");

        // Gộp cả hai mảng vào một mảng chung
        GameObject[] allPlayers = new GameObject[players1.Length + players2.Length];
        players1.CopyTo(allPlayers, 0);
        players2.CopyTo(allPlayers, players1.Length);

        if (allPlayers.Length > 0)
        {
            // Lấy player gần NPC nhất
            player = GetClosestPlayer(allPlayers).transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy Player1 hoặc Player2 trong scene!", this);
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        else
            Debug.LogError("Dialogue Panel chưa được gán trong Inspector!", this);
    }

    GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p;
            }
        }

        return closest;
    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (!isTalking)
                {
                    StartDialogue();
                }
                else
                {
                    NextLine();
                }
            }
        }
        else if (isTalking)
        {
            EndDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialoguePanel.SetActive(false);
    }
}
