using System.Collections.Generic;
using UnityEngine;

public class GameSM : SM
{
    public class AnomalyOccurInfo
    {
        public float time;
        public int id;
    }

    [SerializeField]ReportBox reportBox;

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
        for(int i = 0; i < countAnomalyOverAll; i++)
        {
            int midValue = countAnomalyOverAll / 6;
            int[] countAnomalies = new int[6] { 2, 3, 4, 4, 5, 6 };
            int sum = 24; // 2 + 3 + 4 + 4 + 5 + 6

            // More Percentage for Last
            for(int j = countAnomalyOverAll - sum; j >= 0; j--)
            {
                int randValue = Random.Range(j, 6);
                countAnomalies[randValue]++;
            }

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
                    float time = hour * 60 + (60f / countCurrentHour) * count + Random.Range(-2f, 2f);
                    occurTimes.Add(time);
                }
            }
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (occurTimes[currentTimeIdx] > currentTime)
        {
            OnOccur();
            currentTimeIdx++;
        }
    }

    void OnOccur()
    {
        List<int> candidates = new();
        foreach(var info in DataManager.dicAnomalyInfos)
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
        if (candidates.Count > 0)
            currentIdx = Random.Range(0, candidates.Count);

        var currentInfo = DataManager.dicAnomalyInfos[candidates[currentIdx]];
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
    }

    public void OnReport(PlaceType placeType, AnomalyType anomalyType)
    {
        List<int> fixedIds = new();

        foreach(int id in anomalies)
        {
            var anomalyInfo = DataManager.dicAnomalyInfos[id];

            if(anomalyInfo.placeType == placeType && anomalyInfo.anomalyType == anomalyType)
                fixedIds.Add(id);
        }


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
}
