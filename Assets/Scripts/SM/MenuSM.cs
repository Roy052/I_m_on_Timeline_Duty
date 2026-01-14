using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSM : SM
{
    [SerializeField] CreditBox creditBox;
    [SerializeField] List<GameObject> objImgs;

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
        changeTime = Random.Range(2f, 3f);
    }

    float time = 0f;
    float changeTime = 0f;
    private void Update()
    {
        time += Time.deltaTime;

        if (time > changeTime)
        {
            int front = Random.Range(0, objImgs.Count);
            objImgs[front].transform.SetAsLastSibling();
            changeTime = Random.Range(2f, 3f);
            time = 0f;
        }
    }

    public void OnStart()
    {
        gm.gameResult = null;
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
