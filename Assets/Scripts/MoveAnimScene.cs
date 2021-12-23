using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveAnimScene : MonoBehaviour
{
    [SerializeField] RectTransform playerobj;
    [SerializeField] RectTransform stageObjTemp;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Text currentStageText;
    [SerializeField] Transform particlePos;
    
    RectTransform[] rects;
    Queue<RectTransform> rectPoolQueue = new Queue<RectTransform>();
    private bool isFirst = true;

    public void StartMoveAnim()
    {
        if(GameManager.Inst.Story.isEndding)
        {
            GameManager.Inst.Story.StartStory();
            gameObject.SetActive(false);
            return;
        }

        isFirst = DataManager.Inst.CurrentPlayer.crtStoryNum == 0 || stageObjTemp.parent.childCount <= 1;

        if (rects != null && isFirst)
        {
            currentStageText.text = "";
            for (int i = 0; i < rects.Length; i++)
            {
                rectPoolQueue.Enqueue(rects[i]);
                rects[i].gameObject.SetActive(false);
            }
            playerobj.gameObject.SetActive(false);

        }

        canvasGroup.DOFade(1f, 1f);
        StartCoroutine(SpawnStage());
    }

    IEnumerator SpawnStage()
    {
        yield return new WaitForSeconds(0.5f);

        Scenario scenario = GameManager.Inst.Story.GetNowScenario();

        if(scenario.scenarioName != currentStageText.text)
        {
            currentStageText.text = scenario.scenarioName;
            currentStageText.DOFade(0f, 0f);

            currentStageText.DOFade(1f, 0.7f);

            yield return new WaitForSeconds(0.7f);
        }



        if (isFirst)
        {

            int objCnt = scenario.stories.Length;
            float[] objLerps = new float[objCnt];
            float interval = 1f / (objCnt - 1);

            rects = new RectTransform[objCnt];

            for (int i = 0; i < objCnt; i++)
            {
                objLerps[i] = interval * i;
                if(rectPoolQueue.Count != 0)
                {
                    rects[i] = rectPoolQueue.Dequeue();
                }
                else
                {
                    rects[i] = Instantiate(stageObjTemp, stageObjTemp.transform.parent);
                }
            }

            for (int i = 0; i < objCnt; i++)
            {
                float targetPosX = Mathf.Lerp(-550f, 550f, objLerps[i]);

                Vector2 targetPos = rects[i].anchoredPosition;
                targetPos.x = targetPosX;
                rects[i].anchoredPosition = targetPos;
                rects[i].localScale = Vector3.zero;
                rects[i].gameObject.SetActive(true);
                rects[i].DOScale(Vector3.one, 0.3f);

                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1f);

        }

        if(isFirst)
        {
            playerobj.anchoredPosition = rects[0].anchoredPosition;
            playerobj.localScale = Vector3.zero;
            playerobj.gameObject.SetActive(true);
            playerobj.DOScale(Vector3.one, 0.5f);

            GameManager.Inst.Particle.PlayParticle(4, 2f, playerobj.transform);
            yield return new WaitForSeconds(0.8f);
        }

        else
        {
            GameManager.Inst.Particle.PlayParticle(4, 2f, playerobj.transform);
        }


        playerobj.DOAnchorPosX(rects[DataManager.Inst.CurrentPlayer.crtStoryNum].anchoredPosition.x, 1f);

        yield return new WaitForSeconds(1.3f);
        canvasGroup.DOFade(0f, 1f);
        GameManager.Inst.Story.StartStory();

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}

