using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MSGBoxScript : MonoBehaviour
{
    bool isShown=false;

    public void ShowMSGBox()
    {
        ShowPanels(gameObject.GetComponent<RectTransform>());
    }
    public void ShowPanels(RectTransform gameObject)
    {
        if (isShown) return;
        isShown = true;
        Vector2 Scale = new Vector2(gameObject.localScale.x, gameObject.localScale.y);
        gameObject.transform.localScale = Vector3.zero;
        gameObject.gameObject.SetActive(true);
        gameObject.transform.DOScale(Scale, 0.4f);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void Cancle()
    {
        gameObject.SetActive(false);
        isShown = false;
    }
}
