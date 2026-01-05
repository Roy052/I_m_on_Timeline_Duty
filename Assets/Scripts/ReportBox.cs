using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ReportBox : Singleton
{
    [SerializeField] Dropdown dropDownPlace;
    [SerializeField] Dropdown dropdownAnomaly;

    PlaceType placeType;
    AnomalyType anomalyType;

    bool isBeegAdded = false;

    private void Awake()
    {
        Observer.onChangeLanguageType += Set;
    }

    private void OnDestroy()
    {
        Observer.onChangeLanguageType -= Set;
    }

    public void Set()
    {
        List<Dropdown.OptionData> optionPlaceDatas = new();
        for(PlaceType type = PlaceType.None; type < PlaceType.Max; type++)
            optionPlaceDatas.Add(new Dropdown.OptionData() { text = DataManager.GetString($"Label_Place_{type}") });
        dropDownPlace.options = optionPlaceDatas;

        List<Dropdown.OptionData> optionAnomalyDatas = new();
        for (AnomalyType type = AnomalyType.None; type < AnomalyType.BeegSana; type++)
            optionAnomalyDatas.Add(new Dropdown.OptionData() { text = DataManager.GetString($"Label_Anomaly_{type}") });
        dropdownAnomaly.options = optionAnomalyDatas;

        if (isBeegAdded)
            AddBeegSana();

        SetInitialState();
    }

    public void SetInitialState()
    {
        placeType = PlaceType.None;
        anomalyType = AnomalyType.None;
    }

    public void AddBeegSana()
    {
        dropdownAnomaly.options.Add(new Dropdown.OptionData() { text = DataManager.GetString("Label_Anomaly_BeegSana") });
        isBeegAdded = true;
    }

    public void SelectPlace(int value)
    {
        placeType = (PlaceType)value;
    }

    public void SelectAnomaly(int value)
    {
        anomalyType = (AnomalyType)value;
    }

    public void OnClickSend()
    {
        if (placeType == PlaceType.None || anomalyType == AnomalyType.None)
            return;

        gameSM.OnSendReport(placeType, anomalyType);
    }

    public void OnClickCancel()
    {
        this.SetActive(false);
    }
}
