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

    protected virtual void Start()
    {
        Observer.onRefreshLanguage?.Invoke();
    }

    public void FadeOut()
    {
        StartCoroutine(Co_FadeOut());
    }

    IEnumerator Co_FadeOut()
    {
        imgFade.SetActive(true);
        yield return StartCoroutine(FadeManager.FadeIn(imgFade, 1f));
    }

    public void OpenOptionUI()
    {
        optionUI.SetActive(true);
    }
}
