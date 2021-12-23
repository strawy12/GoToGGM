using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SeletingBtnBase : MonoBehaviour
{
    [SerializeField] protected Text seletingText;

    protected CanvasGroup canvasGroup;
    protected Image btnImage;
    protected SelectLine currentSelectLine;
    protected EventStory currentEventStory;
    private ESelectType currentSelectState;
    protected Button currentButton = null;

    public ESelectType SelectType { get { return currentSelectState; } }

    protected int btnNum = 0;

    public void Awake()
    {
        currentButton = GetComponent<Button>();
        btnImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }


    public void SettingBtn(SelectLine selectLine, int num = 0)
    {
        currentSelectLine = selectLine;
        currentSelectState = selectLine.selectType;
        btnNum = num;

        if (currentSelectState == ESelectType.Event)
        {
            int additionNum = 0;
            switch (DataManager.Inst.CurrentPlayer.playerjob)
            {
                case "기획자":
                    additionNum = 0;
                    break;

                case "프로그래머":
                    additionNum = 1;
                    break;

                case "그래픽 아티스트":
                    additionNum = 2;
                    break;
            }
            currentSelectLine.eventStoryID += additionNum;
        }

        if (currentSelectState == ESelectType.Gread)
        {
            int randNum = Random.Range(1, 11);
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID, true, randNum <= selectLine.percent);
        }

        else if (currentSelectState == ESelectType.Final)
        {
            GameManager.Inst.Story.EndStory();
            currentEventStory = null;
        }
        
        else
        {
            currentEventStory = GameManager.Inst.Story.GetEventStory(currentSelectLine.eventStoryID);
        }

        if (currentSelectState == ESelectType.Special || currentSelectState == ESelectType.Event)
        {
            currentButton.interactable = GameManager.Inst.CheckPlayerStat(currentSelectLine.needStatType, currentSelectLine.needStat);
        }
        else
        {
            currentButton.interactable = true;
        }

        SetBtnState();

    }

    public void CheckInfo()
    {
        if (currentEventStory == null) return;

        if (currentEventStory.ExistIncreaseStats)
        {
            IncreaseStats[] increaseStats = currentEventStory.increaseStats;
           foreach(var increaseStat in increaseStats)
            {
                GameManager.Inst.SetPlayerStat(increaseStat.increaseStatType, increaseStat.increaseStat);
            }
        }
        if (currentEventStory.increaseTime != 0)
        {
            GameManager.Inst.UI.SetTime(currentEventStory.increaseTime);
        }
    }

    public void SetBtnState()
    {
        seletingText.text = currentSelectLine.selectLine;
        GameManager.Inst.UI.ChangeSelectBtnSprite(btnImage, currentSelectState);
    }
    public void OnClickBtn()
    {
        GameManager.Inst.UI.isSelected = true;
        currentButton.interactable = false;

        if (currentSelectState == ESelectType.Final)
        {
            if (GameManager.Inst.Story.isEndding)
            {
                GameManager.Inst.UI.UnShowSelectBtn();
                DataManager.Inst.InGameDataReset();
                GameManager.Inst.UI.SetBGM(11);
                GameManager.Inst.UI.DarkBattleEffect(true, true);
                return;
            }

            CheckInfo();

            GameManager.Inst.UI.UnShowSelectBtn();
            GameManager.Inst.Story.SetStoryNum();

            if (GameManager.Inst.Story.IsEndScenario ||
                (currentEventStory != null && currentEventStory.ExistIncreaseStat_ArrivalTime))
            {
                GameManager.Inst.Story.StartSceneStory(4f);
            }
            else
            {
                GameManager.Inst.Story.StartSceneStory();
            }



        }

        else
        {
            if (currentSelectState == ESelectType.Gread)
            {
                GameManager.Inst.CheckLucky(currentEventStory.isSuccess);
            }

            if(currentSelectState == ESelectType.Event)
            {
                GameManager.Inst.Story.EnterEventStory();
            }

            if (currentEventStory.usedEffect)
            {
                GameManager.Inst.Story.SetNowEffectSettings(currentEventStory.effectSettings);
            }

            string storyLine = currentEventStory.eventMainStory;
            GameManager.Inst.UI.StartWrite(storyLine, GameManager.Inst.UI.ActiveFinalSelectBtn, btnNum, currentEventStory.usedEffect);
            GameManager.Inst.UI.UnShowSelectBtn();

            ChangeFinalSelect();
        }

    }

    public void ChangeFinalSelect()
    {
        int index = 0;
        if (currentEventStory.eventFinalStory.Length > 1)
        {
            index = GameManager.Inst.StoryLine;
        }
        seletingText.text = currentEventStory.eventFinalStory[index];
        currentSelectState = ESelectType.Final;
        GameManager.Inst.UI.ChangeSelectBtnSprite(btnImage, currentSelectState);
    }



    public void SetEvent(Action action, bool isRemove)
    {
        if (isRemove)
        {
            currentButton.onClick.RemoveAllListeners();
        }
        else
        {
            currentButton.onClick.AddListener(() => action());
        }
    }

    public void ActiveBtn(bool isActive)
    {
        if (isActive)
        {
            if (currentSelectState != ESelectType.Special && currentSelectState != ESelectType.Event)
            {
                currentButton.interactable = true;
            }
            gameObject.SetActive(true);
            canvasGroup.DOFade(1f, 1f);
        }
        else
        {
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

    }

    public void ChangeLockImage()
    {

    }

    public void SetPlayerJob()
    {
        GameManager.Inst.SetPlayerJob(btnNum);
    }
}
