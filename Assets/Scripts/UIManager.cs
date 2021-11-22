using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text storyText = null;
    [SerializeField] private Text jobStatusText = null;
    [SerializeField] private Text statText = null;
    [SerializeField] private Text arrivalTimeText = null;
    [SerializeField] private GameObject touchScreen = null;
    [SerializeField] private CanvasGroup nicknameInputPanal = null;
    [SerializeField] private SeletingBtnBase[] selectBtns;
    [SerializeField] private Sprite[] selectBtnSprites;
    [SerializeField] private SeletingBtnBase finalSelectBtn = null;
    [SerializeField] private MoveAnimScene MoveAnimScenePanal = null;
    [SerializeField] private GameObject points = null;
    [SerializeField] private GameObject pointPrefab = null;
    [SerializeField] private Sprite[] pointSprites = null;
    private int currentPlayerPos = 0;

    private InputField nicknameInputField = null;
    [SerializeField] private Text timerTimeText = null;


    private bool isWriting = false;
    private bool isSkip = false;
    private float currentWriteSpeed = 0f;

    private void Awake()
    {
        
        nicknameInputField = nicknameInputPanal.GetComponentInChildren<InputField>();
        nicknameInputField.onEndEdit.AddListener(_ =>
        {
            OnClickNickNameInput();
        });
    }
    private void Start()
    {
        CreatePoints();
    }

    public void StartWrite(string message, Action action = null, float writeSpeed = 0.03f)
    {
        StartCoroutine(WriteText(message, action, writeSpeed));
    }

    public void StartWrite<T>(string message, Action<T> action, T param, float writeSpeed = 0.03f)
    {
        StartCoroutine(WriteText(message, action, param, writeSpeed));
    }

    public IEnumerator WriteText(string message, Action action, float writeSpeed)
    {
        isWriting = true;
        ActiveTouchScreen(true);
        currentWriteSpeed = writeSpeed;
        float waitTime = currentWriteSpeed * 2f;
        string messageText = "";

        if (isSkip) isSkip = false;

        yield return new WaitForSeconds(0.05f);
        message = message.Replace("&", GameManager.Inst.CurrentPlayer.nickname);
        message = message.Replace("*", GameManager.Inst.CurrentPlayer.playerjob);

        foreach (var c in message)
        {
            if (isSkip) break;

            messageText = string.Format("{0}{1}", messageText, c);
            storyText.text = messageText;
            yield return new WaitForSeconds(currentWriteSpeed);
            if (c == '\n')
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (!isSkip)
        {
            yield return new WaitForSeconds(1.25f);
        }
        else
        {
            storyText.text = message;
            yield return new WaitForSeconds(0.5f);
            isSkip = false;
        }

        if (action == null)
        {
            ShowSelectBtn();
        }

        else
        {
            action();
        }

        isWriting = false;
    }

    public IEnumerator WriteText<T>(string message, Action<T> action, T param, float writeSpeed)
    {
        isWriting = true;
        ActiveTouchScreen(true);
        currentWriteSpeed = writeSpeed;
        float waitTime = currentWriteSpeed * 2f;
        string messageText = "";

        if (isSkip) isSkip = false;

        yield return new WaitForSeconds(0.05f);

        message = message.Replace("&", GameManager.Inst.CurrentPlayer.nickname);
        message = message.Replace("*", GameManager.Inst.CurrentPlayer.playerjob);

        foreach (var c in message)
        {
            if (isSkip) break;

            messageText = string.Format("{0}{1}", messageText, c);
            storyText.text = messageText;
            yield return new WaitForSeconds(currentWriteSpeed);
            if (c == '\n')
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (!isSkip)
        {
            yield return new WaitForSeconds(1.25f);
        }
        else
        {
            storyText.text = message;
            yield return new WaitForSeconds(0.5f);
            isSkip = false;
        }

        if (action == null)
        {
            ShowSelectBtn();
        }

        else
        {
            action(param);
        }

        isWriting = false;
    }

    public void ActiveNameInputField(bool isActive)
    {
        if (isActive)
        {
            nicknameInputPanal.gameObject.SetActive(true);
            nicknameInputPanal.DOFade(1f, 1f);
            GameManager.Inst.Story.EndStory();
        }

        else
        {
            nicknameInputPanal.DOFade(0f, 1f);
            nicknameInputPanal.gameObject.SetActive(false);
        }


    }

    public void OnClickNickNameInput()
    {
        if (nicknameInputField.text == "") return;

        GameManager.Inst.CurrentPlayer.nickname = nicknameInputField.text;
        ActiveNameInputField(false);

        GameManager.Inst.Story.SetStoryNum();
        GameManager.Inst.Story.StartSceneStory();
    }

    public void ShowSelectBtn()
    {
        SelectLine[] selectLines = GameManager.Inst.Story.GetNowStory().selectLines;

        for (int i = 0; i < selectLines.Length; i++)
        {
            selectBtns[i].ActiveBtn(true);
            selectBtns[i].SettingBtn(selectLines[i], i);
        }
    }

    public void MoveAnimScene()
    {
        MoveAnimScenePanal.gameObject.SetActive(true);
        MoveAnimScenePanal.StartMoveAnim();
    }

    public void ShowSingleSelectBtn()
    {
        int num;
        if (GameManager.Inst.CurrentPlayer.playerjob == "아티스트")
        {
            num = 1;
        }
        else
        {
            num = 0;
        }

        SelectLine selectLine = GameManager.Inst.Story.GetNowStory().selectLines[num];

        selectBtns[0].ActiveBtn(true);
        selectBtns[0].SettingBtn(selectLine);
    }

    public void UnShowSelectBtn(SeletingBtnBase seletingBtn = null)
    {
        for (int i = 0; i < 3; i++)
        {
            if (seletingBtn == selectBtns[i]) continue;
            selectBtns[i].ActiveBtn(false);
        }
    }

    public void ActiveFinalSelectBtn()
    {
        for (int i = 0; i < 3; i++)
        {
            if (selectBtns[i].SelectType == ESelectType.Final)
            {
                selectBtns[i].ActiveBtn(true);

                GameManager.Inst.Story.EndStory();
                return;
            }
        }
    }

    public void OnClickTouchScreen()
    {
        if (!GameManager.Inst.Story.IsEndStory && isWriting)
        {
            isSkip = true;
            ActiveTouchScreen(false);
        }
        else
        {
            return;
        }
    }

    public void ActiveTouchScreen(bool isActive)
    {
        touchScreen.SetActive(isActive);
    }

    public void SetEventToSelectBtn(bool isRemove)
    {
        for (int i = 0; i < 3; i++)
        {
            selectBtns[i].SetEvent(selectBtns[i].SetPlayerJob, isRemove);
        }
    }

    public void ChangeSelectBtnSprite(Image image, ESelectType type)
    {
        int num = (int)type;
        image.sprite = selectBtnSprites[num];
    }

    public void SetJobText()
    {
        jobStatusText.text = GameManager.Inst.CurrentPlayer.playerjob;
    }

    public void SetStatText()
    {
        int wit = GameManager.Inst.CurrentPlayer.stat_Wit;
        int knowledge = GameManager.Inst.CurrentPlayer.stat_Knowledge;
        int sencetive = GameManager.Inst.CurrentPlayer.stat_Sencetive;
        int arrivalTime = GameManager.Inst.CurrentPlayer.arrivalTime;

        statText.text = string.Format("재치: {0} / 섬세: {1} / 지식: {2}", wit, sencetive, knowledge);
        arrivalTimeText.text = string.Format("도착 시간: {0}{1}분", arrivalTime >= 0 ? "+" : "-", arrivalTime);

    }

    public void SetTime(int add)
    {
        Debug.Log("setTime: " + add);
        if(GameManager.Inst.Timer.minute + add >= 6)
        {
            GameManager.Inst.Timer.minute = add - (6 - GameManager.Inst.Timer.minute);
            GameManager.Inst.Timer.hour++;
        }
        else if(GameManager.Inst.Timer.minute + add < 0)
        {
            GameManager.Inst.Timer.minute = (6 + GameManager.Inst.Timer.minute) + add;
            GameManager.Inst.Timer.hour--;
        }
        else
        {
            GameManager.Inst.Timer.minute = GameManager.Inst.Timer.minute + add;
        }

        SetTimerUI();
    }
    public void SetTimerUI()
    {
        timerTimeText.text = string.Format("{00}:{1}0", GameManager.Inst.Timer.hour,GameManager.Inst.Timer.minute);
    }
    public void CreatePoints()
    {
        for (int i = 0; i < GameManager.Inst.Story.GetStoryLine().storyOrder.Length; i++)
        {
            GameObject newPoint = Instantiate(pointPrefab, points.transform);
            Image image = newPoint.GetComponent<Image>();
            if (i == 0) newPoint.transform.GetChild(0).gameObject.SetActive(true);
            image.sprite = pointSprites[(int)GameManager.Inst.Story.GetStoryLine().storyOrder[i]];
            newPoint.SetActive(true);
        }
        GameObject lastPoint = Instantiate(pointPrefab, points.transform);
        Image lastimage = lastPoint.GetComponent<Image>();
        lastimage.sprite = pointSprites[(int)GameManager.Inst.Story.GetStoryLine().storyOrder.Length];
        lastPoint.SetActive(true);
    }

    public void CheckPlayerPoint()
    {
        if (points.transform.GetChild(currentPlayerPos+1) == null) return;
        points.transform.GetChild(currentPlayerPos).GetChild(0).gameObject.SetActive(false);
        currentPlayerPos++;
        points.transform.GetChild(currentPlayerPos).GetChild(0).gameObject.SetActive(true);
    }

    #region Sound Setting

    public void BGMVolume(float value)
    {
        SoundManager.Inst.BGMVolume(value);
    }

    public void BGMMute(bool isMute)
    {
        SoundManager.Inst.BGMMute(isMute);
    }

    public void EffectMute(bool isMute)
    {
        SoundManager.Inst.EffectMute(isMute);
    }

    public void EffectVolume(float value)
    {
        SoundManager.Inst.EffectVolume(value);
    }
    public void SetBGM(int bgmNum)
    {
        SoundManager.Inst.SetBGM(bgmNum);
    }
    public void SetEffectSound(int effectNum)
    {
        SoundManager.Inst.SetEffectSound(effectNum);
    }

    #endregion
}