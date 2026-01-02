using UnityEngine;
using System.Collections;

public class ShowSM : SM
{
    public TextTyper textTyper;
    public GameObject[] objImgs;
    public SceneName sceneName;
    public (string, int)[] strImgNums;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(PlayDialogs());
    }

    bool isClicked = false;
    IEnumerator PlayDialogs()
    {
        int currentImgNum = -1;
        for (int i = 0; i < strImgNums.Length; i++)
        {
            isClicked = false;
            var (str, imgNum) = strImgNums[i];


            if (str == "Intro_4")
                soundManager.PlaySFX(SFX.Message);

            if (currentImgNum != imgNum)
            {
                objImgs[imgNum].SetActive(true);
                textTyper.SetActive(false);
                yield return new WaitForSeconds(1f);
                currentImgNum = imgNum;
                textTyper.SetActive(true);
            }


            textTyper.Play(DataManager.GetString(str));
            yield return new WaitUntil(() => textTyper.isTyping == false);
            soundManager.StopSFX();
            yield return new WaitForSeconds(0.3f);
            yield return new WaitUntil(() => isClicked);
        }

        for (int i = 0; i < objImgs.Length - 1; i++)
            objImgs[i].SetActive(false);

        gm.LoadScene(sceneName);
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
