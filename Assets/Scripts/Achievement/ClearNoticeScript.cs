using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearNoticeScript : MonoBehaviour
{
    [SerializeField] Text textClearTitle = null;
    RectTransform rectTransform = null;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ShowNotice(string title)
    {
        textClearTitle.text = string.Format("업적 \"" + title + "\" 달성");
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - rectTransform.rect.width, 0.8f);
        Invoke(nameof(HideNotice), 2f);
    }
    private void HideNotice()
    {
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + rectTransform.rect.width, 0.8f);
    }
}
