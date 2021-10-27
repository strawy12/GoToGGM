using System.Collections;using System;using System.Collections.Generic;using UnityEngine;using Random = UnityEngine.Random;using UnityEngine.UI;public class SeletingBtnBase : MonoBehaviour{    [SerializeField] protected Text seletingText;    [SerializeField] private Text btnStateText;    protected SelectLine currentSelectLine;    protected EventStory currentEventStory;    private ESelectType currentSelectState;    protected Button button = null;    protected int increaseStoryNum = 1;

    public void SettingBtn(SelectLine selectLine)    {        if (button == null)        {            button = GetComponent<Button>();        }        currentSelectLine = selectLine;        currentSelectState = selectLine.selectType;
        if (currentSelectState == ESelectType.Gread)
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID, true, Random.Range(0, 2) == 0);
        }        //else if(currentSelectState == ESelectType.Special)
        //{
        //    // ���� ���� üũ �ؼ� �� ��Ű�� �ڵ� �ۼ� �ϱ�
        //}        else
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID);        }        SetBtnState();        CheckInfo();    }    public void CheckInfo()
    {
        if (currentEventStory.increaseStatType != EStatType.None)
        {
            // ���� ���� ã�Ƽ� ���� �߰��ϴ� �ڵ� �ۼ����ּ��� ����
        }
        if(currentEventStory.skipRouteCnt != 0)
        {
            increaseStoryNum = currentEventStory.skipRouteCnt;
        }
    }    public void SetBtnState()
    {
        int endStringCnt = 1;

        if (currentSelectLine.selectType == ESelectType.Special)
        {
            endStringCnt = 2;
        }

        seletingText.text = currentSelectLine.selectLine;        btnStateText.text = currentSelectLine.selectType.ToString().Substring(0, endStringCnt);    }    public void OnClickBtn()    {        string storyLine = currentEventStory.eventMainStory;        GameManager.Inst.UI.StartWrite(storyLine, FinallyMention);        GameManager.Inst.UI.UnShowSelectBtn(this);    }    public void FinallyMention()
    {
        string storyLine = currentEventStory.eventStory;
        GameManager.Inst.UI.StartWrite(storyLine, GameManager.Inst.Story.EndStory);        GameManager.Inst.Story.SetStoryNum(increaseStoryNum);    }}