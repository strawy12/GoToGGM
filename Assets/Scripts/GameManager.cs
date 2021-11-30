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
    private string SAVE_PATH = "";
    private string SAVE_FILE = "/SaveFile.txt";

    [SerializeField] private Player player;
    public Player CurrentPlayer { get { return player; } }

    private AchievementManager achievementManager;
    private UIManager uiManager;
    private StoryManager storyManager;
    private Timer timer = new Timer();

    public int stat;
    public UIManager UI { get { return uiManager; } }
    public StoryManager Story { get { return storyManager; } }
    public Timer Timer { get { return timer; } }
    public int StoryLine { get { return player.storyLineNum; } }
    
    void Awake()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        achievementManager = GetComponent<AchievementManager>();
        uiManager = GetComponent<UIManager>();
        storyManager = GetComponent<StoryManager>();

        LoadFromJson();
    }

    private void Start()
    {
        Story.SettingStory();
        UI.SetNowTimeText();
        UI.SetJobText();
        UI.SetStatText();
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveToJson();
    }

    private void LoadFromJson()
    {
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string stringJson = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            player = JsonUtility.FromJson<Player>(stringJson);
        }
        else
        {
            player = new Player("고등학생", 0, 0, 0);
            SaveToJson();
        }
    }

    private void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(player, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, stringJson, System.Text.Encoding.UTF8);
    }

    public void SelectJob()
    {
        UI.SetEventToSelectBtn(false);  
    }

    public void SetNowTime()
    {
        int index = player.usedTimeCnt;
        player.usedTimeCnt++;
        player.nowTime += player.arrivalTime;
        player.arrivalTime = 0;
        player.nowTime += Story.GetStoryLine().usedTimeArray[index];
        UI.SetNowTimeText();
    }

    
    public void SetPlayerJob(int jobNum)
    {
        if (jobNum == 0)
        {
            player.playerjob = "기획자";
            player.storyLineNum = 0;
            SetPlayerStat(EStatType.Sencetive, 5);
            SetPlayerStat(EStatType.Knowledge, 5);
            SetPlayerStat(EStatType.Wit, 10);
        }
        else if (jobNum == 1)
        {
            player.playerjob = "프로그래머";
            player.storyLineNum = 1;
            SetPlayerStat(EStatType.Sencetive, 5);
            SetPlayerStat(EStatType.Knowledge, 10);
            SetPlayerStat(EStatType.Wit, 5);
        }
        else if (jobNum == 2)
        {
            player.playerjob = "아티스트";
            player.storyLineNum = 2;
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
                player.stat_Knowledge += increaseStat;
                break;

            case EStatType.Wit:
                player.stat_Wit += increaseStat;
                break;

            case EStatType.Sencetive:
                player.stat_Sencetive += increaseStat;
                break;

            case EStatType.ArrivalTime:
                player.arrivalTime += increaseStat;
                UI.ShowArriveTimeDangerousMessage(player.arrivalTime, player.GetLastWord(), player.arrivalTime > 0);

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
                return player.stat_Knowledge >= needStat;

            case EStatType.Wit:
                return player.stat_Wit >= needStat;

            case EStatType.Sencetive:
                return player.stat_Sencetive >= needStat;
        }
        return true;
    }

    public void SaveClears(int ID)
    {
        player.clears[ID] = true;
    }

    public int CheckArrivalTime()
    {
        if(player.arrivalTime > 0)
        {
            return 0;
        }

        else if(player.arrivalTime == 0)
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
        if (ID < 71) return;
        ID -= 71;
        if (player.playerjob == "프로그래머")
        {
            ID += 3;
        }
        else if (player.playerjob == "아티스튼")
        {
            ID += 6;
        }
    }
}
