using System.Collections;
using System.Collections.Generic;
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
}



[System.Serializable]
public class Story
{
    public string storyName;
    public int storyNum;
    [TextArea(7, 10)] public string mainStory;
    public SelectLine[] selectLines;
}


[CreateAssetMenu(fileName = "Stories", menuName = "Sprictable Object/Stories")]
public class Stories : ScriptableObject
{
    public Story[] stories;
}
