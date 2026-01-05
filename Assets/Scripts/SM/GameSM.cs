using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Report")]
    [SerializeField] GameObject objBtnReport;
    [SerializeField] GameObject objPending;
    [SerializeField] Text textPending;
    [SerializeField] ReportBox reportBox;

    bool isGameEnd = false;

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
        textTime.text = $"{hour:00} : {minute:00}";

        //After 6 Hour, Game Clear
        if(hour >= 6)
        {
            gm.LoadScene(SceneName.End);
            gm.isClear = true;
            gm.SetClear();
            isGameEnd = true;
            return;
        }

        if (currentTime > occurTimes[currentTimeIdx])
        {
            //If Reach 4, Game Over
            if(anomalies.Count == 3)
            {
                gm.LoadScene(SceneName.End);
                gm.isClear = false;
                isGameEnd = true;
                return;
            }

            int idAnomaly = GetAnomalyId();
            OnOccurAnomaly(idAnomaly);
            currentTimeIdx++;
        }
    }

    int idxCurrentCamera = 0;

    public void OnChangeCameraLeft()
    {
        if (idxCurrentCamera == 0)
            idxCurrentCamera = DataManager.countCamera - 1;
        else
            idxCurrentCamera--;

        OnChangeCamera(idxCurrentCamera);
    }

    public void OnChangeCameraRight()
    {
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
    }

    void OnOccurAnomaly(int idAnomaly)
    {
        var currentInfo = DataManager.dicAnomalyInfos[idAnomaly];
        switch (currentInfo.anomalyType)
        {
            case AnomalyType.Intruder:
                break;
            case AnomalyType.ObjectMovement:
                break;
            case AnomalyType.LightAnomaly:
                break;
            case AnomalyType.PictureAnomaly:
                break;
            case AnomalyType.ObjectDisappearance:
                break;
            case AnomalyType.ObjectAppearance:
                break;
            case AnomalyType.Distortion:
                break;
            case AnomalyType.TimeAnomaly:
                break;
            case AnomalyType.CameraMalfunction:
                break;
            case AnomalyType.BeegSana:
                break;
            case AnomalyType.Max:
                break;
        }

        anomalies.Add(currentInfo.id);
    }

    int GetAnomalyId()
    {
        List<int> candidates = new();
        foreach (var info in DataManager.dicAnomalyInfos)
        {
            //1. Already Added
            if (anomalies.Contains(info.Key))
                continue;


            //2.Beeg Sana Always Last
            if (currentTime < 1380 && info.Value.anomalyType == AnomalyType.BeegSana)
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
        reportBox.SetActive(true);
        reportBox.Set();
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
            objFixAnomaly.SetActive(true);
            foreach (int fixedId in fixedIds)
                OnFix(fixedId);
            yield return new WaitForSeconds(3f);
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

    public void OnFix(int id)
    {
        var anomalyInfo = DataManager.dicAnomalyInfos[id];

        switch (anomalyInfo.anomalyType)
        {
            case AnomalyType.Intruder:
                break;
            case AnomalyType.ObjectMovement:
                break;
            case AnomalyType.LightAnomaly:
                break;
            case AnomalyType.PictureAnomaly:
                break;
            case AnomalyType.ObjectDisappearance:
                break;
            case AnomalyType.ObjectAppearance:
                break;
            case AnomalyType.Distortion:
                break;
            case AnomalyType.TimeAnomaly:
                break;
            case AnomalyType.CameraMalfunction:
                break;
            case AnomalyType.BeegSana:
                break;
            case AnomalyType.Max:
                break;
        }
    }


    //Test
    public void OnOccurTest(int id)
    {
        OnOccurAnomaly(id);
    }
}
