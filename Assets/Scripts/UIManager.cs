using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text storyText = null;
    [SerializeField] private GameObject touchScreen = null;
    [SerializeField] private SeletingBtnBase[] selectBtns = new SeletingBtnBase[3];

    private bool isWriting = false;
    private bool isSkip = false;
    private float currentWriteSpeed = 0f;

    public void StartWrite(string message, Action action = null, float writeSpeed = 0.03f)
    {
        StartCoroutine(WriteText(message, action, writeSpeed));
    }

    public IEnumerator WriteText(string message, Action action, float writeSpeed)
    {
        isWriting = true;
        ActiveTouchScreen(true);
        currentWriteSpeed = writeSpeed;
        float waitTime = currentWriteSpeed * 2f;
        string messageText = "";
        foreach (var c in message)
        {
            if (isSkip) break;

            messageText = string.Format("{0}{1}", messageText, c);
            storyText.text = messageText;
            yield return new WaitForSeconds(currentWriteSpeed);
            if (c == '\n')
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (!isSkip)
        {
            yield return new WaitForSeconds(1.25f);
        }
        else
        {
            storyText.text = message;
            yield return new WaitForSeconds(0.5f);
            isSkip = false;
        }

        if (action == null)
        {
            ShowSelectBtn();
        }

        else
        {
            action();
        }

        isWriting = false;
    }


    public void ShowSelectBtn()
    {
        SelectLine[] selectLines = GameManager.Inst.Story.GetNowStory().selectLines;

        for (int i = 0; i < 3; i++)
        {
            selectBtns[i].gameObject.SetActive(true);
            selectBtns[i].SettingBtn(selectLines[i]);
        }
    }
    public void UnShowSelectBtn(SeletingBtnBase seletingBtn = null)
    {
        for (int i = 0; i < 3; i++)
        {
            if (seletingBtn == selectBtns[i]) continue;
            selectBtns[i].gameObject.SetActive(false);
        }
    }

    public void OnClickTouchScreen()
    {

        if (!GameManager.Inst.Story.IsEndStory && isWriting)
        {
            isSkip = true;
            ActiveTouchScreen(false);
            return;
        }
        else if (!GameManager.Inst.Story.IsEndStory) return;

        ActiveTouchScreen(false);
        GameManager.Inst.Story.StartStory();
    }

    public void ActiveTouchScreen(bool isActive)
    {
        touchScreen.SetActive(isActive);
    }

    private void FadeObject(Image obj)
    {

    }
}
