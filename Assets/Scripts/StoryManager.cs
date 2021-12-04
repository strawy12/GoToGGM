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

    private bool endScenario = false;
    public bool IsEndScenario
    {
        get
        {
            return endScenario;
        }
    }

    public bool isEndding = false;



    private void Start()
    {
        if (DataManager.Inst.CurrentPlayer.crtScenarioCnt == 5 && !isEndding)
        {
            isEndding = true;
        }
        StartSceneStory();
    }

    private void CheckJobSelectScene()
    {
        Player player = GameManager.Inst.CurrentPlayer;
        if(player.playerjob != "고등학생" && GetNowStory().storyID == 13)
        {
            player.playerjob = "고등학생";
            player.stat_Knowledge = 0;
            player.stat_Sencetive = 0;
            player.stat_Wit = 0;
            player.arrivalTime = 0;
        }
    }

    public StoryLine GetStoryLine()
    {
        return storyLine.storyLines[GameManager.Inst.StoryLine];
    }

    public int GetCurrentScenarioNum()
    {
        return (int)storyLine.storyLines[GameManager.Inst.StoryLine].storyOrder[DataManager.Inst.CurrentPlayer.crtScenarioCnt];
    }


    public EventStory GetEventStory(int storyID, bool isGreed = false, bool isSuccess = false)
    {
        EventStory eventStory = null;
        EventStoryLine[] eventStoryLines = eventStories.eventScenarios[GetCurrentScenarioNum()].eventStoryLines;

        if (isGreed)
        {
            eventStory = Array.Find(eventStoryLines[DataManager.Inst.CurrentPlayer.crtEventStoryCnt].eventStories, x => x.eventStoryID == storyID && x.isSuccess == isSuccess);
        }
        else
        {
            eventStory = Array.Find(eventStoryLines[DataManager.Inst.CurrentPlayer.crtEventStoryCnt].eventStories, x => x.eventStoryID == storyID);
        }



        return eventStory;
    }

    public Story GetStory(int storyNum)
    {
        int scenarioNum = GetCurrentScenarioNum();
        return stories.scenarios[scenarioNum].stories[storyNum];
    }

    public Story GetEnddingStory(int num)
    {
        return stories.scenarios[6].stories[num];
    }

    public Story GetNowStory()
    {
        int storyNum = GetCurrentScenarioNum();
        return stories.scenarios[storyNum].stories[DataManager.Inst.CurrentPlayer.crtStoryNum];
    }

    public Scenario GetNowScenario()
    {
        int storyNum = GetCurrentScenarioNum();
        return stories.scenarios[storyNum];
    }


    public void SetStoryNum()
    {
        int maxStoryNum = stories.scenarios[GetCurrentScenarioNum()].stories.Length - 1;
        int crtStoryNum = DataManager.Inst.CurrentPlayer.crtStoryNum;
        bool usedEventStory = stories.scenarios[GetCurrentScenarioNum()].stories[crtStoryNum].selectLines.Length >= 3;

        if (DataManager.Inst.CurrentPlayer.crtStoryNum < maxStoryNum)
        {
            DataManager.Inst.CurrentPlayer.crtStoryNum++;
            if (usedEventStory)
            {
                DataManager.Inst.CurrentPlayer.crtEventStoryCnt++;
            }
            return;
        }

        SetScenraioNum();
    }



    private void SetScenraioNum()
    {
<<<<<<< HEAD
        DataManager.Inst.CurrentPlayer.crtScenarioCnt++;
=======
        GameManager.Inst.SetNowTime();
>>>>>>> System/Particle

        if (DataManager.Inst.CurrentPlayer.crtScenarioCnt == 5)
        {
            isEndding = true;
        }
        endScenario = true;

<<<<<<< HEAD
        DataManager.Inst.CurrentPlayer.crtStoryNum = 0;
        DataManager.Inst.CurrentPlayer.crtEventStoryCnt = 0;
        GameManager.Inst.UI.ResetStoryText();
        GameManager.Inst.SetNowTime();
=======
        GameManager.Inst.CurrentPlayer.crtStoryNum = 0;
        GameManager.Inst.CurrentPlayer.crtEventStoryCnt = 0;

        GameManager.Inst.UI.ResetStoryText();
>>>>>>> System/Particle
    }


    public void SetSelectBtn(SeletingBtnBase seletingBtn)
    {
        nowSelectBtn = seletingBtn;
    }

    public void StartEvent(Story story)
    {
        switch (story.storyID)
        {
            case 11:
                Action<bool> action = GameManager.Inst.UI.ActiveNameInputField;
                GameManager.Inst.UI.StartWrite(story.mainStory, action, true, story.usedEffect);
                break;
            case 13:
                GameManager.Inst.SelectJob();
                GameManager.Inst.UI.StartWrite(story.mainStory, story.usedEffect);
                break;
            case 24:
                CertainJobPlay("기획자&개발자", "게임 디자이너");
                break;

            case 44:
                CertainJobPlay("기획자", "개발자");
                break;

            case 54:
                CertainJobPlay("기획자", "게임 디자이너");
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
            if (DataManager.Inst.CurrentPlayer.playerjob == fJobs[i])
            {
                story = messages[0];
                GameManager.Inst.UI.StartWrite(story, GameManager.Inst.UI.ShowSingleSelectBtn, 0, GetNowStory().usedEffect);
                return;
            }
        }

        for (int i = 0; i < sJobs.Length; i++)
        {
            if (DataManager.Inst.CurrentPlayer.playerjob == sJobs[i])
            {
                story = messages[1];
                GameManager.Inst.UI.StartWrite(story, GameManager.Inst.UI.ShowSingleSelectBtn, 1, GetNowStory().usedEffect);
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

    public void SettingStory()
    {
        int crtStoryNum = DataManager.Inst.CurrentPlayer.crtStoryNum;

        if (crtStoryNum != 0 && GameManager.Inst.CurrentPlayer.crtScenarioCnt != 5)
        {
            Story story;
            for (int i = 0; i < crtStoryNum; i++)
            {
                story = GetStory(i);
                GameManager.Inst.UI.InstantiateStoryText(story.mainStory);

                if (story.usedEffect)
                {
                    nowEffectSettings = story.effectSettings;

                    SettingEffect();
                }
            }
        }
    }

    public void PlayEndding()
    {
        int enddingNum = GameManager.Inst.CheckArrivalTime();
        Story story = GetEnddingStory(enddingNum);
        nowEffectSettings = story.effectSettings;
        GameManager.Inst.ClearEnding(story.storyID);
        StartEndding(story);
    }

    public void StartStory()
    {
        if (endScenario)
        {
            endScenario = false;
        }
        if (storyLine.storyLines[GameManager.Inst.StoryLine].storyOrder.Length <= DataManager.Inst.CurrentPlayer.crtScenarioCnt)
        {
            return;
        }

        if (isEndding)
        {
            PlayEndding();
            return;
        }

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
            StartEvent(story);
            return;
        }
        GameManager.Inst.UI.StartWrite(GetNowStory().mainStory, story.usedEffect);
    }

    public void EndStory()
    {
        GameManager.Inst.UI.ActiveTouchScreen(true);
        GameManager.Inst.UI.SetStatText();
    }

    public float CheckEffect(int storyOrder)
    {
        float delaySum = 0f;

        for (int i = 0; i < nowEffectSettings.Length; i++)
        {
            if (storyOrder != nowEffectSettings[i].playStoryOrder) continue;

            delaySum = Mathf.Max(delaySum, PlayEffect(nowEffectSettings[i].usedEffect, nowEffectSettings[i].effectNum));
        }
        transform.SetSiblingIndex(0);
        return delaySum;
    }

    private void ReplaceRoomBGM(int i)
    {
        if (nowEffectSettings[i].usedEffect == EEffectType.BGM)
        {
            if (nowEffectSettings[i].effectNum == 0)
            {
                if (DataManager.Inst.CurrentPlayer.crtStoryNum > 1)
                {
                    nowEffectSettings[i].effectNum = 1;
                }
            }
        }
    }


    public void SettingEffect()
    {
        for (int i = 0; i < nowEffectSettings.Length; i++)
        {
            if (nowEffectSettings[i].usedEffect == EEffectType.Effect) continue;
            if (nowEffectSettings[i].usedEffect == EEffectType.Sound) continue;

            ReplaceRoomBGM(i);

            PlayEffect(nowEffectSettings[i].usedEffect, nowEffectSettings[i].effectNum);
        }
    }

    public void SetNowEffectSettings(EffectSetting[] effectSettings)
    {
        nowEffectSettings = effectSettings;
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
                return GameManager.Inst.UI.GetEffectSoundLength();

            case EEffectType.Effect:
                return GameManager.Inst.UI.PlayEffect(effectNum);

            case EEffectType.BGM:
                GameManager.Inst.UI.SetBGM(effectNum);
                return 0f;

        }

        return 0f;
    }


    private void StartEndding(Story story)
    {
        int num = story.storyID - 70;
        Debug.Log(story.storyName);
        GameManager.Inst.UI.StartWrite(story.mainStory, PlayEnddingCredit, num, true);
    }


   public void PlayEnddingCredit(int num)
    {
        switch (num)
        {
            case 1:
                StartCoroutine(GameManager.Inst.UI.LateEnddingEffect());
                break;

            case 3:
                StartCoroutine(GameManager.Inst.UI.FastEnddingEffect());
                break;


            case 2:
                StartCoroutine(GameManager.Inst.UI.SpecialEnddingEffect());
                break;

        }
    }

}
