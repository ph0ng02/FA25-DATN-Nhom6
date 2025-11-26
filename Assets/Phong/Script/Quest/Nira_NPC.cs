using UnityEngine;
using TMPro;
using System;
// Cần tham chiếu đến NPCDialogue
// Cần sử dụng QuestManagerr, QuestState (Giả định chúng nằm trong Global Namespace)

public class Nira_NPC : MonoBehaviour
{
    // Bổ sung: Tham chiếu đến NPCDialogue
    private NPCDialogue npcDialogue;

    public ItemData brokenAmuletItem;

    [Header("Lines: Đặt vào Inspector của Nira_NPC")]
    [TextArea(3, 10)] public string[] lines_StartQuest; // Lời thoại giao nhiệm vụ
    [TextArea(3, 10)] public string[] lines_QuestActive; // Lời thoại khi đang tìm item
    [TextArea(3, 10)] public string[] lines_TurnIn; // Lời thoại hoàn thành
    [TextArea(3, 10)] public string[] lines_Completed; // Lời thoại sau khi xong

    void Start()
    {
        // Lấy NPCDialogue component
        npcDialogue = GetComponent<NPCDialogue>();
        if (npcDialogue == null)
        {
            Debug.LogError("Cần gắn NPCDialogue.cs lên cùng GameObject!", this);
            return;
        }

        // Đăng ký sự kiện: Khi đối thoại kết thúc, thực hiện hành động Quest
        npcDialogue.OnDialogueEnd += OnNiraDialogueEnd;
    }

    void OnDestroy()
    {
        if (npcDialogue != null)
        {
            npcDialogue.OnDialogueEnd -= OnNiraDialogueEnd;
        }
    }



    // Thay thế InteractWithNira() bằng hàm này để kiểm soát luồng thoại
    public void Interact()
    {
        if (npcDialogue.isTalking)
        {
            npcDialogue.DisplayNextLine();
            return;
        }

        string[] currentLines;

        switch (QuestManagerr.NiraQuestState)
        {
            case QuestState.Inactive:
                currentLines = lines_StartQuest;
                break;
            case QuestState.Active_FindItem:
                currentLines = lines_QuestActive;
                break;
            case QuestState.HasItem_ReturnToNira:
                currentLines = lines_TurnIn;
                break;
            case QuestState.Completed:
                currentLines = lines_Completed;
                break;
            default:
                currentLines = lines_Completed;
                break;
        }

        if (currentLines.Length > 0)
        {
            npcDialogue.dialogueLines = currentLines;
            npcDialogue.StartDialogue(); // gọi hàm bắt đầu hội thoại
        }
    }

    // Callback này được gọi khi đối thoại kết thúc (dòng cuối cùng)
    void OnNiraDialogueEnd()
    {
        switch (QuestManagerr.NiraQuestState)
        {
            case QuestState.Inactive:
                // Logic: Chấp nhận nhiệm vụ sau khi kết thúc lời thoại giao nhiệm vụ
                QuestManagerr.UpdateNiraQuestState(QuestState.Active_FindItem);
                Debug.Log("[NHIỆM VỤ MỚI] Đã chấp nhận nhiệm vụ Mảnh Bùa Gãy.");
                break;

            case QuestState.HasItem_ReturnToNira:
                // Logic: Hoàn thành nhiệm vụ sau khi kết thúc lời thoại hoàn thành
                CompleteQuest();
                break;

                // Các trạng thái khác không cần hành động logic sau khi đối thoại kết thúc
        }
    }

    // Giữ nguyên logic hoàn thành
    void CompleteQuest()
    {
        // Lấy vật phẩm khỏi inventory
        ItemManager.Instance.RemoveItem(brokenAmuletItem, 1);

        // Trao thưởng (Giả định PlayerStats.Instance hoạt động)
        //PlayerStats.Instance.GainExperience(50);

        QuestManagerr.UpdateNiraQuestState(QuestState.Completed);
        Debug.Log("[NHIỆM VỤ HOÀN THÀNH] Nira đã nhận lại Mảnh Bùa Gãy.");
    }

    // Loại bỏ hoàn toàn các hàm StartQuestDialogue(), CheckForCompletion(), 
    // CompletedQuestDialogue(), và AskPlayerChoice() vì chúng không cần thiết nữa.
}