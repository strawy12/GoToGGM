using UnityEngine;

[System.Serializable]
public class SelectLine
{
    public string selectLine;

    [Header("������")]
    public ESelectType selectType;

    [Header("�ʿ� ����")]
    public EStatType needStatType;
    public int needStat;

    [Header("�̺�Ʈ ���丮")]
    public int eventStoryID;

    [Header("���� �ð�")]
    public float timeLimit;

    public bool useTimer;
    public float time;
}

[System.Serializable]
public class EffectSetting
{
    public EEffectType usedEffect;
    public int effectNum;

    public int playStoryOrder;
}


[System.Serializable]
public class Story
{
    [Header("Story Info")]
    public string storyName;
    public int storyID;
    public bool usedFunc;

    public bool usedEffect
    {
        get
        {
            return effectSettings.Length != 0;
        }
    }

    public EffectSetting[] effectSettings;

    [TextArea(7, 25)] public string mainStory;

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
    public Scenario[] scenarios;
}
