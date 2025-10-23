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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dialoguePanel.SetActive(false);
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
