using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AchievementPanel : MonoBehaviour
{
    [SerializeField] Text textTitle = null;
    [SerializeField] Text textExplanational = null;
    [SerializeField] GameObject clearScreen = null;
    [SerializeField] ClearNoticeScript clearNotice = null;

    private void Awake()
    {
        clearNotice = FindObjectOfType<ClearNoticeScript>();
    }
    public void SetValue(string title, string explanation)
    {
        textTitle.text = string.Format(title);
        textExplanational.text = string.Format(explanation);
    }
    public void Clear()
    {
        clearScreen.SetActive(true);
    }
}
