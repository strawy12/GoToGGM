using System.Collections;

    public void SettingBtn(SelectLine selectLine)

        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID, true, Random.Range(0, 2) == 0);
        }
        //{
        //    // ���� ���� üũ �ؼ� �� ��Ű�� �ڵ� �ۼ� �ϱ�
        //}
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID);
    {
        if (currentEventStory.increaseStatType != EStatType.None)
        {
            // ���� ���� ã�Ƽ� ���� �߰��ϴ� �ڵ� �ۼ����ּ��� ����
        }
        if(currentEventStory.skipRouteCnt != 0)
        {
            increaseStoryNum = currentEventStory.skipRouteCnt;
        }
    }
    {
        int endStringCnt = 1;

        if (currentSelectLine.selectType == ESelectType.Special)
        {
            endStringCnt = 2;
        }

        seletingText.text = currentSelectLine.selectLine;
    {
        string storyLine = currentEventStory.eventStory;
        GameManager.Inst.UI.StartWrite(storyLine, GameManager.Inst.Story.EndStory);