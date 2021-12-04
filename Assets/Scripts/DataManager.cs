using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoSingleTon<DataManager>
{
    [SerializeField] private int defaultFontsize = 72;
    [SerializeField] private float defaultSound = 0.5f;
    [SerializeField] private Player player;
    [SerializeField] private HelpTexts helpTexts = null;

    public Player CurrentPlayer { get { return player; } }
    string SAVE_PATH = "";
    string SAVE_FILE = "/SaveFile.txt";
    private void Awake()
    {

        DataManager[] dmanagers = FindObjectsOfType<DataManager>();
        if (dmanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        SAVE_PATH = Application.persistentDataPath + "/Save";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        LoadFromJson();
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
            player = new Player("고등학생", 0, 0, 0, defaultSound, defaultSound, false, false, defaultFontsize);
        }
        SaveToJson();
    }
    public void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(player, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, stringJson, System.Text.Encoding.UTF8);
    }
    public void DataReset()
    {
        player = new Player("고등학생", 0, 0, 0, defaultSound, defaultSound, false, false, defaultFontsize);
        SaveToJson();
        Application.Quit();
    }

    public void InGameDataReset()
    {
        player.stat_Knowledge = 0;
        player.stat_Sencetive = 0;
        player.stat_Wit = 0;
        player.arrivalTime = 0;
        player.nickname = "";
        player.playerjob = "";
        player.storyLineNum = 0;
        player.crtEventStoryCnt = 0;
        player.crtStoryNum = 0;
        player.crtScenarioCnt = 0;
        player.usedTimeCnt = 0;
        player.nowTime = 420;
    }

    public HelpTexts GetHelpTexts()
    {
        return helpTexts;
    }
}