using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public enum TutorialType
{
    Camera_Move,
    Report,
    Deadline,
    End,
    Max,
}

public class TutorialManager : Singleton
{
    const string KeyTutorial = "TutorialShowed_New";
    const float PosArrowY = 30f;

    public Canvas mainCanvas;

    public Image[] imgTutorials;

    public RectTransform rtArrow;
    public UnityAction funcClick;

    [Header("TextBox")]
    public GameObject objTextBox;
    public RectTransform rtText;
    public Text uiText;
    public TextTyper textTyper;

    public GameObject objClick;

    bool[] isTutorialShowed;
    Coroutine co_MovementArrow;
    public Coroutine co_CurrentTutorial;

    public void Awake()
    {
        rtArrow.SetActive(false);
        objTextBox.SetActive(false);
        objClick.SetActive(false);

        isTutorialShowed = new bool[(int)TutorialType.Max];
        for (int i = 0; i < (int)TutorialType.Max; i++)
        {
            bool isShowed = PlayerPrefs.GetInt($"{KeyTutorial}_{(TutorialType)i}", 0) == 1;
            isTutorialShowed[i] = isShowed;
        }

        Observer.onChangeLanguageType += ChangeLanguageType;
    }

    public void OnDestroy()
    {
        Observer.onChangeLanguageType -= ChangeLanguageType;
    }

    public void Play(TutorialType type)
    {
        if (co_CurrentTutorial != null)
            return;

        if (isTutorialShowed[(int)type])
            return;

        if (mainCanvas == null)
        {
            if (gameSM)
                mainCanvas = gameSM.GetComponent<Canvas>();
        }
        
        co_CurrentTutorial = StartCoroutine(PlayTutorial(type));
    }

    public IEnumerator PlayTutorial(TutorialType type)
    {

        isClicked = false;
        objClick.SetActive(true);

        switch (type)
        {
            case TutorialType.Camera_Move:
                yield return StartCoroutine(PlayDialog(type));

                yield return new WaitForSeconds(0.3f);

                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(gameSM.transform as RectTransform, mainCanvas) + new Vector2(0, 100f);
                co_MovementArrow = StartCoroutine(MovementArrow());
                yield return StartCoroutine(PlayDialog(TutorialType.Camera_Move));
                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.End:
                yield return StartCoroutine(PlayDialog(type));
                break;
        }
        yield return null;

        isTutorialShowed[(int)type] = true;
        PlayerPrefs.SetInt($"{KeyTutorial}_{type}", 1);
        rtArrow.SetActive(false);
        objTextBox.SetActive(false);
        isClicked = false;
        objClick.SetActive(false);
        co_CurrentTutorial = null;
    }

    bool isClicked = false;
    public void OnClick()
    {
        if (textTyper.isTyping)
        {
            textTyper.OnQuickTyping();
            return;
        }

        funcClick?.Invoke();
        isClicked = true;
    }

    IEnumerator MovementArrow()
    {
        float time = 0f;
        Vector2 originPos = rtArrow.localPosition;
        Vector2 endPos = originPos + new Vector2(0, PosArrowY);

        while (true)
        {
            while(time < 1f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time += Time.deltaTime * 2f;
                yield return null;
                //Debug.Log($"{rtArrow.localPosition} : {time}");
            }

            while(time > 0f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time -= Time.deltaTime * 2f;
                yield return null;
                //Debug.Log($"{rtArrow.localPosition} : {time}");
            }
        }
    }

    IEnumerator MovementArrow(Vector2 originPos, Vector2 endPos)
    {
        float time = 0f;

        while (true)
        {
            while (time < 1f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time += Time.deltaTime * 2f;
                yield return null;
                Debug.Log($"{rtArrow.localPosition} : {time}");
            }

            while (time > 0f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time -= Time.deltaTime * 2f;
                yield return null;
                Debug.Log($"{rtArrow.localPosition} : {time}");
            }
        }
    }

    IEnumerator MovementArrowOneSide(Vector2 originPos, Vector2 endPos, float duration = 1f)
    {
        float time = 0f;

        while (true)
        {
            while (time < 1f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time += Time.deltaTime / duration;
                yield return null;
            }

            rtArrow.localPosition = endPos;
            yield return new WaitForSeconds(0.3f);
            time = 0f;
        }
    }

    string currentText;
    IEnumerator PlayDialog(TutorialType type)
    {
        var strDialogs = GetDialog(type);
        if (strDialogs == null)
            yield break;

        objTextBox.SetActive(true);

        for (int i = 0; i < strDialogs.Length; i++)
        {
            isClicked = false;
            var text = strDialogs[i];
            if (text == null)
                continue;

            currentText = text;

            uiText.text = DataManager.GetString(text);
            rtText.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, uiText.preferredHeight);

            textTyper.Play(DataManager.GetString(text));
            yield return new WaitUntil(() => textTyper.isTyping == false);
            soundManager.StopSFX();
            yield return new WaitForSeconds(0.3f);
            yield return new WaitUntil(() => isClicked);
        }

        currentText = "";
    }

    void ChangeIsClicked()
    {
        isClicked = true;
    }

    string[] GetDialog(TutorialType type)
    {
        switch (type)
        {
            case TutorialType.Camera_Move:
                return DataManager.tutorialStr_Camera_Move;
        }

        return null;
    }

    void ChangeLanguageType()
    {
        if (currentText == null)
            return;

        textTyper.ChangeLanguageType(DataManager.GetString(currentText));
    }
}
