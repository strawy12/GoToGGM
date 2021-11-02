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
    private int storyLineNum = 0;
    private int currentScenarioCnt = 0;
    private int currentStoryNum = 0;
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
        return (int)storyLine.storyLines[storyLineNum].storyOrder[currentScenarioCnt];
    }


    public EventStory GetEventStory(int storyID, bool isGreed = false, bool isSuccess = false)
    {
        EventStory eventStory = null;
        EventStoryLine[] eventStoryLines = eventStories.eventScenarios[GetCurrentScenarioNum()].eventStoryLines;

        if (isGreed)
        {
            eventStory = Array.Find(eventStoryLines[currentStoryNum].eventStories, x => x.eventStoryID == storyID && x.isSuccess == isSuccess);
        }
        else
        {
            eventStory = Array.Find(eventStoryLines[currentStoryNum].eventStories, x => x.eventStoryID == storyID);
        }


        return eventStory;
    }

    public Story GetNowStory()
    {
        int storyNum = GetCurrentScenarioNum();
        return stories.stories[storyNum].stories[currentStoryNum];
    }

    public void SetStoryNum()
    {
        int maxStoryNum = stories.stories[GetCurrentScenarioNum()].stories.Length - 1;
        if (currentStoryNum < maxStoryNum)
        {
            currentStoryNum++;
            return;
        }

        currentScenarioCnt++;
        currentStoryNum = 0;
    }

    public void SetSelectBtn(SeletingBtnBase seletingBtn)
    {
        nowSelectBtn = seletingBtn;
    }


    public void StartStory()
    {
        if (endStory)
        {
            endStory = false;
        }
        Debug.Log(currentScenarioCnt);
        if (storyLine.storyLines[storyLineNum].storyOrder.Length <= currentScenarioCnt) return;
        GameManager.Inst.UI.StartWrite(GetNowStory().mainStory);
    }

    public void EndStory()
    {
        endStory = true;
        GameManager.Inst.UI.ActiveTouchScreen(true);
        GameManager.Inst.UI.UnShowSelectBtn();

    }

}
