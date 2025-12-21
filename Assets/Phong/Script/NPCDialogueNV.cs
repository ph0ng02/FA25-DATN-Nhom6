using UnityEngine;
using TMPro; // Thư viện TextMeshPro
using System.Collections;

public class NPCDialogueNV : MonoBehaviour
{
    [Header("Cấu hình NPC")]
    public string npcName = "NPC"; // Tên NPC

    [Header("Nội dung hội thoại")]
    [TextArea(3, 10)] // Dòng này tạo ô nhập văn bản to, xuống dòng được như trong hình
    public string[] dialogueLines;

    [Header("UI References")]
    public GameObject dialoguePanel;       // Panel chứa khung thoại
    public TextMeshProUGUI dialogueText;   // Text hiển thị nội dung
    public TextMeshProUGUI nameText;       // Text hiển thị tên (nếu có)

    private int currentLineIndex = 0;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;

    void Start()
    {
        // Đảm bảo lúc đầu bảng thoại tắt
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // Nếu Player ở gần và bấm E -> Bắt đầu hoặc Tiếp tục
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActive)
            {
                StartDialogue();
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;

        // Hiển thị tên NPC nếu có gán biến nameText
        if (nameText != null) nameText.text = npcName;

        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            // Hiển thị dòng hiện tại
            dialogueText.text = dialogueLines[currentLineIndex];

            // Tăng chỉ số lên để lần bấm sau hiện câu tiếp theo
            currentLineIndex++;
        }
        else
        {
            // Hết thoại thì tắt
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        currentLineIndex = 0;
        Debug.Log("Kết thúc hội thoại");

        // Xử lý nhận nhiệm vụ ở đây nếu cần
        CheckForQuest();
    }

    void CheckForQuest()
    {
        // Ví dụ: Dòng cuối cùng trong hình của bạn là Giao Nhiệm Vụ
        // Bạn có thể kiểm tra nếu vừa nói xong câu cuối thì kích hoạt Quest
    }

    // Xử lý va chạm để biết Player đến gần
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Đã vào vùng NPC");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialoguePanel.SetActive(false); // Đi xa tự tắt thoại
            isDialogueActive = false;
        }
    }
}