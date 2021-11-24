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
    private EffectSetting[] nowEffectSettings;

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
        StartSceneStory();
    }

    public StoryLine GetStoryLine()
    {
        return storyLine.storyLines[GameManager.Inst.StoryLine];
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

    public Scenario GetNowScenario()
    {
        int storyNum = GetCurrentScenarioNum();
        return stories.scenarios[storyNum];
    }


    public void SetStoryNum()
    {
        int maxStoryNum = stories.scenarios[GetCurrentScenarioNum()].stories.Length - 1;
        int crtStoryNum = GameManager.Inst.CurrentPlayer.crtStoryNum;
        bool usedEventStory = stories.scenarios[GetCurrentScenarioNum()].stories[crtStoryNum].selectLines.Length >= 3;

        if (GameManager.Inst.CurrentPlayer.crtStoryNum < maxStoryNum)
        {
            GameManager.Inst.CurrentPlayer.crtStoryNum++;
            if (usedEventStory)
            {
                GameManager.Inst.CurrentPlayer.crtEventStoryCnt++;
            }
            return;
        }

        GameManager.Inst.UI.CheckPlayerPoint();
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
                if (GameManager.Inst.CurrentPlayer.playerjob == "¾ÆÆ¼½ºÆ®")
                {
                    story = messages[1];
                }
                else
                {
                    story = messages[0];
                }
                GameManager.Inst.UI.StartWrite(story, false, GameManager.Inst.UI.ShowSingleSelectBtn);
                break;
        }
    }

    public void StartSceneStory(float delay = 0f)
    {
        StartCoroutine(GameManager.Inst.UI.MoveAnimScene(delay));
    }

    public void StartStory()
    {
        if (endStory)
        {
            endStory = false;
        }
        if (storyLine.storyLines[GameManager.Inst.StoryLine].storyOrder.Length <= GameManager.Inst.CurrentPlayer.crtScenarioCnt)
        {
            return;
        }

        Story story = GetNowStory();

        bool usedEffect = story.effectSettings.Length != 0;
        if (usedEffect)
        {
            Debug.Log("¸ÚÀÖ´Ù ÀÌ¹Î¿µ");
            nowEffectSettings = story.effectSettings;
        }

        if (story.usedFunc)
        {
            StartEvent(story.storyID);
            return;
        }

        GameManager.Inst.UI.StartWrite(GetNowStory().mainStory, usedEffect);
    }

    public void EndStory()
    {
        endStory = true;
        GameManager.Inst.UI.ActiveTouchScreen(true);
        GameManager.Inst.UI.SetStatText();
    }

    public void CheckEffect(int storyOrder)
    {
        for (int i = 0; i < nowEffectSettings.Length; i++)
        {
            if (storyOrder != nowEffectSettings[i].playStoryOrder) continue;

            PlayEffect(nowEffectSettings[i].usedEffect, nowEffectSettings[i].effectNum);
        }
    }

    public void PlayEffect(EEffectType type, int effectNum)
    {
        switch (type)
        {
            case EEffectType.BackGround:
                GameManager.Inst.UI.ChangeBackGround(effectNum);
                break;

            case EEffectType.Sound:
                GameManager.Inst.UI.SetEffectSound(effectNum);
                break;

            case EEffectType.Effect:
                GameManager.Inst.UI.PlayEffect(effectNum);
                break;

        }
    }

}
