using System.Collections;

[System.Serializable]
public class Player
{
    //Player Info
    public string nickname;
    public string playerjob;

    //Story Info
    public int storyLineNum = 0; 
    public int crtScenarioCnt = 0;
    public int crtEventStoryCnt = 0;
    public int crtStoryNum = 0;

    //InGame Stat
    public int stat_Sencetive;
    public int stat_Knowledge;
    public int stat_Wit;
    public int arrivalTime;
    public bool[] clears;


    //InGame Sound Info
    public float bgmVolume;
    public float effectVolume;
    public bool bgmMute;
    public bool effectMute;

    public Player(string playerjob, int stat_Sencetive, int stat_Knowledge, int stat_Wit)
    {
        this.playerjob = playerjob;
        this.stat_Sencetive = stat_Sencetive;
        this.stat_Knowledge = stat_Knowledge;
        this.stat_Wit = stat_Wit;
        clears = new bool[15];
    }

    public string GetLastWord()
    {
        if(arrivalTime < 0)
        {
            return "오? 예상보다 빨리 도착하네??";
        }

        else if(arrivalTime == 0)
        {
            return "다행이다. 늦진 않겠다.";
        }

        else
        {
            return "이러다간 지각하겠어!!";
        }
    }
}
