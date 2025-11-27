using System.Collections;
using UnityEngine;
using TMPro;

// Merged and fixed version of NPCDialogueNV2
// - Keeps original Start/Update/NextLine flow
// - Adds: static activeNPC to prevent multiple NPCs handling input at once
// - Adds: autoLineDelay coroutine (optional)
// - Fixes: TriggerQuest logic to actually call QuestManager.StartQuestFromNPC
// - Public API: StartDialoguePublic(), Continue(), IsActive()

public class NPCDialogueNV2 : MonoBehaviour
{
    [Header("Dialogue")]
    public string npcName = "NPC";
    [TextArea(3, 10)] public string[] dialogueLines;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    [Header("Quest")]
    public QuestNV questToGive;            // ScriptableObject quest (can be null)
    public QuestManagerNV questManager;    // optional: assign in inspector to avoid Find
    private bool hasGivenQuest = false;
    public bool showQuestAfterDialogue = true;

    [Header("Auto Advance (optional)")]
    [Tooltip("0 = disabled. >0 = auto advance after seconds.")]
    public float autoLineDelay = 0f;

    // State
    private int currentLine = 0;
    private bool isPlayerInside = false;
    private bool isTalking = false;

    // Prevent multiple NPCs receiving E at same time
    private static NPCDialogueNV2 activeNPC = null;

    // Auto coroutine
    private Coroutine autoCoroutine;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        // Try find QuestManager early if not assigned
        if (questManager == null)
        {
#if UNITY_2023_2_OR_NEWER
            questManager = UnityEngine.Object.FindAnyObjectByType<QuestManagerNV>();
#else
            questManager = FindObjectOfType<QuestManagerNV>();
#endif
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (isTalking)
                EndDialogue();
        }
    }

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            // If no active NPC, start this one
            if (activeNPC == null)
            {
                StartDialogue();
            }
            else if (activeNPC == this)
            {
                // Continue only if this NPC is the active one
                Continue();
            }
            // if another NPC is active, ignore input
        }
    }

    // --- Public API (for UI buttons or external callers) ---
    public void StartDialoguePublic()
    {
        // wrapper so other systems can open dialogue
        StartDialogue();
    }

    public void Continue()
    {
        // Called either by keyboard or UI button
        // Stop any running auto coroutine when player manually continues
        if (autoCoroutine != null)
        {
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }

        NextLine();
    }

    public bool IsActive()
    {
        return dialogueUI != null && dialogueUI.activeSelf;
    }

    // --- Internal flow ---
    void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        // If another NPC active, ignore start
        if (activeNPC != null && activeNPC != this) return;

        activeNPC = this;
        isTalking = true;
        currentLine = 0;

        if (dialogueUI != null)
            dialogueUI.SetActive(true);

        if (dialogueText != null && dialogueLines.Length > 0)
            dialogueText.text = dialogueLines[currentLine];

        // start auto-advance if enabled
        if (autoLineDelay > 0f)
        {
            if (autoCoroutine != null) StopCoroutine(autoCoroutine);
            autoCoroutine = StartCoroutine(AutoNext());
        }
    }

    public void NextLine()
    {
        if (!isTalking) return;

        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        if (dialogueText != null)
            dialogueText.text = dialogueLines[currentLine];

        // restart auto-advance
        if (autoLineDelay > 0f)
        {
            if (autoCoroutine != null) StopCoroutine(autoCoroutine);
            autoCoroutine = StartCoroutine(AutoNext());
        }
    }

    void EndDialogue()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        isTalking = false;
        currentLine = 0;

        // release active NPC
        if (activeNPC == this)
            activeNPC = null;

        // Stop auto coroutine if any
        if (autoCoroutine != null)
        {
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }

        // Trigger quest logic (fixed to actually call QuestManager)
        TriggerQuest();
    }

    private IEnumerator AutoNext()
    {
        yield return new WaitForSecondsRealtime(autoLineDelay);
        autoCoroutine = null;
        // If still talking and this is the active NPC, advance
        if (isTalking && activeNPC == this)
            NextLine();
    }

    void TriggerQuest()
    {
        if (!showQuestAfterDialogue) return;
        if (questToGive == null) return;
        if (hasGivenQuest) return;

        // Lazy find questManager if not assigned
        if (questManager == null)
        {
#if UNITY_2023_2_OR_NEWER
            questManager = UnityEngine.Object.FindAnyObjectByType<QuestManagerNV>();
#else
            questManager = FindObjectOfType<QuestManagerNV>();
#endif
        }

        if (questManager == null)
        {
            Debug.LogWarning("[NPCDialogueNV2] QuestManagerNV not found in scene. Assign it in the Inspector to avoid Find calls.");
            return;
        }

        // Call the QuestManager to start the quest (this shows panel / updates manager)
        questManager.StartQuestFromNPC(questToGive);
        hasGivenQuest = true;
        Debug.Log($"[NPCDialogueNV2] Quest '{questToGive.questName}' started by NPC '{npcName}'.");
    }

    // Optional: allow external reset (if you want NPC to be able to give again)
    public void ResetGivenQuestFlag()
    {
        hasGivenQuest = false;
    }
}
