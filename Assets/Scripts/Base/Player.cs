using System.Collections;

[System.Serializable]
public class Player
{
    //Player Info
    public string nickname;
    public string playerjob;

    public int usedTimeCnt;

    public bool isTuto;

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

    public int nowTime;


    //InGame Sound Info
    public float bgmVolume;
    public float effectVolume;
    public bool bgmMute;
    public bool effectMute;

    public int fontSize;
    public float writeSpeed = 0.03f;
    public Player(string playerjob, int stat_Sencetive, int stat_Knowledge, int stat_Wit, float bgmVolume, float effectVolume, bool bgmMute, bool effectMute, int fontSize)
    {
        this.playerjob = playerjob;
        this.stat_Sencetive = stat_Sencetive;
        this.stat_Knowledge = stat_Knowledge;
        this.stat_Wit = stat_Wit;

        this.bgmVolume = bgmVolume;
        this.effectVolume = effectVolume;
        this.bgmMute = bgmMute;
        this.effectMute = effectMute;
        this.fontSize = fontSize;

        nowTime = 420;
        clears = new bool[15];
    }

    public int GetArrivalTime()
    {
        return nowTime + arrivalTime;
    }


    public string GetLastWord()
    {
        if (arrivalTime < 0)
        {
            return string.Format("���󺸴� {0}�� ���� �����ϰھ�!", arrivalTime * -1);
        }

        else if(arrivalTime == 0)
        {
            return "�� ���缭 �����ϰڴ°�!";
        }

        else
        {
            return string.Format("���󺸴� {0}�� �ʰ� �����ϰھ�..", arrivalTime);
        }
    }
}
