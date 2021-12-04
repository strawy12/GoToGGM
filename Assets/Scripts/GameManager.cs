using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ESelectType
{
    Normal,
    Gread,
    Special,
    Final,
    Hidden
}

public enum EStatType
{
    None,
    Sencetive,
    Knowledge,
    Wit,
    ArrivalTime
}

public enum EStoryOrder
{
    Room,
    Bus1,
    Bus2,
    Subway1,
    Transfer,
    Walk,
    Endding
}

public enum EEffectType
{
    BackGround,
    Sound,
    Effect,
    BGM
}


//public enum btnState { Special, Normal, Good }

public class GameManager : MonoSingleTon<GameManager>
{

    private ParticleManager particleManager;
    private AchievementManager achievementManager;
    private UIManager uiManager;
    private StoryManager storyManager;
    private DataManager dataManager;
    private Timer timer = new Timer();

    public int stat;
    public ParticleManager Particle { get { return particleManager; } }
    public AchievementManager Achivement { get { return achievementManager; } }
    public UIManager UI { get { return uiManager; } }
    public StoryManager Story { get { return storyManager; } }
    public Timer Timer { get { return timer; } }
    public int StoryLine { get { return DataManager.Inst.CurrentPlayer.storyLineNum; } }
    
    void Awake()
    {
        achievementManager = GetComponent<AchievementManager>();
        particleManager = GetComponent<ParticleManager>();
        uiManager = GetComponent<UIManager>();
        storyManager = GetComponent<StoryManager>();
        dataManager = GetComponent<DataManager>();
    }

    private void Start()
    {
        Story.SettingStory();
        UI.SetNowTimeText();
        UI.SetJobText();
        UI.SetStatText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.ActiveQuitPanal(true);
        }
    }

    private void OnApplicationQuit()
    {
        DataManager.Inst.SaveToJson();
    }

    private void OnApplicationPause(bool pause)
    {
        DataManager.Inst.SaveToJson();
    }

    public void SelectJob()
    {
        UI.SetEventToSelectBtn(false);
    }

    public void DataReset()
    {
    }

    public void SetNowTime()
    {
        int index = DataManager.Inst.CurrentPlayer.usedTimeCnt;
        DataManager.Inst.CurrentPlayer.usedTimeCnt++;
        DataManager.Inst.CurrentPlayer.nowTime += Story.GetStoryLine().usedTimeArray[index];
        UI.SetNowTimeText();
        UI.ShowArriveTimeDangerousMessage();
    }


    public void SetPlayerJob(int jobNum)
    {
        if (jobNum == 0)
        {
            DataManager.Inst.CurrentPlayer.playerjob = "기획자";
            DataManager.Inst.CurrentPlayer.storyLineNum = 0;
            SetPlayerStat(EStatType.Sencetive, 5);
            SetPlayerStat(EStatType.Knowledge, 5);
            SetPlayerStat(EStatType.Wit, 10);
        }
        else if (jobNum == 1)
        {
            DataManager.Inst.CurrentPlayer.playerjob = "프로그래머";
            DataManager.Inst.CurrentPlayer.storyLineNum = 1;
            SetPlayerStat(EStatType.Sencetive, 5);
            SetPlayerStat(EStatType.Knowledge, 10);
            SetPlayerStat(EStatType.Wit, 5);
        }
        else if (jobNum == 2)
        {
            DataManager.Inst.CurrentPlayer.playerjob = "아티스트";
            DataManager.Inst.CurrentPlayer.storyLineNum = 2;
            SetPlayerStat(EStatType.Sencetive, 10);
            SetPlayerStat(EStatType.Knowledge, 5);
            SetPlayerStat(EStatType.Wit, 5);
        }

        UI.SetJobText();
        UI.SetStatText();
        UI.SetEventToSelectBtn(true);
    }

    public void SetPlayerStat(EStatType type, int increaseStat)
    {
        switch (type)
        {
            case EStatType.Knowledge:
                DataManager.Inst.CurrentPlayer.stat_Knowledge += increaseStat;
                break;

            case EStatType.Wit:
                DataManager.Inst.CurrentPlayer.stat_Wit += increaseStat;
                break;

            case EStatType.Sencetive:
                DataManager.Inst.CurrentPlayer.stat_Sencetive += increaseStat;
                break;

            case EStatType.ArrivalTime:
                DataManager.Inst.CurrentPlayer.arrivalTime += increaseStat;
                UI.ShowArriveTimeDangerousMessage();
                break;
        }
        achievementManager.CheckMacGyver();
        UI.SetStatText();
    }

    public bool CheckPlayerStat(EStatType type, int needStat)
    {
        switch (type)
        {
            case EStatType.Knowledge:
                return DataManager.Inst.CurrentPlayer.stat_Knowledge >= needStat;

            case EStatType.Wit:
                return DataManager.Inst.CurrentPlayer.stat_Wit >= needStat;

            case EStatType.Sencetive:
                return DataManager.Inst.CurrentPlayer.stat_Sencetive >= needStat;
        }
        return true;
    }

    public void SaveClears(int ID)
    {
        DataManager.Inst.CurrentPlayer.clears[ID] = true;
    }

    public int CheckArrivalTime()
    {
        if(DataManager.Inst.CurrentPlayer.arrivalTime > 0)
        {
            return 0;
        }

        else if(DataManager.Inst.CurrentPlayer.arrivalTime == 0)
        {
            return 1;
        }

        else
        {
            return 2;
        }
    }

    public void CheckLucky(bool isSuccess)
    {
        if (isSuccess)
            achievementManager.CheckLucky();
        else
            achievementManager.CheckUnlucky();
    }

    public void ClearEnding(int ID)
    {
        Debug.Log(ID);
        if (ID < 71) return;
        ID -= 71;
        if (DataManager.Inst.CurrentPlayer.playerjob == "프로그래머")
        {
            ID += 3;
        }
        else if (DataManager.Inst.CurrentPlayer.playerjob == "아티스트")
        {
            ID += 6;
        }
        achievementManager.ClearEnding(ID);
    }
}
