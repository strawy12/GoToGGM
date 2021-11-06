using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using DG.Tweening;

public class SeletingBtnBase : MonoBehaviour
{
    [SerializeField] protected Text seletingText;

    protected CanvasGroup canvasGroup;
    protected Image btnImage;
    protected SelectLine currentSelectLine;
    protected EventStory currentEventStory;
    private ESelectType currentSelectState;
    protected Button button = null;

    public ESelectType SelectType { get { return currentSelectState; } }

    protected int btnNum = 0;

    public void SettingBtn(SelectLine selectLine, int num)
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if(btnImage == null)
        {
            btnImage = GetComponent<Image>();
        }

        if(canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        currentSelectLine = selectLine;
        currentSelectState = selectLine.selectType;
        btnNum = num;
        if (currentSelectState == ESelectType.Gread)
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID, true, Random.Range(0, 2) == 0);
        }
        else
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID);
        }
        if (currentSelectState == ESelectType.Special)
        {
            button.interactable = GameManager.Inst.CheckPlayerStat(currentSelectLine.needStatType, currentSelectLine.needStat);
        }
        else
        {
            button.interactable = true;
        }

        SetBtnState();

    }

    public void CheckInfo()
    {
        if (currentEventStory.increaseStatType != EStatType.None)
        {
            GameManager.Inst.SetPlayerStat(currentEventStory.increaseStatType, currentEventStory.increaseStat);
        }

    }

    public void SetBtnState()
    {
        seletingText.text = currentSelectLine.selectLine;
        GameManager.Inst.UI.ChangeSelectBtnSprite(btnImage, currentSelectState);
    }

    public void OnClickBtn()
    {
        if(currentSelectState == ESelectType.Final)
        {
            GameManager.Inst.Story.SetStoryNum();
            GameManager.Inst.Story.StartStory();
            GameManager.Inst.UI.UnShowSelectBtn();

            CheckInfo();
        }

        else
        {
            string storyLine = currentEventStory.eventMainStory;
            GameManager.Inst.UI.StartWrite(storyLine, GameManager.Inst.UI.ActiveFinalSelectBtn);
            GameManager.Inst.UI.UnShowSelectBtn();

            ChangeFinalSelect();
        }
        
    }

    public void ChangeFinalSelect()
    {
        Debug.Log(GameManager.Inst.StoryLine);
        seletingText.text = currentEventStory.eventFinalStory[GameManager.Inst.StoryLine];
        GameManager.Inst.UI.ChangeSelectBtnSprite(btnImage, currentSelectState);
        currentSelectState = ESelectType.Final;
    }



    public void SetEvent(Action action, bool isRemove)
    {
        if (isRemove)
        {
            button.onClick.RemoveListener(() => action());
        }
        else
        {
            button.onClick.AddListener(() => action());
        }
    }

    public void ActiveBtn(bool isActive)
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (isActive)
        {
            gameObject.SetActive(true);
            canvasGroup.DOFade(1f, 1f);
        }
        else
        {
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

    }

    public void ChangeLockImage()
    {

    }

    public void SetPlayerJob()
    {
        GameManager.Inst.SetPlayerJob(btnNum);
    }
}
