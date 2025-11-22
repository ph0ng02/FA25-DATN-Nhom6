using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public string npcName = "NPC";
    [TextArea(3, 10)]
    public string[] dialogueLines;   // Các câu hội thoại

    public GameObject dialogueUI;
    public TMPro.TextMeshProUGUI dialogueText;

    private int currentLine = 0;
    private bool isPlayerInside = false;
    private bool isTalking = false;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player entered NPC zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            EndDialogue();
        }
    }

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
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

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;
        dialogueUI.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueLines[currentLine];
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isTalking = false;
        currentLine = 0;
    }
}
