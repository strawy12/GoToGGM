using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearNoticeScript : MonoBehaviour
{
    [SerializeField] Text textClearTitle = null;
    RectTransform rectTransform = null;

    Image checkMarkImage = null;

    float minPos;
    float maxPos;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        checkMarkImage = transform.GetChild(1).GetComponent<Image>();
        minPos = rectTransform.anchoredPosition.x - rectTransform.rect.width;
        maxPos = rectTransform.anchoredPosition.x;  
        gameObject.SetActive(false);
        checkMarkImage.DOFade(0f, 0f);
    }
    public void ShowNotice(string title)
    {
        gameObject.SetActive(true);
        textClearTitle.text = string.Format( "\" {0} \" ´Þ¼º", title);
        rectTransform.DOAnchorPosX(Mathf.Clamp(rectTransform.anchoredPosition.x - rectTransform.rect.width, minPos, maxPos), 0.8f).OnComplete(() =>
        checkMarkImage.DOFade(1f, 1f));

        Invoke(nameof(HideNotice), 2f);
    }
    private void HideNotice()
    {
        rectTransform.DOAnchorPosX(Mathf.Clamp(rectTransform.anchoredPosition.x + rectTransform.rect.width, minPos, maxPos), 0.8f);
    }
}