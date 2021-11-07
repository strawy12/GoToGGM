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
        return (int)storyLine.storyLines[GameManager.Inst.StoryLine].storyOrder[GameManager.Inst.CurrentPlayer.crtScenarioCnt];
    }


    public EventStory GetEventStory(int storyID, bool isGreed = false, bool isSuccess = false)
    {
        EventStory eventStory = null;
        EventStoryLine[] eventStoryLines = eventStories.eventScenarios[GetCurrentScenarioNum()].eventStoryLines;

        if (isGreed)
        {
            eventStory = Array.Find(eventStoryLines[GameManager.Inst.CurrentPlayer.crtEventStoryCnt].eventStories, x => x.eventStoryID == storyID && x.isSuccess == isSuccess);
        }
        else
        {
            eventStory = Array.Find(eventStoryLines[GameManager.Inst.CurrentPlayer.crtEventStoryCnt].eventStories, x => x.eventStoryID == storyID);
        }

        

        return eventStory;
    }

    public Story GetNowStory()
    {
        int storyNum = GetCurrentScenarioNum();
        return stories.scenarios[storyNum].stories[GameManager.Inst.CurrentPlayer.crtStoryNum];
    }


    public void SetStoryNum()
    {
        int maxStoryNum = stories.scenarios[GetCurrentScenarioNum()].stories.Length - 1;
        int crtStoryNum = GameManager.Inst.CurrentPlayer.crtStoryNum;
        bool usedEventStory = stories.scenarios[GetCurrentScenarioNum()].stories[crtStoryNum].selectLines.Length >= 3;

        if (GameManager.Inst.CurrentPlayer.crtStoryNum < maxStoryNum)
        {
            GameManager.Inst.CurrentPlayer.crtStoryNum++;

            if(usedEventStory)
            {
                GameManager.Inst.CurrentPlayer.crtEventStoryCnt++;
            }
            return;
        }

        GameManager.Inst.CurrentPlayer.crtScenarioCnt++;
        GameManager.Inst.CurrentPlayer.crtStoryNum = 0;
        GameManager.Inst.CurrentPlayer.crtEventStoryCnt = 0;
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
            case 12:
                GameManager.Inst.SelectJob();
                GameManager.Inst.UI.StartWrite(GetNowStory().mainStory);
                break;
            case 24:
                string[] messages = GetNowStory().mainStory.Split('&');
                string story = "";
                if(GameManager.Inst.CurrentPlayer.playerjob == "아티스트")
                {
                    story = messages[1];
                }
                else
                {
                    story = messages[0];
                }
                GameManager.Inst.UI.StartWrite(story, GameManager.Inst.UI.ShowSingleSelectBtn);
                break;
        }
    }

    public void StartStory()
    {
        Debug.Log("응애");

        if (endStory)
        {
            endStory = false;
        }
        if (storyLine.storyLines[GameManager.Inst.StoryLine].storyOrder.Length <= GameManager.Inst.CurrentPlayer.crtScenarioCnt)
        {
            Debug.Log("응애");
            return;
        }

        Story story = GetNowStory();


        if (story.usedFunc)
        {
            Debug.Log(story.storyID);

            StartEvent(story.storyID);
            return;
        }
        Debug.Log("응애");

        GameManager.Inst.UI.StartWrite(GetNowStory().mainStory);
    }

    public void EndStory()
    {
        endStory = true;
        GameManager.Inst.UI.ActiveTouchScreen(true);
        GameManager.Inst.UI.SetStatText();
    }

}
