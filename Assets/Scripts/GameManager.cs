using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Menu,
    Intro,
    Game,
    End,
}

public enum LanguageType
{
    Eng,
    Kor,
}

public class GameManager : Singleton
{
    public const string LaunguageKey = "LanguageType";
    public const string IsClearedKey = "IsCleared";

    public bool isCleared = false;
    public bool isClear = false; //Current Game Clear
    public SceneName currentScene;
    public LanguageType languageType;

    public void Awake()
    {
        if (gm != null)
        {
            Destroy(gameObject);
            return;
        }

        gm = this;
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        isCleared = PlayerPrefs.GetInt(IsClearedKey, 0) != 0;
        languageType = (LanguageType)PlayerPrefs.GetInt(LaunguageKey, 0);
        Observer.onRefreshLanguage?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            var currentSM = GetCurrentSM();
            if (currentSM != null)
            {
                var optionUI = currentSM.optionUI;
                if (optionUI != null)
                {
                    optionUI.SetActive(!optionUI.gameObject.activeSelf);
                    if (optionUI.gameObject.activeSelf)
                        optionUI.Set();
                }
            }
        }
    }

    public void LoadScene(SceneName sceneName)
    {
        StartCoroutine(Co_LoadScene(sceneName));
    }

    IEnumerator Co_LoadScene(SceneName sceneName)
    {
        yield return null;
        var currentSM = GetCurrentSM();
        if(currentSM != null) 
            currentSM.FadeOut();
        yield return new WaitForSeconds(SM.FadeTime);
        SceneManager.LoadScene(sceneName.ToString());
        currentScene = sceneName;
    }

    public SM GetCurrentSM()
    {
        return currentScene switch
        {
            SceneName.Menu => menuSM,
            SceneName.Intro => introSM,
            SceneName.End => endSM,
            SceneName.Game => gameSM,
            _ => null,
        };
    }

    public void SetClear()
    {
        PlayerPrefs.SetInt(IsClearedKey, 1);
    }
}
