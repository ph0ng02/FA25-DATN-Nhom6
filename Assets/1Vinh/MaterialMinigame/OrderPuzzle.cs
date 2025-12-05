using UnityEngine;

public class OrderPuzzle : MonoBehaviour
{
    public ButtonTrigger[] buttons;
    public int[] correctOrder = { 3 , 1 , 4 , 2 }; // thứ tự bạn muốn

    private int currentIndex = 0;

    [Header("Door to destroy")]
    public GameObject doorObject;  // cửa sẽ bị phá hủy khi hoàn thành puzzle

    public void PressButton(int id)
    {
        Debug.Log("Press button: " + id);

        // Nếu đúng nút
        if (correctOrder[currentIndex] == id)
        {
            buttons[id].SetColor(Color.green);
            currentIndex++;

            // Nếu hoàn thành hết thứ tự
            if (currentIndex >= correctOrder.Length)
            {
                Debug.Log("Puzzle 1 Completed!");
                OpenDoorByDestroy();
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    void ResetPuzzle()
    {
        Debug.Log("Sai rồi! Reset...");
        currentIndex = 0;

        foreach (var b in buttons)
            b.SetColor(Color.white);
    }

    void OpenDoorByDestroy()
    {
        if (doorObject != null)
        {
            Destroy(doorObject);
        }
        else
        {
            Debug.LogWarning("doorObject chưa gán trong Inspector!");
        }
    }
}
