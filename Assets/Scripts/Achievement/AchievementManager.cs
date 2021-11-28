using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AchievementBase
{
    public string title;
    public string explanation;
    public bool isCleared;
    public int ID;
    public AchievementPanel achievementPanel;
}
public class AchievementManager : MonoBehaviour
{
    [SerializeField] GameObject panelObject = null;
    [SerializeField] Transform contentTransform = null;
    [SerializeField] ClearNoticeScript clearNotice = null;
    public List<AchievementBase> achievementPanels = new List<AchievementBase>();
    private AchievementPanel component = null;
    private int testID = 0;
    private int luckPoint = 0;
    private void Start()
    {
        for(int i = 0; i < 15; i++)
        {
            GameObject newObject = Instantiate(panelObject, contentTransform);
            achievementPanels[i].achievementPanel = newObject.GetComponent<AchievementPanel>();
            achievementPanels[i].achievementPanel.SetValue(achievementPanels[i].title, achievementPanels[i].explanation);
            newObject.SetActive(true);

            //achievementPanels[i].isCleared = GameManager.Inst.CurrentPlayer.clears[i];
            //if (achievementPanels[i].isCleared)
            //{
            //    achievementPanels[i].achievementPanel.Clear(i);
            //}
        }
    }
    private void Clear(int ID)
    {
        if (achievementPanels[ID].isCleared) return;
        achievementPanels[ID].isCleared = true;

        achievementPanels[ID].achievementPanel.Clear(ID);

        clearNotice.ShowNotice(achievementPanels[ID].title);

        //GameManager.Inst.SaveClears(ID);
    }
    public void Test()//실험용 클리어함수
    {
        Debug.Log(testID);
        Clear(testID);
        testID+=2;
    }
    public void ClearEnding(int endingID)
    {
        Clear(endingID);
        CheckEndingCollector();
    }

    public void CheckEndingCollector()
    {
        bool isCleared = true;
        for (int i = 0; i < 10; i++)
        {
            if (!achievementPanels[i].isCleared)
                isCleared = false;
        }
        if (isCleared)
        {
            Clear(9);
            CheckPerfectClear();
        }
    }

    public void CheckPerfectClear()
    {
        bool isCleared = true;
        foreach(AchievementBase achievement in achievementPanels)
        {
            if(!achievement.isCleared)
            {
                isCleared = false;
            }
        }
        if(isCleared) Clear(14);
    }

    public void CheckMacGyver()
    {
        if (achievementPanels[12].isCleared)
        {
            CheckWeaponMaster();
            return;
        }
        if(GameManager.Inst.CurrentPlayer.stat_Knowledge + GameManager.Inst.CurrentPlayer.stat_Sencetive + GameManager.Inst.CurrentPlayer.stat_Wit >= 30)
        {
            Clear(12);
        }
    }

    public void CheckWeaponMaster()
    {
        if (GameManager.Inst.CurrentPlayer.stat_Knowledge + GameManager.Inst.CurrentPlayer.stat_Sencetive + GameManager.Inst.CurrentPlayer.stat_Wit >= 45)
        {
            Clear(13);
        }
    }

    public void CheckLucky()
    {
        if(luckPoint <= 0)
        {
            luckPoint = 1;
        }
        else
        {
            Clear(10);
        }
    }
    public void CheckUnlucky()
    {
        if (luckPoint >= 0)
        {
            luckPoint = -1;
        }
        else
        {
            Clear(11);
        }
    }
}