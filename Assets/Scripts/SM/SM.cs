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
        StartCoroutine(FadeManager.FadeIn(imgFade, 1f));
    }

    public void OpenOptionUI()
    {
        optionUI.SetActive(true);
    }
}
