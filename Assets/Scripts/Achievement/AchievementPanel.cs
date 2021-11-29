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

    public void SetValue(string title, string explanation)
    {
        textTitle.text = string.Format(title);
        textExplanational.text = string.Format(explanation);
    }
    public void Clear(int ID)
    {
        clearScreen.SetActive(true);
        transform.SetSiblingIndex(transform.GetSiblingIndex() + transform.parent.childCount);
        Debug.Log(transform.GetSiblingIndex());
    }
}
