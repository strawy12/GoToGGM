using System.Collections;

[System.Serializable]
public class Player
{
    public string playerjob;
    public int stat_Sencetive;
    public int stat_Knowledge;
    public int stat_Wit;

    public Player(string playerjob, int stat_Sencetive, int stat_Knowledge, int stat_Wit)
    {
        this.playerjob = playerjob;
        this.stat_Sencetive = stat_Sencetive;
        this.stat_Knowledge = stat_Knowledge;
        this.stat_Wit = stat_Wit;
    }
}
