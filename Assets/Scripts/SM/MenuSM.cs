using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuSM : SM
{
    public Text textStart;
    public Text textSettings;
    public Text textQuit;

    public void Awake()
    {
        if (menuSM != null)
            return;

        menuSM = this;
        Observer.onChangeLanguageType += ChangeLanguageType;
    }

    public void OnDestroy()
    {
        menuSM = null;
        Observer.onChangeLanguageType -= ChangeLanguageType;
    }

    protected override void Start()
    {
        base.Start();
        soundManager.PlayBGM(BGM.Menu);
    }

    public void OnStart()
    {
        if (gm.isCleared)
            gm.LoadScene(SceneName.Game);
        else
            gm.LoadScene(SceneName.Intro);
    }

    public void OnSettings()
    {
        optionUI.SetActive(true);
        optionUI.Set();
    }
    public void OnQuit()
    {
        Application.Quit();
    }

    void ChangeLanguageType()
    {
        textStart.text = DataManager.GetString("Start");
        textSettings.text = DataManager.GetString("Settings");
        textQuit.text = DataManager.GetString("Quit");
    }
}
