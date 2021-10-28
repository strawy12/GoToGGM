using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryLine storyLine;
    [SerializeField] private Stories stories;
    [SerializeField] private EventStories eventStories;
    private SeletingBtnBase nowSelectBtn;
    private int sencenairoNum = 0;
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

    public int GetCurrentStoryNum()
    {
        return storyLine.story[sencenairoNum].storyOrder[currentStoryNum];
    }

    public EventStory GetEventStory(int storyID, bool isGreed = false, bool isSuccess = false)
    {
        EventStory eventStory;

        if (isGreed)
        {
            eventStory = Array.Find(eventStories.eventStories, x => x.eventStoryID == storyID && x.isSuccess == isSuccess);
        }
        else
        {
            eventStory = Array.Find(eventStories.eventStories, x => x.eventStoryID == storyID);
        }

        return eventStory;
    }

    public Story GetNowStory()
    {
        int storyNum = GetCurrentStoryNum();
        return stories.stories[storyNum];
    }

    public void SetStoryNum(int increaseNum = 1)
    {
        currentStoryNum+= increaseNum;
    }
    
    public void SetSelectBtn(SeletingBtnBase seletingBtn)
    {
        nowSelectBtn = seletingBtn;
    }


    public void StartStory()
    {
        if(endStory)
        {
            endStory = false;
        }
        Debug.Log(currentStoryNum);
        if (storyLine.story[sencenairoNum].storyOrder.Length <= currentStoryNum) return;
        GameManager.Inst.UI.StartWrite(GetNowStory().mainStory);
    }

    public void EndStory()
    {
        endStory = true;
        GameManager.Inst.UI.ActiveTouchScreen(true);
        GameManager.Inst.UI.UnShowSelectBtn();

    }

}
