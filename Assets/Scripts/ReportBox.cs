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
        List<Dropdown.OptionData> optionDatas = new();
        for(PlaceType type = PlaceType.None; type < PlaceType.Max; type++)
            optionDatas.Add(new Dropdown.OptionData() { text = DataManager.GetString($"Label_Place_{type}") });
        dropDownPlace.options = optionDatas;

        optionDatas.Clear();
        for (AnomalyType type = AnomalyType.None; type < AnomalyType.BeegSana; type++)
            optionDatas.Add(new Dropdown.OptionData() { text = DataManager.GetString($"Label_Anomaly_{type}") });
        dropdownAnomaly.options = optionDatas;

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

    public void OnReport()
    {
        if (placeType == PlaceType.None || anomalyType == AnomalyType.None)
            return;

        gameSM.OnReport(placeType, anomalyType);
    }
}
