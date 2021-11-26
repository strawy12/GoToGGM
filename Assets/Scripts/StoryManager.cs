using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
                GameManager.Inst.UI.StartWrite(GetNowStory().mainStory, action, true, GetNowStory().usedEffect);
                break;
            case 13:
                GameManager.Inst.SelectJob();
                GameManager.Inst.UI.StartWrite(GetNowStory().mainStory, GetNowStory().usedEffect);
                break;
            case 24:
                CertainJobPlay("��ȹ��&������", "��Ƽ��Ʈ");
                break;

            case 44:
                CertainJobPlay("��ȹ��", "������");
                break;

            case 64:
                CertainJobPlay("��ȹ��", "��Ƽ��Ʈ");
                break;
        }
    }

    public void CertainJobPlay(string firstJobs, string sencondJobs)
    {
        string[] fJobs = firstJobs.Split('&');
        string[] sJobs = sencondJobs.Split('&');

        string[] messages = GetNowStory().mainStory.Split('&');

        string story = "";

        for (int i = 0; i < fJobs.Length; i++)
        {
            if (GameManager.Inst.CurrentPlayer.playerjob == fJobs[i])
            {
                story = messages[1];
                GameManager.Inst.UI.StartWrite(story, GameManager.Inst.UI.ShowSingleSelectBtn, 1, GetNowStory().usedEffect);
                return;
            }
        }

        for (int i = 0; i < sJobs.Length; i++)
        {
            if (GameManager.Inst.CurrentPlayer.playerjob == sJobs[i])
            {
                story = messages[0];
                GameManager.Inst.UI.StartWrite(story, GameManager.Inst.UI.ShowSingleSelectBtn, 0, GetNowStory().usedEffect);
                return;
            }
        }

        story = messages[0];
        GameManager.Inst.UI.StartWrite(story, GameManager.Inst.UI.ShowSingleSelectBtn, 0, GetNowStory().usedEffect);

    }

    public void StartSceneStory(float delay = 0f)
    {

        StartCoroutine(GameManager.Inst.UI.MoveAnimScene(delay));
    }

    public IEnumerator AutoSelectBtn()
    {
        yield return new WaitForSeconds(2.5f);
        List<SeletingBtnBase> seletingBtns = GameManager.Inst.UI.GetActiveSelectBtn();

        if (seletingBtns.Count > 0)
        {
            if (seletingBtns.Count > 1)
            {
                seletingBtns[Random.Range(0, seletingBtns.Count)].OnClickBtn();
            }

            else
            {
                seletingBtns[0].OnClickBtn();
            }

        }
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

        GameManager.Inst.UI.CheckBGFade();

        Story story = GetNowStory();

        if (story.usedEffect)
        {
            nowEffectSettings = story.effectSettings;
        }

        else
        {
            nowEffectSettings = null;
        }


        if (story.usedFunc)
        {
            StartEvent(story.storyID);
            return;
        }
        GameManager.Inst.UI.StartWrite(GetNowStory().mainStory, story.usedEffect);
    }

    public void EndStory()
    {
        endStory = true;
        GameManager.Inst.UI.ActiveTouchScreen(true);
        GameManager.Inst.UI.SetStatText();
    }

    public float CheckEffect(int storyOrder)
    {
        float delaySum = 0f;
        for (int i = 0; i < nowEffectSettings.Length; i++)
        {
            if (storyOrder != nowEffectSettings[i].playStoryOrder) continue;

            delaySum += PlayEffect(nowEffectSettings[i].usedEffect, nowEffectSettings[i].effectNum);
        }

        return delaySum;
    }

    public float PlayEffect(EEffectType type, int effectNum)
    {
        switch (type)
        {
            case EEffectType.BackGround:
                GameManager.Inst.UI.ChangeBackGround(effectNum);
                return 0f;

            case EEffectType.Sound:
                GameManager.Inst.UI.SetEffectSound(effectNum);
                return 0f;

            case EEffectType.Effect:
                return GameManager.Inst.UI.PlayEffect(effectNum);

        }

        return 0f;
    }

}
