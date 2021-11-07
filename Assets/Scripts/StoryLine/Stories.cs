using UnityEngine;

[System.Serializable]
public class SelectLine
{
    public string selectLine;

    [Header("선택지")]
    public ESelectType selectType;

    [Header("필요 스탯")]
    public EStatType needStatType;
    public int needStat;

    [Header("이벤트 스토리")]
    public int eventStoryID;
}

[System.Serializable]
public class Story
{
    public string storyName;
    [TextArea(7, 10)] public string mainStory;
    public SelectLine[] selectLines;
}

[System.Serializable]
public class Scenario
{
    public string scenarioName;
    public Story[] stories;
}


[CreateAssetMenu(fileName = "Stories", menuName = "Sprictable Object/Stories")]
public class Stories : ScriptableObject
{
    public Scenario[] stories;
}
