using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Khoảng cách tối đa để tương tác")]
    public float interactionDistance = 3f;

    [TextArea]
    public string interactionText = "Nhấn [E] để tương tác";

    // Gọi khi người chơi nhấn phím E
    public virtual void Interact()
    {
        Debug.Log("Đã tương tác với " + gameObject.name);
    }
}
