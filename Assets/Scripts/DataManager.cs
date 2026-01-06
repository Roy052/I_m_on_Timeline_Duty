using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using Unity.VisualScripting;
using UnityEngine;

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
    public static int countCamera = 4;

    //(Position, Rotation)
    public static List<(Vector3, Vector3)> dataCameras = new()
    {
        (new Vector3(1.3f, 3.5f, -4.7f), new Vector3(23.5f, -128f, 0)), //Office
        (new Vector3(3.2f, 3.5f, -7.5f), new Vector3(23.5f, -270f, 0)), //Lounge
        (new Vector3(2, 3.5f, -3), new Vector3(25, 0, 0)), //Entrance
        (new Vector3(3.2f, 3.5f, 9.5f), new Vector3(23.5f, -90f, 0)), //Lounge
    };

    public static Dictionary<int, AnomalyInfo> dicAnomalyInfos = new()
    {
        {0, new AnomalyInfo()
        {
            id = 0,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.Intruder,
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

        {"Label_Anomaly_None", new() { "None", "없음" } },
        {"Label_Anomaly_Intruder", new() { "Intruder", "침입자" } },
        {"Label_Anomaly_ObjectMovement", new() { "Object Movement", "물체 이동" } },
        {"Label_Anomaly_LightAnomaly", new() { "Light Anomaly", "빛 이상 현상" } },
        {"Label_Anomaly_PictureAnomaly", new() { "Picture Anomaly", "그림 이상 현상" } },
        {"Label_Anomaly_ObjectDisappearance", new() { "Object Disappearance", "물체 실종" } },
        {"Label_Anomaly_ObjectAppearance", new() { "Object Appearance", "물체 생성" } },
        {"Label_Anomaly_Distortion", new() { "Distortion", "공간왜곡" } },
        {"Label_Anomaly_TimeAnomaly", new() { "Time Anomaly", "시간 이상 현상" } },
        {"Label_Anomaly_CameraMalfunction", new() { "Camera Malfunction", "카메라 오작동" } },
        {"Label_Anomaly_BeegSana", new() { "!!Beeg Sana!!", "!!Beeg Sana!!" } },

        //Btns
        { "Label_Start", new() { "START", "시작" } },
        { "Label_Settings", new() { "SETTINGS", "설정" } },
        { "Label_Credits", new() { "CREDITS", "크레딧" } },
        { "Label_Quit", new() { "QUIT", "종료" } },

        {"Label_File_Anomaly", new() { "File an anomaly report", "이상 현상 보고서 제출"} },
        {"Label_Send", new() { "Send", "보내기"} },
        {"Label_Cancel", new() { "Cancel", "취소"} },
        {"Label_Anomaly_Where", new() { "Anomaly spotted in:", "이상 현상이 발견된 곳:"} },
        {"Label_Anomaly_What", new() { "Anomaly type:", "이상 현상 종류:"} },
        {"Label_Report_Pending", new() {"Report pending", "보고서 전송 중"}},

        //Message
        {"Message_Notice_First", new(){"!!!ATTENTION FOR AGENT K!!! NUMEROUS ANOMALIES HAVE BEEN DETECTED LATELY. PAY EXTREMELY CLOSE ATTENTION TO THE SURVEILLANCE CAMERA FOOTAGE AND FILE AN ANOMALY REPORT AS SOON AS POSSIBLE WHEN YOU NOTICE SOMETHING CHANGED.", "!!!요원 K에게 긴급 통보!!! 최근 다수의 이상 현상이 감지되었습니다. 감시 카메라 영상을 극도로 주의 깊게 관찰하고 무언가 변화가 감지되는 즉시 가능한 한 빨리 이상 현상 보고서를 제출하십시오." } },

        {"Message_Anomaly_Fixed", new() {"Anomaly Fixed", "이상 현상 해결됨"} },
        {"Message_Anomaly_Not_Found", new() {"No anomaly of type \'_2_\' found in \'_1_\'.", "\'_1_\'에서 \'_2_\'이라는 이상 현상은 발견되지 않았습니다."} },


        { "Language", new() { "Language", "언어" } },
        { "BGM Volume", new() { "BGM Volume", "배경음악 크기" } },
        { "Sound Effects Volume", new() { "Sound Effects Volume", "효과음 크기" } },

        { "AnswerYes", new() { "YES", "그렇다" } },
        { "AnswerNo", new() { "NO", "아니다" } },

        

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

    public static List<(string, string, string)> credits = new()
    {
        ("A", "B", "C")
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