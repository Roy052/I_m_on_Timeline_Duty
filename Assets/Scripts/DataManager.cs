using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using Unity.VisualScripting;

public enum PlaceType
{
    None,
    Office,
    Lounge,
    Entrance,
    Storage,
    Max
}

public enum AnomalyType
{
    None,
    Intruder,
    ObjectMovement,
    LightAnomaly,
    PictureAnomaly,
    ObjectDisappearance,
    ObjectAppearance,
    Distortion,
    TimeAnomaly,
    CameraMalfunction,
    BeegSana,
    Max
}

public class AnomalyInfo
{
    public int id;
    public PlaceType placeType;
    public AnomalyType anomalyType;
    public float orginValue1;
    public float orginValue2;
    public float orginValue3;
    public float changeValue1;
    public float changeValue2;
    public float changeValue3;
}


public static class DataManager
{
    public static Dictionary<int, AnomalyInfo> dicAnomalyInfos = new()
    {
        {0, new AnomalyInfo()
        {

        } }
    };

    public static string[] tutorialStr_Camera_Move = new string[]
    {
        "Tutorial_Camera_Move_1",
        "Tutorial_Camera_Move_2",
    };

    public static (string, int)[] introStrImgNums = new (string, int)[]
    {
        ("Intro_1", 0),
        ("Intro_2", 0),
        ("Intro_3", 1),
        ("Intro_4", 2),
        ("Intro_5", 2),
        ("Intro_6", 2),
        ("Intro_7", 3),
    };

    public static (string, int)[] endingStrImgNums = new (string, int)[]
    {
        ("Ending_1", 0),
        ("Ending_2", 0),
    };

    public static Dictionary<string, List<string>> localizedText = new()
    {
        {"Label_Place_None", new() { "None", "없음" } },
        {"Label_Place_Office", new() { "Office", "사무실" } },
        {"Label_Place_Lounge", new() { "Lounge", "휴게실" } },
        {"Label_Place_Entrance", new() { "Entrance", "입구" } },
        {"Label_Place_Storage", new() { "Storage", "창고" } },

        {"Label_Anomaly_Intruder", new() { "Intruder", "침입자" } },
        {"Label_Anomaly_ObjectMovement", new() { "Object Movement", "물체 이동" } },
        {"Label_Anomaly_LightAnomaly", new() { "Light Anomaly", "빛 변칙" } },
        {"Label_Anomaly_PictureAnomaly", new() { "Picture Anomaly", "그림 변칙" } },
        {"Label_Anomaly_ObjectDisappearance", new() { "Object Disappearance", "물체 실종" } },
        {"Label_Anomaly_ObjectAppearance", new() { "Object Appearance", "물체 생성" } },
        {"Label_Anomaly_Distortion", new() { "Distortion", "공간왜곡" } },
        {"Label_Anomaly_TimeAnomaly", new() { "Time Anomaly", "시간 변칙" } },
        {"Label_Anomaly_CameraMalfunction", new() { "Camera Malfunction", "카메라 오작동" } },
        {"Label_Anomaly_BeegSana", new() { "!!Beeg Sana!!", "!!Beeg Sana!!" } },

        { "Language", new() { "Language", "언어" } },
        { "BGM Volume", new() { "BGM Volume", "배경음악 크기" } },
        { "Sound Effects Volume", new() { "Sound Effects Volume", "효과음 크기" } },
        { "Discard Deck", new() { "Discard Deck", "버린 더미" } },
        { "Reasoning", new() { "Is the opponent’s number _1_ or higher?", "상대방의 숫자가 _1_ 이상일까?" } },

        { "AnswerYes", new() { "YES", "그렇다" } },
        { "AnswerNo", new() { "NO", "아니다" } },

        { "Start", new() { "START", "시작" } },
        { "Settings", new() { "SETTINGS", "설정" } },
        { "Quit", new() { "QUIT", "종료" } },

        { "LabelFloor", new() { "_1_ Floor", "_1_ 층" } },

        { "UpgradeText1", new() { "Please select a card to upgrade.\r\n", "강화할 카드를 선택해주세요." } },
        { "UpgradeText2", new() { "Check the selected card.", "선택한 카드를 확인하세요." } },

        { "LabelUpgrade", new(){ "UPGRADE" , "강화" } },
        { "LabelCancel", new(){ "CANCEL" , "취소" } },
        { "LabelNext", new(){ "NEXT" , "다음" } },

        { "Objection", new(){ "Objection!", "이의 있음!" } },

        { "Intro_1", new() { "As on any other day, I head to IRyS\'s room", "여느날처럼 아이리스의 방으로 향했으나" } },
        { "Intro_2", new() { "but she isn\'t there.", "그녀는 자리에 없다." } },
        { "Intro_3", new() { "Just as I turn on my phone to contact her,", "연락을 하려고 폰을 켠 그 순간," } },
        { "Intro_4", new() { "a message appears.", "한 통의 문자가 날아온다." } },
        { "Intro_5", new() { "If you wish to find her,", "그녀를 찾고 싶다면," } },
        { "Intro_6", new() { "come here and take the trial.", "이리로 와 시험을 치루라고" } },
        { "Intro_7", new() { "With no other choice, you board a ship and set out for that place.", "어쩔 수 없이 배를 타고 그곳으로 향한다." } },
        
        { "Ending_1", new() { "Everything had been her prank all along.", "모든 일은 그녀의 장난이었다." } },
        { "Ending_2", new() { "What kind of prank could be waiting next time?", "앞으론 또 어떤 장난이 기다리고 있을까?" } },
    };

    public static string GetString(string label, params string[] arr)
    {
        localizedText.TryGetValue(label, out var texts);
        if (texts == null)
            return "";

        int idx = 0;
        string text = texts[(int)Singleton.gm.languageType];
        string result = Regex.Replace(
            text,
            @"_(\d+)_",
            match =>
            {
                if (idx >= arr.Length)
                    return "";

                return arr[idx++];
            },
            RegexOptions.CultureInvariant);

        return result;
    }
}