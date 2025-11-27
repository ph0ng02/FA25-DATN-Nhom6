using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NPC_Dialogue : MonoBehaviour
{
    [TextArea(3, 6)]
    public string[] dialogues;

    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    private int index = 0;
    private bool playerNear = false;
    private bool talking = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!talking)
            {
                StartDialogue();
            }
            else
            {
                NextDialogue();
            }
        }
    }

    void StartDialogue()
    {
        talking = true;
        dialogueUI.SetActive(true);
        index = 0;
        dialogueText.text = dialogues[index];
    }

    void NextDialogue()
    {
        index++;
        if (index >= dialogues.Length)
        {
            EndDialogue();
            return;
        }
        dialogueText.text = dialogues[index];
    }

    void EndDialogue()
    {
        talking = false;
        dialogueUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
