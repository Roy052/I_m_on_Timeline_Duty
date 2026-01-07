using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuSM : SM
{
    [SerializeField] CreditBox creditBox;

    public void Awake()
    {
        if (menuSM != null)
            return;

        menuSM = this;
    }

    public void OnDestroy()
    {
        menuSM = null;
    }

    protected override void Start()
    {
        base.Start();
        soundManager.PlayBGM(BGM.Menu);
    }

    public void OnStart()
    {
        if (gm.isPlayedIntro)
            gm.LoadScene(SceneName.Game);
        else
            gm.LoadScene(SceneName.Intro);
    }

    public void OnSettings()
    {
        optionUI.SetActive(true);
        optionUI.Set();
    }

    public void OnCredit()
    {
        creditBox.Set();
        creditBox.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
