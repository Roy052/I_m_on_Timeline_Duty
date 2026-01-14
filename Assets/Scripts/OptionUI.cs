using UnityEngine;
using UnityEngine.UI;

public class OptionUI : Singleton
{
    const float VolumeDefault = 0.5f;
    const string BGMKey = "BGMKey";
    const string SFXKey = "SFXKey";

    public Dropdown dropDownLanguage;
    public Slider sliderBGMVolume;
    public Slider sliderSFXVolume;

    int numLanguage;
    float bgmVolumeValue = 1f;
    float sfxVolumeValue = 1f;


    private void OnDisable()
    {
        PlayerPrefs.SetFloat(BGMKey, bgmVolumeValue);
        PlayerPrefs.SetFloat(SFXKey, sfxVolumeValue);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(BGMKey, bgmVolumeValue);
        PlayerPrefs.SetFloat(SFXKey, sfxVolumeValue);
    }

    public void Set()
    {
        numLanguage = (int)gm.languageType;
        dropDownLanguage.value = numLanguage;

        float bgmVolume = PlayerPrefs.GetFloat(BGMKey, 1);
        float sfxVolume = PlayerPrefs.GetFloat(SFXKey, 1);

        sliderBGMVolume.value = bgmVolume;
        sliderSFXVolume.value = sfxVolume;
    }

    public void ChangeBGMVolume(float value)
    {
        Singleton.soundManager.ChangeBgmVolume(VolumeDefault * value);
        bgmVolumeValue = value;
    }

    public void ChangeSFXVolume(float value)
    {
        Singleton.soundManager.ChangeSfxVolume(VolumeDefault * value);
        sfxVolumeValue = value;
    }

    public void ChangeLanguage(int value)
    {
        gm.languageType = (LanguageType)value;
        PlayerPrefs.SetInt(GameManager.LaunguageKey, value);
        Observer.onRefreshLanguage?.Invoke();
    }

    public void OnClickHome()
    {
        gm.LoadScene(SceneName.Menu);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
