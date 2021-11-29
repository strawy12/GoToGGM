using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearNoticeScript : MonoBehaviour
{
    [SerializeField] Text textClearTitle = null;
    RectTransform rectTransform = null;

    float minPos;
    float maxPos;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        minPos = rectTransform.anchoredPosition.x - rectTransform.rect.width;
        maxPos = rectTransform.anchoredPosition.x;
        gameObject.SetActive(false);
    }
    public void ShowNotice(string title)
    {
        gameObject.SetActive(true);
        textClearTitle.text = string.Format("업적 \" {0} \" 달성", title);
        rectTransform.DOAnchorPosX(Mathf.Clamp(rectTransform.anchoredPosition.x - rectTransform.rect.width, minPos, maxPos), 0.8f);
        Invoke(nameof(HideNotice), 2f);
    }
    private void HideNotice()
    {
        rectTransform.DOAnchorPosX(Mathf.Clamp(rectTransform.anchoredPosition.x + rectTransform.rect.width, minPos, maxPos), 0.8f);
    }
}