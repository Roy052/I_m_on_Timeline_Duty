using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShowSM : SM
{
    [SerializeField] RectTransform rtVideo;
    [SerializeField] RectTransform rtText;
    [SerializeField] Text text;
    [SerializeField] TextTyper textTyper;
    [SerializeField] SceneName sceneName;

    protected List<string> strings;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(PlayDialogs());
    }

    bool isClicked = false;
    IEnumerator PlayDialogs()
    {
        soundManager.PlaySFX(SFX.KaelaScreen);
        yield return StartCoroutine(FadeManager.ChangeSize(rtVideo, new Vector2(1600, 0), new Vector2(1600, 900), 0.3f));

        for (int i = 0; i < strings.Count; i++)
        {
            soundManager.PlaySFX(SFX.TextTyping);
            isClicked = false;
            string s = DataManager.GetString(strings[i]);
            text.text = s;
            rtText.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, text.preferredWidth + 40f);
            text.text = "";
            textTyper.Play(s);
            yield return new WaitUntil(() => textTyper.isTyping == false);
            soundManager.StopSFX();
            yield return new WaitForSeconds(0.3f);
            yield return new WaitUntil(() => isClicked);
        }

        gm.LoadScene(sceneName);

        soundManager.PlaySFX(SFX.KaelaScreen);
        yield return StartCoroutine(FadeManager.ChangeSize(rtVideo, new Vector2(1600, 900), new Vector2(1600, 0), 0.3f));
    }

    public void OnClick()
    {
        if (textTyper.isTyping)
        {
            textTyper.OnQuickTyping();
            return;
        }

        isClicked = true;
    }
}
