using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("UI")]
    public RectTransform arrowUI;   // kéo Image (RectTransform) vào đây
    public Canvas canvas;           // kéo Canvas vào (nếu để trống sẽ tự tìm)
    public Camera mainCamera;       // nếu null -> Camera.main

    [Header("Settings")]
    public float edgeBuffer = 40f;          // khoảng cách cách rìa
    public bool hideWhenOnScreen = true;    // ẩn arrow nếu mục tiêu nằm trong màn hình
    public float showEvenIfOnScreenDistance = 30f; // nếu mục tiêu trong view nhưng xa hơn giá trị này vẫn hiển thị arrow

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (canvas == null && arrowUI != null) canvas = arrowUI.GetComponentInParent<Canvas>();
        if (arrowUI == null) Debug.LogError("ArrowPointer: arrowUI chưa gán!");
        if (canvas == null) Debug.LogWarning("ArrowPointer: Canvas không tìm thấy. Nếu Canvas là ScreenSpace-Overlay thì sẽ dùng null camera conversion.");
    }

    void Update()
    {
        if (target == null || arrowUI == null || mainCamera == null)
        {
            if (arrowUI != null) arrowUI.gameObject.SetActive(false);
            return;
        }

        Vector3 worldPos = target.position;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        bool isBehind = screenPos.z < 0f;

        // nếu phía sau camera, flip tọa độ để định hướng đúng (gần đúng)
        if (isBehind)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        // kiểm tra mục tiêu có nằm trong màn hình (với buffer)
        bool inScreenBounds = screenPos.x > edgeBuffer && screenPos.x < (Screen.width - edgeBuffer)
                              && screenPos.y > edgeBuffer && screenPos.y < (Screen.height - edgeBuffer) && !isBehind;

        float distance = Vector3.Distance(mainCamera.transform.position, worldPos);

        // quyết định có hiển thị arrow hay ẩn

        arrowUI.gameObject.SetActive(true);


        // Tính hướng từ tâm màn hình tới target
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 fromCenter = ((Vector2)screenPos - screenCenter).normalized;

        // Tìm scale để clamp trên hình chữ nhật (rìa màn hình)
        float maxX = Screen.width / 2f - edgeBuffer;
        float maxY = Screen.height / 2f - edgeBuffer;

        // tránh chia cho 0
        float scaleX = fromCenter.x == 0 ? float.MaxValue : Mathf.Abs(maxX / fromCenter.x);
        float scaleY = fromCenter.y == 0 ? float.MaxValue : Mathf.Abs(maxY / fromCenter.y);
        float scale = Mathf.Min(scaleX, scaleY);

        Vector2 screenPosClamped = screenCenter + fromCenter * scale;

        // Chuyển sang local pos của Canvas
        RectTransform canvasRect = canvas != null ? canvas.GetComponent<RectTransform>() : null;
        Vector2 localPoint;
        if (canvasRect != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosClamped,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out localPoint);
            arrowUI.anchoredPosition = localPoint;
        }
        else
        {
            // nếu canvas là null (Overlay không có canvas ref), đặt world UI position
            arrowUI.position = screenPosClamped;
        }

        // Xoay mũi tên theo hướng
        float angle = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;
        arrowUI.localEulerAngles = new Vector3(0, 0, angle - 90f);
    }

    // tiện ích để set target runtime
    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void ClearTarget()
    {
        target = null;
        if (arrowUI != null) arrowUI.gameObject.SetActive(false);
    }
}
