using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryLines storyLine;
    [SerializeField] private Stories stories;
    [SerializeField] private EventStories eventStories;
    private SeletingBtnBase nowSelectBtn;


    private bool endStory = false;

    public bool IsEndStory
    {
        get
        {
            return endStory;
        }
    }

    private void Start()
    {
        StartStory();
    }

    public int GetCurrentScenarioNum()
    {
        return (int)storyLine.storyLines[GameManager.Inst.StoryLine].storyOrder[GameManager.Inst.CurrentPlayer.currentScenarioCnt];
    }


    public EventStory GetEventStory(int storyID, bool isGreed = false, bool isSuccess = false)
    {
        EventStory eventStory = null;
        EventStoryLine[] eventStoryLines = eventStories.eventScenarios[GetCurrentScenarioNum()].eventStoryLines;

        if (isGreed)
        {
            eventStory = Array.Find(eventStoryLines[GameManager.Inst.CurrentPlayer.currentStoryNum].eventStories, x => x.eventStoryID == storyID && x.isSuccess == isSuccess);
        }
        else
        {
            eventStory = Array.Find(eventStoryLines[GameManager.Inst.CurrentPlayer.currentStoryNum].eventStories, x => x.eventStoryID == storyID);
        }


        return eventStory;
    }

    public Story GetNowStory()
    {
        int storyNum = GetCurrentScenarioNum();
        return stories.stories[storyNum].stories[GameManager.Inst.CurrentPlayer.currentStoryNum];
    }


    public void SetStoryNum()
    {
        int maxStoryNum = stories.stories[GetCurrentScenarioNum()].stories.Length - 1;
        if (GameManager.Inst.CurrentPlayer.currentStoryNum < maxStoryNum)
        {
            GameManager.Inst.CurrentPlayer.currentStoryNum++;
            return;
        }

        GameManager.Inst.CurrentPlayer.currentScenarioCnt++;
        GameManager.Inst.CurrentPlayer.currentStoryNum = 0;
    }

    public void SetSelectBtn(SeletingBtnBase seletingBtn)
    {
        nowSelectBtn = seletingBtn;
    }

    public void StartEvent(int storyID)
    {
        switch (storyID)
        {
            case 11:
                Action<bool> action = GameManager.Inst.UI.ActiveNameInputField;
                GameManager.Inst.UI.StartWrite(GetNowStory().mainStory, action, true);
                break;
        }

    }

    public void StartStory()
    {
        if (endStory)
        {
            endStory = false;
        }
        if (storyLine.storyLines[GameManager.Inst.StoryLine].storyOrder.Length <= GameManager.Inst.CurrentPlayer.currentScenarioCnt) return;
        
        Story story = GetNowStory();


        if (story.usedFunc)
        {
            StartEvent(story.storyID);
            return;
        }

        GameManager.Inst.UI.StartWrite(GetNowStory().mainStory);
    }

    public void EndStory()
    {
        Debug.Log("дв╢Г");
        endStory = true;
        GameManager.Inst.UI.ActiveTouchScreen(true);
        GameManager.Inst.UI.SetStatText();
    }

}
