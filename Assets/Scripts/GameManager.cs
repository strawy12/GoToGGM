using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ESelectType
{
    Normal,
    Gread,
    Special,
    Final
}

public enum EStatType
{
    None,
    Sencetive,
    Knowledge,
    Wit
}

public enum EStoryOrder
{
    Room,
    Bus,
    Subway
}
//public enum btnState { Special, Normal, Good }

public class GameManager : MonoSingleTon<GameManager>
{
    private string SAVE_PATH = "";
    private string SAVE_FILE = "/SaveFile.txt";

    [SerializeField] private Player player;
    public Player CurrentPlayer { get { return player; } }

    private UIManager uiManager;
    private StoryManager storyManager;
    public int stat;
    public UIManager UI { get { return uiManager; } }
    public StoryManager Story { get { return storyManager; } }
    public int StoryLine { get { return player.storyLineNum; } }
    private Timer timer = new Timer();
    public Timer Timer { get { return timer; } }
    
    void Awake()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        uiManager = GetComponent<UIManager>();
        storyManager = GetComponent<StoryManager>();

        LoadFromJson();
    }

    private void Start()
    {
        UI.SetJobText();
        UI.SetStatText();

    }

    private void OnApplicationQuit()
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
    public void SetPlayerStat(int stat/*스탯 타입*/)
    {
        //player.stat[스탯타입] += stat;
    }

    public void SelectJob()
    {
        UI.SetEventToSelectBtn(false);
    }

    public void SetPlayerJob(int jobNum)
    {
        if (jobNum == 0)
        {
            player.playerjob = "기획자";
            SetPlayerStat(EStatType.Sencetive, 5);
            SetPlayerStat(EStatType.Knowledge, 5);
            SetPlayerStat(EStatType.Wit, 10);
        }
        else if (jobNum == 1)
        {
            player.playerjob = "프로그래머";
            SetPlayerStat(EStatType.Sencetive, 5);
            SetPlayerStat(EStatType.Knowledge, 10);
            SetPlayerStat(EStatType.Wit, 5);
        }
        else if (jobNum == 2)
        {
            player.playerjob = "아티스트";
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
        }

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
}
