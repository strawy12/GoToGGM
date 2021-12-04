using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HelpBox : MonoBehaviour
{
    private Text helpText;
    private RectTransform rectTransform;

    public int helpBoxID { get; private set; }

    private void Start()
    {
        helpText = transform.GetChild(0).GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void SetHelpText(string message, int childIndex)
    {
        if(helpBoxID != childIndex)
        {
            helpText.text = message;
            helpBoxID = childIndex;
            Vector2 boxSize = new Vector2(rectTransform.sizeDelta.x, helpText.preferredHeight + 100f);
            rectTransform.sizeDelta = boxSize;
            transform.SetSiblingIndex(childIndex);
            ActiveHelpBox(true);
        }
        else
        {
            helpBoxID = -1;
            ActiveHelpBox(false);
        }
    }

    public void ActiveHelpBox(bool isActive)
    {
        if (isActive)
        {
            if (transform.localScale.y < 1f)
            {
                gameObject.SetActive(true);
                transform.DOScaleY(1f, 0.5f).SetUpdate(true);
            }
        }

        else
        {
            if (transform.localScale.y > 0f)
            {
                transform.DOScaleY(0f, 0.5f).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
            }
        }
    }
}
