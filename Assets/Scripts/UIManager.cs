using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image timeLimiter = null; private float limiterScaleY;
    [SerializeField] private StoryText storyTextTemp = null;
    [SerializeField] private StoryScrollRect storyScrollRect = null;
    [SerializeField] private Text jobStatusText = null;
    [SerializeField] private Text statText = null;
    [SerializeField] private Text arrivalTimeText = null;
    [SerializeField] private GameObject touchScreen = null;
    [SerializeField] private CanvasGroup nicknameInputPanal = null;
    [SerializeField] private SeletingBtnBase[] selectBtns;
    [SerializeField] private SeletingBtnBase finalSelectBtn = null;
    [SerializeField] private MoveAnimScene MoveAnimScenePanal = null;
    [SerializeField] private GameObject points = null;
    [SerializeField] private GameObject pointPrefab = null;
    [SerializeField] private Image backgroundImage = null;
    [SerializeField] private RectTransform hudObjects = null;
    [SerializeField] private RectTransform messagePanal = null;

    [SerializeField] private Sprite[] pointSprites = null;
    private int currentPlayerPos = 0;

    private Sprite[] backGroundArray = null;
    private Sprite[] selectBtnSprites;
    private Text messageText = null;
    private List<StoryText> storyTextList = new List<StoryText>();

    private InputField nicknameInputField = null;
    [SerializeField] private Text timerTimeText = null;

    private bool isWriting = false;
    private bool isSkip = false;
    private bool currentUsedEffect = false;

    private void Awake()
    {
        messageText = messagePanal.transform.GetChild(0).GetComponent<Text>();
        nicknameInputField = nicknameInputPanal.GetComponentInChildren<InputField>();
        nicknameInputField.onEndEdit.AddListener(_ =>
        {
            OnClickNickNameInput();
        });

        selectBtnSprites = Resources.LoadAll<Sprite>("SelectBtns");
        backGroundArray = Resources.LoadAll<Sprite>("backGrounds");
    }
    private void Start()
    {
        CreatePoints();
        limiterScaleY = timeLimiter.rectTransform.localScale.y;
        timeLimiter.rectTransform.localScale = new Vector2(timeLimiter.rectTransform.localScale.x, 0f);
    }

    public void StartWrite(string message, bool usedEffect = false, Action action = null)
    {
        SettingSelectBtn();
        currentUsedEffect = usedEffect;
        StartCoroutine(WriteText(message, action));
    }

    public void StartWrite<T>(string message, Action<T> action, T param, bool usedEffect = false)
    {
        SettingSelectBtn();
        currentUsedEffect = usedEffect;
        StartCoroutine(WriteText(message, action, param));
    }

    public IEnumerator CheckEffect(int storyCnt)
    {
        if (!currentUsedEffect) yield break;

        float delay = 0f;

        delay = GameManager.Inst.Story.CheckEffect(storyCnt);

        yield return new WaitForSeconds(delay);

    }

    public IEnumerator WriteStoryText(string message)
    {
        StoryText storyText = InstantiateStoryText();

        int cnt = 1;
        int storyCnt = 0;

        string messageText = "";

        foreach (var c in message)
        {
            if (isSkip) break;

            messageText = string.Format("{0}{1}", messageText, c);
            storyText.text = messageText;
            yield return new WaitForSeconds(0.03f);

            if (c == '\n')
            {
                cnt++;
                if (cnt == 2)
                {
                    cnt = 0;
                    storyCnt++;
                }

                yield return DelayEnter(cnt, storyCnt);
            }
        }

        if (isSkip)
        {
            storyText.text = message;
            isSkip = false;
        }
    }

    public IEnumerator DelayEnter(int cnt, int storyCnt)
    {
        if (currentUsedEffect)
        {
            yield return CheckEffect(storyCnt);
        }

        yield return new WaitForSeconds(0.3f);
    }

    public IEnumerator LastCheckEffect(string message)
    {
        if (!currentUsedEffect) yield break;

        int lastCnt = (int)((message.Split('\n').Length - 1) * 0.5f) + 1;

        yield return CheckEffect(lastCnt);
    }


    public IEnumerator WriteText(string message, Action action)
    {
        yield return new WaitForSeconds(0.05f);

        isWriting = true;
        ActiveTouchScreen(true);

        if (isSkip) isSkip = false;

        message = ReplaceMessage(message);

        yield return CheckEffect(0);

        yield return WriteStoryText(message);

        yield return LastCheckEffect(message);

        yield return new WaitForSeconds(isSkip ? 0.5f : 1f);


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

    public IEnumerator WriteText<T>(string message, Action<T> action, T param)
    {
        yield return new WaitForSeconds(0.05f);

        isWriting = true;
        ActiveTouchScreen(true);
        StoryText storyText = InstantiateStoryText();

        if (isSkip) isSkip = false;

        message = ReplaceMessage(message);

        yield return CheckEffect(0);

        yield return WriteStoryText(message);

        yield return LastCheckEffect(message);

        yield return new WaitForSeconds(isSkip ? 0.5f : 1f);

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

    public void ShakeEffect()
    {
        hudObjects.DOShakeAnchorPos(0.5f, 300);
    }

    public StoryText InstantiateStoryText()
    {

        StoryText storyText = Instantiate(storyTextTemp, storyTextTemp.transform.parent);

        if (storyTextList.Count != 0)
        {
            int index = storyTextList.Count - 1;
            storyTextList[index].NextText();
        }

        storyTextList.Add(storyText);
        storyText.text = "";
        storyText.gameObject.SetActive(true);
        storyScrollRect.SetContentPos();

        return storyText;
    }

    public StoryText InstantiateStoryText(string message)
    {

        StoryText storyText = Instantiate(storyTextTemp, storyTextTemp.transform.parent);

        message = ReplaceMessage(message);

        storyText.text = message;

        if (storyTextList.Count != 0)
        {
            int index = storyTextList.Count - 1;
            storyTextList[index].NextText();
        }

        storyTextList.Add(storyText);

        storyText.gameObject.SetActive(true);
        storyScrollRect.SetContentPos();

        return storyText;
    }

    public string ReplaceMessage(string message)
    {
        message = message.Replace("&", GameManager.Inst.CurrentPlayer.nickname);
        message = message.Replace("*", GameManager.Inst.CurrentPlayer.playerjob);

        return message;
    }

    public void ResetStoryText()
    {
        int cnt = storyTextList.Count;
        for (int i = 0; i < storyTextList.Count; i++)
        {
            Destroy(storyTextList[i].gameObject);
        }

        storyTextList.Clear();
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

            if (selectLines[i].selectType != ESelectType.Hidden)
            {
                selectBtns[i].ActiveBtn(true);
            }

            else
            {
                StartCoroutine(ShowHiddenSelection(selectLines[i].timeLimit, i));
            }

        }
    }

    public void SettingSelectBtn()
    {
        SelectLine[] selectLines;
        if (GameManager.Inst.Story.isEndding)
        {
            int enddingNum = GameManager.Inst.CheckArrivalTime();
            selectLines = GameManager.Inst.Story.GetEnddingStory(enddingNum).selectLines;
        }

        else
        {
            selectLines = GameManager.Inst.Story.GetNowStory().selectLines;
        }


        for (int i = 0; i < selectLines.Length; i++)
        {

            selectBtns[i].SettingBtn(selectLines[i], i);
        }
    }
    public IEnumerator ShowHiddenSelection(float time, int btnNum)//0.5초동안 y scale 늘리기 애니메이션 실행 후, time만큼 x scale 줄이기 애니메이션 실행
    {
        float currentTime = time;
        float scaleX = timeLimiter.rectTransform.localScale.x;
        timeLimiter.rectTransform.DOScaleY(limiterScaleY, 0.5f);
        while (currentTime > 0)//TimerLimit의 1 프레임마다 time 동안 x scale 줄이기
        {
            currentTime -= Time.deltaTime;
            timeLimiter.rectTransform.localScale = new Vector2(currentTime / time * scaleX, timeLimiter.rectTransform.localScale.y);
            yield return null;
        }
        selectBtns[btnNum].ActiveBtn(true);
        selectBtns[btnNum].OnClickBtn();
    }
    public IEnumerator MoveAnimScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        MoveAnimScenePanal.gameObject.SetActive(true);
        MoveAnimScenePanal.StartMoveAnim();
    }

    public void ShowSingleSelectBtn(int num)
    {
        SelectLine selectLine = GameManager.Inst.Story.GetNowStory().selectLines[num];

        selectBtns[0].ActiveBtn(true);
        selectBtns[0].SettingBtn(selectLine);
    }

    public List<SeletingBtnBase> GetActiveSelectBtn()
    {
        List<SeletingBtnBase> seletingBtns = new List<SeletingBtnBase>();
        for (int i = 0; i < 3; i++)
        {
            if (selectBtns[i].gameObject.activeSelf)
            {
                seletingBtns.Add(selectBtns[i]);
            }
        }

        return seletingBtns;
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

        if (num == 4)
        {
            num = 0;
        }

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
        if (GameManager.Inst.Timer.minute + add >= 6)
        {
            GameManager.Inst.Timer.minute = add - (6 - GameManager.Inst.Timer.minute);
            GameManager.Inst.Timer.hour++;
        }
        else if (GameManager.Inst.Timer.minute + add < 0)
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
        timerTimeText.text = string.Format("{00}:{1}0", GameManager.Inst.Timer.hour, GameManager.Inst.Timer.minute);
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
        if (points.transform.GetChild(currentPlayerPos + 1) == null) return;
        points.transform.GetChild(currentPlayerPos).GetChild(0).gameObject.SetActive(false);
        currentPlayerPos++;
        points.transform.GetChild(currentPlayerPos).GetChild(0).gameObject.SetActive(true);
    }

    public void ShowMessagePanal(string message)
    {
        StartCoroutine(PlayMessage(message));
    }

    public void ShowArriveTimeDangerMessage(int arrivalTime, string lastWord, bool isLating)
    {
        string message = string.Format("현재 도착 예정 시간 : {0} {1}", arrivalTime, lastWord);

        StartCoroutine(PlayMessage(message, isLating ? Color.blue : Color.red));

    }

    public IEnumerator PlayMessage(string message)
    {
        messageText.text = message;

        messagePanal.gameObject.SetActive(true);
        messagePanal.DOScale(Vector3.one, 0.5f);

        yield return new WaitForSeconds(3f);

        messagePanal.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);

        messagePanal.gameObject.SetActive(false);
    }

    public IEnumerator PlayMessage(string message, Color color)
    {
        Color currentColor = messageText.color;
        messageText.color = color;
        messageText.text = message;

        messagePanal.gameObject.SetActive(true);
        messagePanal.DOScale(Vector3.one, 0.5f);

        yield return new WaitForSeconds(3f);

        messagePanal.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);

        messagePanal.gameObject.SetActive(false);
        messageText.color = currentColor;
    }

    public bool CheckWriting()
    {
        return isWriting;
    }

    public void StartEnddingCredit()
    {

    }

    public void ChangeBackGround(int backGroundNum)
    {
        backgroundImage.sprite = backGroundArray[backGroundNum];
    }

    public float PlayEffect(int effectNum)
    {
        switch (effectNum)
        {
            case 0:
                ShakeEffect();
                return 1f;

        }
        return 0f;

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