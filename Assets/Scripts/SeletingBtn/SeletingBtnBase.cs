using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SeletingBtnBase : MonoBehaviour
{
    [SerializeField] protected Text seletingText;

    protected CanvasGroup canvasGroup;
    protected Image btnImage;
    protected SelectLine currentSelectLine;
    protected EventStory currentEventStory;
    private ESelectType currentSelectState;
    protected Button currentButton = null;

    public ESelectType SelectType { get { return currentSelectState; } }

    protected int btnNum = 0;

    public void Start()
    {
        currentButton = GetComponent<Button>();
        btnImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }


    public void SettingBtn(SelectLine selectLine, int num = 0)
    {
        currentSelectLine = selectLine;
        currentSelectState = selectLine.selectType;
        btnNum = num;

        if (currentSelectState == ESelectType.Gread)
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID, true, Random.Range(0, 2) == 0);
        }

        else if (currentSelectState == ESelectType.Final)
        {
            GameManager.Inst.Story.EndStory();
            currentEventStory = null;
        }

        else
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID);
        }

        if (currentSelectState == ESelectType.Special)
        {
            currentButton.interactable = GameManager.Inst.CheckPlayerStat(currentSelectLine.needStatType, currentSelectLine.needStat);
        }
        else
        {
            currentButton.interactable = true;
        }

        SetBtnState();

    }

    public void CheckInfo()
    {
        if (currentEventStory == null) return;

        if (currentEventStory.increaseStatType != EStatType.None)
        {
            GameManager.Inst.SetPlayerStat(currentEventStory.increaseStatType, currentEventStory.increaseStat);
        }
        if (currentEventStory.increaseTime != 0)
        {
            GameManager.Inst.UI.SetTime(currentEventStory.increaseTime);
        }
    }

    public void SetBtnState()
    {
        seletingText.text = currentSelectLine.selectLine;
        GameManager.Inst.UI.ChangeSelectBtnSprite(btnImage, currentSelectState);
    }
    public void OnClickBtn()
    {
        GameManager.Inst.UI.isSelected = true;
        currentButton.interactable = false;
        if (currentSelectState == ESelectType.Final)
        {
            CheckInfo();

            GameManager.Inst.UI.UnShowSelectBtn();
            GameManager.Inst.Story.SetStoryNum();

            if (GameManager.Inst.Story.IsEndScenario || 
                (currentEventStory != null && currentEventStory.increaseStatType == EStatType.ArrivalTime))
            {
                GameManager.Inst.Story.StartSceneStory(4f);
            }
            else
            {
                GameManager.Inst.Story.StartSceneStory();
            }



        }

        else
        {
            if (currentSelectState == ESelectType.Gread)
            {
                GameManager.Inst.CheckLucky(currentEventStory.isSuccess);
            }

            if (currentEventStory.usedEffect)
            {
                GameManager.Inst.Story.SetNowEffectSettings(currentEventStory.effectSettings);
            }

            string storyLine = currentEventStory.eventMainStory;
            GameManager.Inst.UI.StartWrite(storyLine, currentEventStory.usedEffect, GameManager.Inst.UI.ActiveFinalSelectBtn);
            GameManager.Inst.UI.UnShowSelectBtn();

            ChangeFinalSelect();
        }

    }

    public void ChangeFinalSelect()
    {
        int index = 0;
        if (currentEventStory.eventFinalStory.Length > 1)
        {
            index = GameManager.Inst.StoryLine;
        }
        seletingText.text = currentEventStory.eventFinalStory[index];
        currentSelectState = ESelectType.Final;
        GameManager.Inst.UI.ChangeSelectBtnSprite(btnImage, currentSelectState);
    }



    public void SetEvent(Action action, bool isRemove)
    {
        if (isRemove)
        {
            currentButton.onClick.RemoveAllListeners();
        }
        else
        {
            currentButton.onClick.AddListener(() => action());
        }
    }

    public void ActiveBtn(bool isActive)
    {
        if (isActive)
        {
            if (currentSelectState != ESelectType.Special)
            {
                currentButton.interactable = true;
            }
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
