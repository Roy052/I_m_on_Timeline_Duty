using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameSM : SM
{
    public Camera mainCamera;

    [Header("Game UI")]
    [SerializeField] Text textCamera;
    [SerializeField] Text textTime;
    [SerializeField] GameObject objFixAnomaly;
    [SerializeField] Text textReportResult;
    [SerializeField] GameObject objBtnCameraLeft;
    [SerializeField] GameObject objBtnCameraRight;

    [Header("Report")]
    [SerializeField] GameObject objBtnReport;
    [SerializeField] GameObject objPending;
    [SerializeField] Text textPending;
    [SerializeField] ReportBox reportBox;

    [Header("AnimationClip")]
    [SerializeField] List<AnimationClip> clips;

    [Header("Anomalies]")]
    [SerializeField] GameObject objCameraMalfunction;
    [SerializeField] List<GameObject> objAnomalies;
    [SerializeField] List<Material> matPaints;
    bool[] timeAnomaly = new bool[4];
    bool[] malfunctionAnomaly = new bool[4];
    float[] timeCameraSaw = new float[4];
    AnomalyInfo[] intruderInfos = new AnomalyInfo[4];

    [Header("Test")]
    [SerializeField] InputField inputField;

    bool isGameEnd = false;

    bool isWarningHappened = false;

    int countAnomaly = 0;
    int countFix = 0;

    public class AnomalyOccurInfo
    {
        public float time;
        public int id;
    }

    public float currentTime = 0f;
    public HashSet<int> anomalies = new HashSet<int>();

    private void Awake()
    {
        if (gameSM != null)
            return;

        gameSM = this;
    }

    private void OnDestroy()
    {
        if (gameSM != this)
            return;

        gameSM = null;
    }

    List<float> occurTimes = new();
    int currentTimeIdx = 0;

    protected override void Start()
    {
        base.Start();
        //Intro
        gm.SetIntroPlayed();

        int countAnomalyOverAll = Random.Range(25, 29);
        int[] countAnomalies = new int[6] { 2, 3, 4, 4, 5, 6 };
        int sum = 24; // 2 + 3 + 4 + 4 + 5 + 6

        // More Percentage for Last
        for(int j = countAnomalyOverAll - sum; j >= 0; j--)
        {
            int randValue = Random.Range(j, 6);
            countAnomalies[randValue]++;
        }

        Array.Sort(countAnomalies);

        for (int hour = 0; hour < 6; hour++)
        {
            int countCurrentHour = countAnomalies[hour];
            int count = 0;
                
            if(hour == 0)
            {
                count = 1;
                countAnomalies[hour]++;
            }

            for(;count < countCurrentHour; count++)
            {
                float minute = hour * 60 + (60f / countCurrentHour) * count + Random.Range(-2f, 2f);
                occurTimes.Add(minute * (60 / 12)); //Game Time is 1/12 of Real Time
            }
        }

        StringBuilder sb = new();
        sb.AppendLine("~~~~~~~~~~~~~~~~~");
        foreach(float occurTime in occurTimes)
        {
            int hour = (int)(occurTime * 12 / 3600);
            int minute = (int)((occurTime * 12 % 3600) / 60);
            sb.AppendLine($"{hour}:{minute}");
        }
        Debug.Log(sb.ToString());

        OnChangeCamera(0);
        reportBox.SetActive(false);
        objFixAnomaly.SetActive(false);
        textReportResult.SetActive(false);
    }

    private void Update()
    {
        if (isGameEnd)
            return;

        currentTime += Time.deltaTime;
        int hour = (int)(currentTime * 12 / 3600);
        int minute = (int)((currentTime * 12 % 3600) / 60);

        if (timeAnomaly[idxCurrentCamera])
            textTime.text = "T:M:T";
        else
            textTime.text = $"{hour:00} : {minute:00}";

        //After 6 Hour, Game Clear
        if(hour >= 6)
        {
            OnGameEnd(true);
            return;
        }

        if (currentTime > occurTimes[currentTimeIdx])
        {
            int idAnomaly = GetAnomalyId();
            OnOccurAnomaly(idAnomaly);
            currentTimeIdx++;
        }

        timeCameraSaw[idxCurrentCamera] += Time.deltaTime;
        if (intruderInfos[idxCurrentCamera] != null && timeCameraSaw[idxCurrentCamera] > 15f)
        {
            StartCoroutine(Co_IntruderRun(intruderInfos[idxCurrentCamera]));
        }    

        if(isWarningHappened == false && anomalies.Count == 3 && (occurTimes[currentTimeIdx] - currentTime  < 60f))
        {
            isWarningHappened = true;
            soundManager.PlaySFX(SFX.Danger);
        }

        if(Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.D))
            inputField.SetActive(true);
    }

    int idxCurrentCamera = 0;

    public void OnChangeCameraLeft()
    {
        timeCameraSaw[idxCurrentCamera] = 0;

        if (idxCurrentCamera == 0)
            idxCurrentCamera = DataManager.countCamera - 1;
        else
            idxCurrentCamera--;

        OnChangeCamera(idxCurrentCamera);
    }

    public void OnChangeCameraRight()
    {
        timeCameraSaw[idxCurrentCamera] = 0;

        if (idxCurrentCamera == DataManager.countCamera - 1)
            idxCurrentCamera = 0;
        else
            idxCurrentCamera++;

        OnChangeCamera(idxCurrentCamera);
    }

    void OnChangeCamera(int idx)
    {
        textCamera.text = DataManager.GetString($"Label_Place_{(PlaceType)idxCurrentCamera}");
        mainCamera.transform.position = DataManager.dataCameras[idxCurrentCamera].Item1;
        mainCamera.transform.rotation = Quaternion.Euler(DataManager.dataCameras[idxCurrentCamera].Item2);

        objCameraMalfunction.SetActive(malfunctionAnomaly[idxCurrentCamera]);

        if (intruderInfos[idx] != null)
            soundManager.PlayBGM(BGM.Heartbeat);
        else if (malfunctionAnomaly[idxCurrentCamera])
            soundManager.PlayBGM(BGM.CameraMalfunction);
        else
            soundManager.StopBGM();
    }

    int GetAnomalyId()
    {
        PlaceType placeCurrentCamera = (PlaceType)idxCurrentCamera;

        List<int> candidates = new();
        foreach (var info in DataManager.dicAnomalyInfos)
        {
            //1. Already Added
            if (anomalies.Contains(info.Key))
                continue;


            //2.Beeg Sana Always Last
            if (currentTime < 1380 && info.Value.anomalyType == AnomalyType.BeegSana)
                continue;

            //3. Not Same Place
            if (placeCurrentCamera == info.Value.placeType)
                continue;

            candidates.Add(info.Key);
        }

        //Select
        int currentIdx = 0;
        if(candidates.Count == 0)
        {
            Debug.LogError($"No Candidates");
            return 0;
        }
        
        currentIdx = Random.Range(0, candidates.Count);
        return candidates[currentIdx];
    }

    public void OnClickReport()
    {
        objBtnReport.SetActive(false);
        reportBox.SetActive(true);
        reportBox.Set();
    }

    public void OnClickReportCancel()
    {
        objBtnReport.SetActive(true);
    }

    Coroutine co_pendingText = null;
    Coroutine co_SendReport = null;
    public void OnSendReport(PlaceType placeType, AnomalyType anomalyType)
    {
        if (co_SendReport != null)
            return;

        co_SendReport = StartCoroutine(Co_OnSendReport(placeType, anomalyType));
    }

    IEnumerator Co_OnSendReport(PlaceType placeType, AnomalyType anomalyType)
    {
        objBtnReport.SetActive(false);
        reportBox.SetActive(false);
        objPending.SetActive(true);

        co_pendingText = StartCoroutine(Co_PendingText());
        yield return new WaitForSeconds(12f);
        StopCoroutine(co_pendingText);
        co_pendingText = null;

        List<int> fixedIds = new();

        foreach (int id in anomalies)
        {
            var anomalyInfo = DataManager.dicAnomalyInfos[id];

            if (anomalyInfo.placeType == placeType && anomalyInfo.anomalyType == anomalyType)
                fixedIds.Add(id);
        }

        //1. If Report Failed
        if (fixedIds.Count == 0)
        {
            co_SendReport = null;
            StartCoroutine(Co_ShowReportResult(false, placeType, anomalyType));
        }
        //2. If Report Succeed => Fix
        else
        {
            soundManager.PlaySFX(SFX.Fix);
            objFixAnomaly.SetActive(true);
            foreach (int fixedId in fixedIds)
                OnFix(fixedId);
            yield return new WaitForSeconds(3f);
            soundManager.StopSFX();
            objFixAnomaly.SetActive(false);
            StartCoroutine(Co_ShowReportResult(true));
        }

        objBtnReport.SetActive(true);
        objPending.SetActive(false);

        co_SendReport = null;
    }

    IEnumerator Co_PendingText()
    {
        int count = 0;
        while (count < 12)
        {
            textPending.text = DataManager.GetString("Label_Report_Pending") + ".";
            yield return new WaitForSeconds(0.5f);
            textPending.text = DataManager.GetString("Label_Report_Pending") + "..";
            yield return new WaitForSeconds(0.5f);
            textPending.text = DataManager.GetString("Label_Report_Pending") + "...";
            yield return new WaitForSeconds(0.5f);
            count++;      
        }
    }

    IEnumerator Co_ShowReportResult(bool isSuccess, PlaceType placeType = PlaceType.None, AnomalyType anomalyType = AnomalyType.None)
    {
        yield return new WaitForSeconds(0.5f);
        textReportResult.SetActive(true);

        if(isSuccess)
            textReportResult.text = DataManager.GetString("Message_Anomaly_Fixed");
        else
            textReportResult.text = DataManager.GetString("Message_Anomaly_Not_Found", DataManager.GetString($"Label_Place_{placeType}"), DataManager.GetString($"Label_Anomaly_{anomalyType}"));
        
        yield return StartCoroutine(FadeManager.FadeIn(textReportResult, 0.5f));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeManager.FadeOut(textReportResult, 0.5f));
        textReportResult.SetActive(false);
    }

    void OnOccurAnomaly(int idAnomaly)
    {
        //If Reach 4, Game Over
        if (anomalies.Count == 3)
        {
            OnGameEnd(false);
            return;
        }

        var currentInfo = DataManager.dicAnomalyInfos[idAnomaly];
        var objAnomaly = objAnomalies[currentInfo.idObject];
        switch (currentInfo.anomalyType)
        {
            case AnomalyType.Intruder:
                objAnomaly.SetActive(true);
                intruderInfos[(int)currentInfo.placeType] = currentInfo;
                break;
            case AnomalyType.ObjectMovement:
                objAnomaly.transform.localPosition = new Vector3(currentInfo.changeValue1, currentInfo.changeValue2, currentInfo.changeValue3);
                break;
            case AnomalyType.LightAnomaly:
                objAnomaly.SetActive(true);
                break;
            case AnomalyType.PictureAnomaly:
                MeshRenderer mr = objAnomaly.GetComponent<MeshRenderer>();
                mr.material = matPaints[(int)currentInfo.changeValue1];
                break;
            case AnomalyType.ObjectDisappearance:
                objAnomaly.SetActive(false);
                break;
            case AnomalyType.ObjectAppearance:
                objAnomaly.SetActive(true);
                break;
            case AnomalyType.Distortion:
                objAnomaly.SetActive(true); //Distortion Object
                break;
            case AnomalyType.TimeAnomaly:
                timeAnomaly[(int)currentInfo.placeType] = true;
                break;
            case AnomalyType.CameraMalfunction:
                malfunctionAnomaly[(int)currentInfo.placeType] = true;
                break;
            case AnomalyType.MonitorAnomaly:
                objAnomaly.SetActive(true);
                break;
            case AnomalyType.BeegSana:
                objAnomaly.SetActive(true);
                break;
            case AnomalyType.Max:
                break;
        }

        anomalies.Add(currentInfo.id);
        countAnomaly++;

        int hour = (int)(currentTime * 12 / 3600);
        int minute = (int)((currentTime * 12 % 3600) / 60);
        Debug.Log($"{countAnomaly} Anomaly : {idAnomaly}, ({hour}:{minute})");
    }

    public void OnFix(int id)
    {
        var currentInfo = DataManager.dicAnomalyInfos[id];
        var objAnomaly = objAnomalies[currentInfo.idObject];
        switch (currentInfo.anomalyType)
        {
            case AnomalyType.Intruder:
                objAnomaly.SetActive(false);
                intruderInfos[(int)currentInfo.placeType] = null;
                break;
            case AnomalyType.ObjectMovement:
                objAnomaly.transform.localPosition = new Vector3(currentInfo.orginValue1, currentInfo.orginValue2, currentInfo.orginValue3);
                break;
            case AnomalyType.LightAnomaly:
                objAnomaly.SetActive(false);
                break;
            case AnomalyType.PictureAnomaly:
                MeshRenderer mr = objAnomaly.GetComponent<MeshRenderer>();
                mr.material = matPaints[(int)currentInfo.orginValue1];
                break;
            case AnomalyType.ObjectDisappearance:
                objAnomaly.SetActive(true);
                break;
            case AnomalyType.ObjectAppearance:
                objAnomaly.SetActive(false);
                break;
            case AnomalyType.Distortion:
                objAnomaly.SetActive(false);
                break;
            case AnomalyType.TimeAnomaly:
                timeAnomaly[(int)currentInfo.placeType] = false;
                break;
            case AnomalyType.CameraMalfunction:
                malfunctionAnomaly[(int)currentInfo.placeType] = false;
                break;
            case AnomalyType.MonitorAnomaly:
                objAnomaly.SetActive(false);
                break;
            case AnomalyType.BeegSana:
                break;
            case AnomalyType.Max:
                break;
        }

        anomalies.Remove(id);

        OnChangeCamera(idxCurrentCamera);
        countFix++;
    }

    IEnumerator Co_IntruderRun(AnomalyInfo info)
    {
        isGameEnd = true;

        objBtnReport.SetActive(false);
        objBtnCameraLeft.SetActive(false);
        objBtnCameraRight.SetActive(false);

        var posCamera = DataManager.dataCameras[(int)info.placeType].Item1;
        var posEnd = new Vector3(posCamera.x, 0, posCamera.z);
        var objAnomaly = objAnomalies[info.idObject];

        Animator animator = objAnomaly.GetComponent<Animator>();
        animator.SetBool("IsRun", true);

        var transform = objAnomaly.transform;
        var posStart = transform.localPosition;

        float time = 0f;
        while(time < 1f)
        {
            transform.localPosition = Vector3.Lerp(posStart, posEnd, time);
            time += Time.deltaTime;
            yield return null;
        }

        OnGameEnd(false);
    }

    void OnGameEnd(bool isClear)
    {
        GameResult gameResult = new GameResult()
        {
            isClear = isClear,
            countAnomaly = countAnomaly,
            countFix = countFix,
            notFixed = anomalies.ToList()
        };
        gm.gameResult = gameResult;
        soundManager.StopBGM();
        soundManager.StopSFX();
        gm.LoadScene(SceneName.End);
        isGameEnd = true;
    }
    

    //Test
    public void OnOccurTest(int id)
    {
        OnOccurAnomaly(id);
    }

    public void OnClickTestInputField()
    {
        OnOccurTest(int.Parse(inputField.text));
    }
}
