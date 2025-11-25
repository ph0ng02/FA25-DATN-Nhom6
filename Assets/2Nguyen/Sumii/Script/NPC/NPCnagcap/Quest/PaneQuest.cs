using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaneQuest : MonoBehaviour
{
    private bool isShown = false;
    private Vector2 initialPosition;
    private Coroutine coroutine;
    [HideInInspector] public bool isPane;
    public Transform contentParent;

    public TextMeshProUGUI questItemPrefab;
    public GameObject buttonMuiTen;
    public GameObject textTab;

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        initialPosition = rect.anchoredPosition;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPane)
        {
            buttonMuiTen.transform.Rotate(0, 0, 180);
            ShowHideQuestsPanel();
            isPane = true;
            textTab.transform.Rotate(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isPane)
        {
            buttonMuiTen.transform.Rotate(0, 0, 180);
            ShowHideQuestsPanel();
            isPane = false;
            textTab.transform.Rotate(0, 0, 180);
        }
    }

    public void ShowAllQuestItem(List<QuestItem> questItems)
    {
        for (int i = 0; i < contentParent.childCount; i++)
        {
            if (contentParent.GetChild(i).gameObject != questItemPrefab.gameObject)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }
        }

        foreach (var item in questItems)
        {
            var questItem = Instantiate(questItemPrefab, contentParent);
            questItem.text =
    $"<b><size=110%><color=#FFD700>{item.QuetsItemName}</color></size></b>\n" +
    $"<size=90%><i><color=#C0C0C0>{item.description}</color></i></size>\n" +
    $"<b><color=white>Tiến độ:</color></b> <b><color={(item.IsComplete() ? "#00FF00" : "#FF6347")}>{item.currentAmount}/{item.questTargetAmount}</color></b> " +
    $"{(item.IsComplete() ? "<b><color=#00FF00>(Hoàn thành)</color></b>" : "")}\n" +
    $"<size=90%><i><color=#87CEFA> Chỉ dẫn:</color> <color=#D3D3D3>{item.hint}</color></i></size>";

            questItem.gameObject.SetActive(true);
        }
    }

    public void ShowHideQuestsPanel()
    {
        isShown = !isShown;

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(MovingPanel(isShown));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.Tab) && !isPane)
        {
            isPane = true;
        }
    }

    IEnumerator MovingPanel(bool show)
    {
        Vector2 targetPos = show
            ? new Vector2(initialPosition.x + 320f, initialPosition.y)
            : new Vector2(initialPosition.x - 5f, initialPosition.y);

        while (Vector2.Distance(rect.anchoredPosition, targetPos) > 1f)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPos, Time.deltaTime * 5);
            yield return null;
        }

        rect.anchoredPosition = targetPos;
    }
}
