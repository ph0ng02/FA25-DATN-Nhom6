using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShowAnimation : MonoBehaviour
{
    public GameObject[] AinObjs;
    private int CurAinObjCount = 0;

    private Animation ain;
    public AnimationClip[] clips;  // Dùng khi build (kéo tay)
    public int CurAnimClip = 0;
    public string CurAnimName;

    void Start()
    {
        AddAnim();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 210, 90, 50), "Next Anim"))
        {
            CurAnimClip++;
            if (CurAnimClip > clips.Length - 1)
                CurAnimClip = 0;

            PlayAnim();
        }

        if (GUI.Button(new Rect(10, 260, 90, 50), "Prev Anim"))
        {
            CurAnimClip--;
            if (CurAnimClip < 0)
                CurAnimClip = clips.Length - 1;

            PlayAnim();
        }

        if (GUI.Button(new Rect(10, 310, 90, 50), "Replay"))
        {
            PlayAnim();
        }

        if (clips != null && clips.Length > 0)
            GUI.Label(new Rect(10, 10, 200, 20), clips[CurAnimClip].name);
    }

    public GameObject[] Chrs;
    public int i = 0;

    void ChooseChar()
    {
        CurAinObjCount = i;
        AinObjs[CurAinObjCount].SetActive(false);

        i++;
        if (i == AinObjs.Length) i = 0;

        AinObjs[i].SetActive(true);
        AddAnim();
    }

    void AddAnim()
    {
        ain = AinObjs[i].GetComponent<Animation>();

#if UNITY_EDITOR
        // Lấy animation clips tự động (chỉ trong Editor)
        clips = AnimationUtility.GetAnimationClips(ain);
#endif

        // Nếu không ở Editor, clips phải được gán bằng tay trong Inspector
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("⚠ Animation clips not assigned! (Build mode)");
        }
    }

    void PlayAnim()
    {
        if (clips == null || clips.Length == 0) return;

        AinObjs[i].GetComponent<Animation>().Play(clips[CurAnimClip].name);
        CurAnimName = clips[CurAnimClip].name;
    }
}
