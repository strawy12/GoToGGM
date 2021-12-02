using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image timeLimiter = null;
    [SerializeField] private StoryText storyTextTemp = null;
    [SerializeField] private Image image = null;

    [SerializeField] private Text statText = null;
    [SerializeField] private Text arrivalTimeText = null;
    [SerializeField] private GameObject touchScreen = null;
    [SerializeField] private CanvasGroup nicknameInputPanal = null;
    [SerializeField] private SeletingBtnBase[] selectBtns;
    [SerializeField] private MoveAnimScene MoveAnimScenePanal = null;
    [SerializeField] private GameObject points = null;
    [SerializeField] private GameObject pointPrefab = null;
    [SerializeField] private Image backgroundImage = null;
    [SerializeField] private Image darkBattlePanal = null;
    [SerializeField] private RectTransform hudObjects = null;
    [SerializeField] private RectTransform messagePanal = null;


    private StoryScrollRect storyScrollRect = null;

    private Text jobStatusText = null;


    private int currentPlayerPos = 0;


    private Sprite[] backGroundArray = null;
    private Sprite[] selectBtnSprites;
    private Sprite[] pointSprites = null;

    private List<Transform> particlePosArray = new List<Transform>();
    private Text messageText = null;
    private List<StoryText> storyTextList = new List<StoryText>();

    private InputField nicknameInputField = null;
    [SerializeField] private Text timerTimeText = null;


    private int fontSize = 72;
    private bool isWriting = false;
    private bool isSkip = false;
    private bool currentUsedEffect = false;
    public bool isSelected = false;

    private void Awake()
    {
        messageText = messagePanal.transform.GetChild(0).GetComponent<Text>();
        nicknameInputField = nicknameInputPanal.GetComponentInChildren<InputField>();
        storyScrollRect = storyTextTemp.transform.GetComponentInParent<StoryScrollRect>();
        jobStatusText = storyScrollRect.transform.GetChild(1).GetComponent<Text>();
        particlePosArray.Add(storyScrollRect.transform.GetChild(6));
        particlePosArray.Add(storyScrollRect.transform.GetChild(7));
        particlePosArray.Add(storyScrollRect.transform.GetChild(8));
        particlePosArray.Add(hudObjects.transform.GetChild(0));

        selectBtnSprites = Resources.LoadAll<Sprite>("SelectBtns");
        backGroundArray = Resources.LoadAll<Sprite>("backGrounds");
    }
    private void Start()
    {
        //CreatePoints();
        timeLimiter.rectTransform.localScale = new Vector2(timeLimiter.rectTransform.localScale.x, 0f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DarkBattleEffect(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DarkBattleEffect(false);
        }
    }

    public void StartWrite(string message, bool usedEffect = false, Action action = null)
    {
        if (action == null)
        {
            SettingSelectBtn();
        }
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

        yield return CheckEffect(0);

        foreach (var c in message)
        {
            if (isSkip) break;

            messageText = string.Format("{0}{1}", messageText, c);
            storyText.text = messageText;
            storyText.CheckTextSize();
            yield return new WaitForSeconds(0.03f);

            if (c == '\n')
            {
                cnt++;

                if (cnt == 2)
                {
                    cnt = 0;
                    storyCnt++;
                    yield return DelayEnter(cnt, storyCnt);
                }

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

        if (isSkip) isSkip = false;

        message = ReplaceMessage(message);

        yield return WriteStoryText(message);

        yield return LastCheckEffect(message);

        yield return new WaitForSeconds(isSkip ? 0.5f : 1f);

        action(param);

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
        storyText.SetFontSize(fontSize);
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
        storyText.SetFontSize(fontSize);
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
        for (int i = 0; i < cnt; i++)
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
        isSelected = false;
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
        timeLimiter.gameObject.SetActive(true);
        timeLimiter.rectTransform.DOScaleY(1f, 0.5f);
        while (currentTime > 0)//TimerLimit의 1 프레임마다 time 동안 x scale 줄이기
        {
            currentTime -= Time.deltaTime;
            timeLimiter.rectTransform.localScale = new Vector2(currentTime / time * scaleX, timeLimiter.rectTransform.localScale.y);
            if (isSelected)
            {
                ResetTimeLimiter(scaleX);
                yield break;
            }
            yield return null;
        }
        ResetTimeLimiter(scaleX);
        selectBtns[btnNum].ActiveBtn(true);
        selectBtns[btnNum].OnClickBtn();
        timeLimiter.gameObject.SetActive(false);
    }
    private void ResetTimeLimiter(float scaleX)
    {
        timeLimiter.rectTransform.localScale = Vector2.one;
        timeLimiter.gameObject.SetActive(false);
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
        if (isWriting)
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

    public void ActiveQuitPanal(bool isActive)
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

        statText.text = string.Format("재치: {0} / 섬세: {1} / 지식: {2}", wit, sencetive, knowledge);

    }

    public void SetNowTimeText()
    {
        int nowTime = GameManager.Inst.CurrentPlayer.GetArrivalTime();
        int hour = nowTime / 60;
        nowTime -= hour * 60;

        arrivalTimeText.text = string.Format("현재 시각\n{0:00} : {1:00}", hour, nowTime); // 숫자 두자리 고정
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

    public void ShowArriveTimeDangerousMessage()
    {
        int arrivalTime = 540 + GameManager.Inst.CurrentPlayer.arrivalTime;
        string lastWord = GameManager.Inst.CurrentPlayer.GetLastWord();
        bool isLating = GameManager.Inst.CurrentPlayer.arrivalTime < 0;

        int hour = arrivalTime / 60;
        arrivalTime -= hour * 60;

        string message = string.Format("<color=#000000> 현재 도착 예정 시간\n{0:00} : {1:00}</color>\n\n{2}", hour, arrivalTime, lastWord);

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

    public void DarkBattleEffect(bool isTrue)
    {
        image.transform.DOScaleY(isTrue ? 1f : 0f, 0.5f);
        //darkBattlePanal.DOFade(0f, 0f);
        //darkBattlePanal.gameObject.SetActive(true);
        //darkBattlePanal.DOFade(1f, 1f);
    }

    public void OnClickDataResetBtn()
    {
        GameManager.Inst.DataReset();
    }

    public void ChangeBackGround(int backGroundNum)
    {
        backgroundImage.sprite = backGroundArray[backGroundNum];
    }

    public float PlayEffect(int effectNum)
    {
        Transform target;
        switch (effectNum)
        {
            case 0: // Shake
                ShakeEffect();
                return 1f;

            case 1: // Flame
                target = particlePosArray[0];
                GameManager.Inst.Particle.PlayParticle(0, 1.2f, target);
                return 1f;

            case 2: // Slash
                target = particlePosArray[1];
                GameManager.Inst.Particle.PlayParticle(1, 1.2f, target);
                return 1f;

            case 3: // Wind
                target = particlePosArray[0];
                GameManager.Inst.Particle.PlayParticle(2, 1.2f, target);
                break;

            case 4: // Falling
                target = particlePosArray[3];
                GameManager.Inst.Particle.PlayParticle(5, 1.2f, target);
                break;

            case 5: // Particle_LastFall
                target = particlePosArray[3];
                GameManager.Inst.Particle.PlayParticle(5, 1.2f, target);
                //Invoke("LastFall", 1.2f);
                break;

            case 6: // Drawing
                target = particlePosArray[2];
                GameManager.Inst.Particle.PlayParticle(6, 1.2f, target);
                break;

            case 7:
                //DarkBattleEffect();
                break;
        }
        return 0f;

    }


    public void SetFontSize(float value)
    {
        fontSize = (int)value;

        foreach (var storyText in storyTextList)
        {
            storyText.SetFontSize(fontSize);
        }
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

    public float GetEffectSoundLength()
    {
        return SoundManager.Inst.GetEffectSoundLength();
    }

    #endregion
}