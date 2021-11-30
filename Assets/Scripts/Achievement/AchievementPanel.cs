using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AchievementPanel : MonoBehaviour
{
    [SerializeField] Text textTitle = null;
    [SerializeField] Text textExplanational = null;
    [SerializeField] Image CheckImage = null;
    [SerializeField] Image clearImage = null;

    Image image = null;

    public void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetValue(string title, string explanation)
    {
        textTitle.text = string.Format(title);
        textExplanational.text = string.Format(explanation);
    }
    public void Clear(int ID)
    {
        CheckImage.gameObject.SetActive(true);
        clearImage.gameObject.SetActive(true);
        transform.SetSiblingIndex(transform.GetSiblingIndex() + transform.parent.childCount);
        Debug.Log(transform.GetSiblingIndex());
    }
}
