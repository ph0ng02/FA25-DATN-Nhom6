//using UnityEngine;
//// Đảm bảo bạn sử dụng đúng Namespace của QuestManager
//using ScriptSSS.Quests;

//// Bắt buộc phải có NPCDialogue để script này hoạt động
//[RequireComponent(typeof(NPCDialogue))]
//public class Nira_QuestGiver : MonoBehaviour
//{
//    [Header("Cấu hình Quest")]
//    public QuestSO niraQuestSO; // Kéo QuestSO Mảnh Bùa Gãy vào đây
//    public ItemData brokenAmuletItem; // Item Data của Mảnh Bùa Gãy

//    [Header("Dialogue Lines (Thiết lập trong Inspector)")]
//    [TextArea(3, 10)] public string[] dialogue_StartQuest; // Lời thoại giao nhiệm vụ
//    [TextArea(3, 10)] public string[] dialogue_QuestActive; // Lời thoại khi đang làm nhiệm vụ
//    [TextArea(3, 10)] public string[] dialogue_TurnIn; // Lời thoại khi hoàn thành nhiệm vụ

//    private NPCDialogue npcDialogue;
//    private QuestManager questManager;
//    private ItemManager itemManager; // Giả định là Singleton

//    void Start()
//    {
//        npcDialogue = GetComponent<NPCDialogue>();
//        questManager = FindObjectOfType<QuestManager>(); // Tạm thời tìm QuestManager
//        itemManager = ItemManager.Instance; // Giả định ItemManager là Singleton

//        // Quan trọng: Đăng ký sự kiện Dialogue End
//        if (npcDialogue != null)
//        {
//            npcDialogue.OnDialogueEnd += OnNiraDialogueEnd;
//        }
//        else
//        {
//            Debug.LogError("Thiếu component NPCDialogue trên Nira!", this);
//        }
//    }

//    void OnDestroy()
//    {
//        // Quan trọng: Hủy đăng ký sự kiện khi đối tượng bị hủy
//        if (npcDialogue != null)
//        {
//            npcDialogue.OnDialogueEnd -= OnNiraDialogueEnd;
//        }
//    }

//    // Public method được gọi bởi NPCDialogue khi Player nhấn E
//    public void NiraInteract()
//    {
//        if (npcDialogue.isTalking)
//        {
//            // Nếu đang nói chuyện, chỉ cần tiến lên dòng thoại tiếp theo
//            npcDialogue.Interact();
//            return;
//        }

//        // --- Xử lý logic Quest để chọn dialogue Lines ---

//        // 1. Nếu có thể hoàn thành
//        if (questManager.CanTurnIn(niraQuestSO))
//        {
//            npcDialogue.dialogueLines = dialogue_TurnIn;
//        }
//        // 2. Nếu nhiệm vụ đã được chấp nhận (và chưa hoàn thành)
//        else if (questManager.Active.ContainsKey(niraQuestSO.questId))
//        {
//            npcDialogue.dialogueLines = dialogue_QuestActive;
//        }
//        // 3. Giao nhiệm vụ
//        else
//        {
//            npcDialogue.dialogueLines = dialogue_StartQuest;
//        }

//        // Bắt đầu đối thoại với bộ Lines mới đã được chọn
//        npcDialogue.Interact();
//    }

//    // Callback được gọi TỰ ĐỘNG khi người chơi kết thúc LỜI THOẠI CUỐI CÙNG
//    private void OnNiraDialogueEnd()
//    {
//        // 1. Hoàn thành Nhiệm vụ (sau lời thoại Turn-In)
//        if (questManagerr.CanTurnIn(niraQuestSO))
//        {
//            // Kiểm tra và xóa vật phẩm khỏi inventory
//            if (itemManager != null && itemManager.HasItem(brokenAmuletItem, 1))
//            {
//                itemManager.RemoveItem(brokenAmuletItem, 1);
//                questManager.TurnIn(niraQuestSO);
//                Debug.Log($"[Quest Log] Hoàn thành: {niraQuestSO.questName}. Nhận thưởng!");
//            }
//            else
//            {
//                // Xảy ra khi có lỗi logic hoặc người chơi bán mất item
//                Debug.LogWarning("Không thể hoàn thành nhiệm vụ: Không tìm thấy vật phẩm!");
//            }
//        }
//        // 2. Chấp nhận Nhiệm vụ (sau lời thoại Start-Quest)
//        else if (!questManager.Active.ContainsKey(niraQuestSO.questId))
//        {
//            questManager.AcceptQuest(niraQuestSO);
//            questManager.RegisterAvailableQuest(niraQuestSO);
//            Debug.Log($"[Quest Log] Chấp nhận: {niraQuestSO.questName}. Bắt đầu tìm kiếm!");
//        }
//    }
//}