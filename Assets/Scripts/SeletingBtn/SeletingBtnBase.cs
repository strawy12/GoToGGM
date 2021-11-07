using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class SeletingBtnBase : MonoBehaviour
{
    [SerializeField] protected Text seletingText;
    [SerializeField] private Text btnStateText;

    protected SelectLine currentSelectLine;
    protected EventStory currentEventStory;
    private ESelectType currentSelectState;
    protected Button button = null;

    protected int btnNum = 0;

    public void SettingBtn(SelectLine selectLine, int num)
    {
        if (button == null)
        {
            button = GetComponent<Button>();
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
        if (currentEventStory.increaseTime != 0)
        {
            Debug.Log(currentEventStory.increaseTime);
            GameManager.Inst.UI.SetTime(currentEventStory.increaseTime);
            Debug.Log(currentEventStory.increaseTime);
        }
    }

    public void SetBtnState()
    {
        int endStringCnt = 1;

        if (currentSelectLine.selectType == ESelectType.Special)
        {
            endStringCnt = 2;
        }

        seletingText.text = currentSelectLine.selectLine;
        btnStateText.text = currentSelectLine.selectType.ToString().Substring(0, endStringCnt);
    }

    public void OnClickBtn()
    {
        string storyLine = currentEventStory.eventMainStory;
        GameManager.Inst.UI.StartWrite(storyLine, FinallyMention);
        GameManager.Inst.UI.UnShowSelectBtn(this);
        CheckInfo();
    }

    public void FinallyMention()
    {
        string storyLine = currentEventStory.eventStory;
        GameManager.Inst.UI.StartWrite(storyLine, GameManager.Inst.Story.EndStory);
        GameManager.Inst.Story.SetStoryNum();
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

    public void ChangeLockImage()
    {

    }

    public void SetPlayerJob()
    {
        GameManager.Inst.SetPlayerJob(btnNum);
    }
}
