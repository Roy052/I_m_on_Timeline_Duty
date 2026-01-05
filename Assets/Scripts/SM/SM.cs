using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SM : Singleton
{
    public const float FadeTime = 1f;

    [Header("FadeOut")]
    public Image imgFade;

    [Header("Option")]
    public OptionUI optionUI;

    [Header("TutorialManager")]
    public TutorialManager tutorialManager;

    protected virtual void Start()
    {
        Observer.onChangeLanguageType?.Invoke();
    }

    public void FadeOut()
    {
        StartCoroutine(Co_FadeOut());
    }

    IEnumerator Co_FadeOut()
    {
        yield return StartCoroutine(FadeManager.FadeIn(imgFade, 1f));
        imgFade.SetActive(false);
    }

    public void OpenOptionUI()
    {
        optionUI.SetActive(true);
    }
}
