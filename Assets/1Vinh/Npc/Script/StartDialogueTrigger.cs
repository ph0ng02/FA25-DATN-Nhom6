using UnityEngine;

public class StartDialogueTrigger : MonoBehaviour
{
    [TextArea]
    public string[] dialogueLines;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            DialogueManager.Instance.StartDialogue(dialogueLines);
        }
    }
}
