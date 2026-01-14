using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using Unity.VisualScripting;
using UnityEngine;

public enum PlaceType
{
    None = -1,
    Office,
    Lounge,
    Entrance,
    Storage,
    Max
}

public enum AnomalyType
{
    None = -1,
    Intruder,
    ObjectMovement,
    LightAnomaly,
    PictureAnomaly,
    ObjectDisappearance,
    ObjectAppearance,
    Distortion,
    TimeAnomaly,
    CameraMalfunction,
    MonitorAnomaly,
    BeegSana,
    Max
}

public class AnomalyInfo
{
    public int id;
    public PlaceType placeType;
    public AnomalyType anomalyType;
    public int idObject;
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
            anomalyType = AnomalyType.LightAnomaly,
            idObject = 0,
        } },
        {1, new AnomalyInfo()
        {
            id = 1,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.LightAnomaly,
            idObject = 1,
        } },
        {2, new AnomalyInfo()
        {
            id = 2,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.LightAnomaly,
            idObject = 2,
        } },
        {3, new AnomalyInfo()
        {
            id = 3,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.LightAnomaly,
            idObject = 3,
        } },
        {4, new AnomalyInfo()
        {
            id = 4,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.CameraMalfunction,
        } },
        {5, new AnomalyInfo()
        {
            id = 5,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.CameraMalfunction,
        } },
        {6, new AnomalyInfo()
        {
            id = 6,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.CameraMalfunction,
        } },
        {7, new AnomalyInfo()
        {
            id = 7,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.CameraMalfunction,
            idObject = 3,
        } },
        {8, new AnomalyInfo()
        {
            id = 8,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.Distortion,
            idObject = 8,
        } },
        {9, new AnomalyInfo()
        {
            id = 9,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.Distortion,
            idObject = 9,
        } },
        {10, new AnomalyInfo()
        {
            id = 10,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.Distortion,
            idObject = 10,
        } },
        {11, new AnomalyInfo()
        {
            id = 11,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.Distortion,
            idObject = 11,
        } },
        {12, new AnomalyInfo()
        {
            id = 12,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 12,
        } },
        {13, new AnomalyInfo()
        {
            id = 13,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 13,
        } },
        {14, new AnomalyInfo()
        {
            id = 14,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 14,
        } },
        {15, new AnomalyInfo()
        {
            id = 15,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 15,
        } },
        {16, new AnomalyInfo()
        {
            id = 16,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.PictureAnomaly,
            idObject = 16,
            orginValue1 = 0,
            changeValue1 = 2,
        } },
        {17, new AnomalyInfo()
        {
            id = 17,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.PictureAnomaly,
            idObject = 17,
            orginValue1 = 1,
            changeValue1 = 5,
        } },
        {18, new AnomalyInfo()
        {
            id = 18,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.PictureAnomaly,
            idObject = 18,
            orginValue1 = 0,
            changeValue1 = 6,
        } },
        {19, new AnomalyInfo()
        {
            id = 19,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.PictureAnomaly,
            idObject = 19,
            orginValue1 = 1,
            changeValue1 = 7,
        } },
        {20, new AnomalyInfo()
        {
            id = 20,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.PictureAnomaly,
            idObject = 20,
            orginValue1 = 4,
            changeValue1 = 8,
        } },
        {21, new AnomalyInfo()
        {
            id = 21,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.PictureAnomaly,
            idObject = 21,
            orginValue1 = 4,
            changeValue1 = 9,
        } },
        {22, new AnomalyInfo()
        {
            id = 22,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 22,
            orginValue1 = 0,
            orginValue2 = 13.8f,
            orginValue3 = -8.3f,
            changeValue1 = 0,
            changeValue2 = 13.8f,
            changeValue3 = -7.3f,
        } },
        {23, new AnomalyInfo()
        {
            id = 23,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 23,
        } },
        {24, new AnomalyInfo()
        {
            id = 24,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 24,
            orginValue1 = -4.88f,
            orginValue2 = 13.89f,
            orginValue3 = -7.85f,
            changeValue1 = -4.88f,
            changeValue2 = 13.89f,
            changeValue3 = -7.36f,
        } },
        {25, new AnomalyInfo()
        {
            id = 25,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 25,
        } },
        {26, new AnomalyInfo()
        {
            id = 26,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 26,
        } },
        {27, new AnomalyInfo()
        {
            id = 27,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 27,
        } },
        {28, new AnomalyInfo()
        {
            id = 28,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 28,
        } },
        {29, new AnomalyInfo()
        {
            id = 29,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 29,
        } },
        {30, new AnomalyInfo()
        {
            id = 30,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 30,
        } },
        {31, new AnomalyInfo()
        {
            id = 31,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 31,
            orginValue1 = 6.54f,
            orginValue2 = 13.4f,
            orginValue3 = -5.86f,
            changeValue1 = 6.54f,
            changeValue2 = 13.4f,
            changeValue3 = -6.4f,
        } },
        {32, new AnomalyInfo()
        {
            id = 32,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 32,
            orginValue1 = 8.8f,
            orginValue2 = 1.35f,
            orginValue3 = -6.8f,
            changeValue1 = 8.8f,
            changeValue2 = 1.35f,
            changeValue3 = -8.6f,
        } },
        {33, new AnomalyInfo()
        {
            id = 33,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 33,
        } },
        {34, new AnomalyInfo()
        {
            id = 34,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.MonitorAnomaly,
            idObject = 34,
        } },
        {35, new AnomalyInfo()
        {
            id = 35,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 35,
        } },
        {36, new AnomalyInfo()
        {
            id = 36,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 36,
        } },
        {37, new AnomalyInfo()
        {
            id = 37,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 37,
        } },
        {38, new AnomalyInfo()
        {
            id = 38,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 38,
            orginValue1 = 5.73f,
            orginValue2 = 1.66f,
            orginValue3 = 1.86f,
            changeValue1 = 7.09f,
            changeValue2 = 1.66f,
            changeValue3 = 1.86f,
        } },
        {39, new AnomalyInfo()
        {
            id = 39,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 39,
            orginValue1 = 6.24f,
            orginValue2 = 0.33f,
            orginValue3 = 1.35f,
            changeValue1 = 5.58f,
            changeValue2 = 0.33f,
            changeValue3 = 1.35f,
        } },
        {40, new AnomalyInfo()
        {
            id = 40,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 40,
            orginValue1 = 4.48f,
            orginValue2 = 13.85f,
            orginValue3 = 1.93f,
            changeValue1 = 3.8f,
            changeValue2 = 13.85f,
            changeValue3 = 1.93f,
        } },
        {41, new AnomalyInfo()
        {
            id = 41,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 41,
        } },
        {42, new AnomalyInfo()
        {
            id = 42,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 42,
        } },
        {43, new AnomalyInfo()
        {
            id = 43,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.ObjectDisappearance,
            idObject = 43,
        } },
        {44, new AnomalyInfo()
        {
            id = 44,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.ObjectAppearance,
            idObject = 44,
        } },
        {45, new AnomalyInfo()
        {
            id = 45,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 45,
            orginValue1 = -3.39f,
            orginValue2 = 13.85f,
            orginValue3 = 8.29f,
            changeValue1 = -3.39f,
            changeValue2 = 13.85f,
            changeValue3 = 7.56f,
        } },
        {46, new AnomalyInfo()
        {
            id = 46,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.ObjectMovement,
            idObject = 46,
            orginValue1 = -0.47f,
            orginValue2 = 1.88f,
            orginValue3 = 11.34f,
            changeValue1 = -1.35f,
            changeValue2 = 1.88f,
            changeValue3 = 11.34f,
        } },
        {47, new AnomalyInfo()
        {
            id = 47,
            placeType = PlaceType.Office,
            anomalyType = AnomalyType.Intruder,
            idObject = 47,
        } },
        {48, new AnomalyInfo()
        {
            id = 48,
            placeType = PlaceType.Lounge,
            anomalyType = AnomalyType.Intruder,
            idObject = 48,
        } },
        {49, new AnomalyInfo()
        {
            id = 49,
            placeType = PlaceType.Entrance,
            anomalyType = AnomalyType.Intruder,
            idObject = 49,
        } },
        {50, new AnomalyInfo()
        {
            id = 50,
            placeType = PlaceType.Storage,
            anomalyType = AnomalyType.Intruder,
            idObject = 50,
        } },
    };

    public static string[] tutorialStr_Camera_Move = new string[]
    {
        "Tutorial_Camera_Move_1",
        "Tutorial_Camera_Move_2",
    };

    public static List<string> introStrs = new()
    {
        "Intro_1",
        "Intro_2",
        "Intro_3",
        "Intro_4",
        "Intro_5",
        "Intro_6",
        "Intro_7",
        "Intro_8",
        "Intro_9",
        "Intro_10",
    };

    public static List<string> gameOverStrs = new()
    {
        "Ending_GameOver_1",
        "Ending_GameOver_2",
    };

    public static List<string> gameEndStrs = new()
    {
        "Ending_GameEnd_1",
        "Ending_GameEnd_2",
    };

    public static Dictionary<string, List<string>> localizedText = new()
    {
        {"Label_Place_None", new() { "None", "없음", "なし" } },
        {"Label_Place_Office", new() { "Office", "사무실", "事務室" } },
        {"Label_Place_Lounge", new() { "Lounge", "휴게실", "休憩室" } },
        {"Label_Place_Entrance", new() { "Entrance", "입구", "入口" } },
        {"Label_Place_Storage", new() { "Storage", "창고", "倉庫" } },

        {"Label_Anomaly_None", new() { "None", "없음", "なし" } },
        {"Label_Anomaly_Intruder", new() { "Intruder", "침입자", "侵入者" } },
        {"Label_Anomaly_ObjectMovement", new() { "Object Movement", "물체 이동", "物体移動" } },
        {"Label_Anomaly_LightAnomaly", new() { "Light Anomaly", "빛 이상 현상", "光異常" } },
        {"Label_Anomaly_PictureAnomaly", new() { "Picture Anomaly", "그림 이상 현상", "影異常" } },
        {"Label_Anomaly_ObjectDisappearance", new() { "Object Disappearance", "물체 실종", "物体消失" } },
        {"Label_Anomaly_ObjectAppearance", new() { "Object Appearance", "물체 생성", "物体生成" } },
        {"Label_Anomaly_Distortion", new() { "Distortion", "공간왜곡", "空間歪曲" } },
        {"Label_Anomaly_TimeAnomaly", new() { "Time Anomaly", "시간 이상 현상", "時間異常" } },
        {"Label_Anomaly_CameraMalfunction", new() { "Camera Malfunction", "카메라 오작동", "カメラの誤作動" } },
        {"Label_Anomaly_MonitorAnomaly", new() { "Monitor Anomaly", "모니터 이상 현상", "モニター異常" } },
        {"Label_Anomaly_BeegSana", new() { "!!Beeg Sana!!", "!!Beeg Sana!!", "!!Beeg Sana!!" } },

        //Btns
        { "Label_Start", new() { "START", "시작", "開始" } },
        { "Label_Settings", new() { "SETTINGS", "설정", "設定" } },
        { "Label_Credits", new() { "CREDITS", "크레딧", "クレジット" } },
        { "Label_Quit", new() { "QUIT", "종료", "終了" } },

        {"Label_File_Anomaly", new() { "File an anomaly report", "이상 현상 보고서 제출", "異常現象報告書を提出" } },
        {"Label_Send", new() { "Send", "보내기", "送信" } },
        {"Label_Cancel", new() { "Cancel", "취소", "キャンセル" } },
        {"Label_Anomaly_Where", new() { "Anomaly spotted in:", "이상 현상이 발견된 곳:", "異常現象が発見された場所:" } },
        {"Label_Anomaly_What", new() { "Anomaly type:", "이상 현상 종류:", "異常現象の種類" } },
        {"Label_Report_Pending", new() {"Report pending", "보고서 전송 중", "報告書を送信中"}},

        //Message
        {"Message_Notice_First", new(){
            "!!!ATTENTION FOR AGENT K!!! NUMEROUS ANOMALIES HAVE BEEN DETECTED LATELY. PAY EXTREMELY CLOSE ATTENTION TO THE SURVEILLANCE CAMERA FOOTAGE AND FILE AN ANOMALY REPORT AS SOON AS POSSIBLE WHEN YOU NOTICE SOMETHING CHANGED.",
            "!!!요원 K에게 긴급 통보!!! 최근 다수의 이상 현상이 감지되었습니다. 감시 카메라 영상을 극도로 주의 깊게 관찰하고 무언가 변화가 감지되는 즉시 가능한 한 빨리 이상 현상 보고서를 제출하십시오.", 
            "！！！エージェントKへ緊急通報！！！最近、多数の異常現象が検知されました。監視カメラの映像を極めて注意深く観察し、何らかの変化を検知した場合は、可能な限り速やかに異常現象報告書を提出してください。" } },

        {"Message_Anomaly_Fixed", new() {"Anomaly Fixed", "이상 현상 해결", "異常現象解決" } },
        {"Message_Anomaly_Not_Found", new() {"No anomaly of type \'_2_\' found in \'_1_\'.", "\'_1_\'에서 \'_2_\'이라는 이상 현상은 발견되지 않았습니다.", "「_1_」において「_2_」の異常現象は確認されませんでした。" } },


        { "Language", new() { "Language", "언어", "言語" } },
        { "BGM Volume", new() { "BGM Volume", "배경음악 크기", "BGM音量" } },
        { "Sound Effects Volume", new() { "Sound Effects Volume", "효과음 크기", "効果音音量" } },

        { "Intro_1", new() { "Greetings, Agent K.", "안녕하신가 요원 K.", "ご機嫌いかがかな、エージェントK。" } },
        { "Intro_2", new() { "A timeline overlap has occurred in a specific sector.", "특정 구역에서 시간선 중첩 현상이 발생했다네.", "特定の区域において、時間線の重なり現象が発生している。" } },
        { "Intro_3", new() { "We are currently activating a device to resolve this issue,", "이걸 고치기 위해서 장치를 가동 중이나,", "現在、この問題を修正するため装置を稼働中だが、" } },
        { "Intro_4", new() { "but until the process is complete, you must report any anomalies that occur.", "이 작업이 완료될 때 까지 발생하는 이상 현상을 보고해줘야 한다네.", "発生する異常現象を報告してもらわねばならない。" } },
        { "Intro_5", new() { "Monitor the CCTV feeds carefully", "CCTV 화면을 주의 깊게 살피고", "CCTVの映像を通じて" } },
        { "Intro_6", new() { "and report any anomalies immediately upon detection.", "이상 현상을 발견하면 즉시 보고해주게.", "異常現象を発見し、即座に報告してくれ。" } },
        { "Intro_7", new() { "If too many anomalies go unreported, the process will fail.", "만약 보고되지 않은 이상 현상이 너무 많아질 경우 작업에 실패하게 된다네.", "未報告の異常現象があまりにも多くなると、作業は失敗に終わる。" } },
        { "Intro_8", new() { "Should an intruder appear,", "혹시 침입자가 발생하거든", "侵入者が発生した場合は、" } },
        { "Intro_9", new() { "we recommend switching to another camera feed before filing your report.", "다른 화면으로 이동해서 보고하는 걸 추천드리지.", "別の映像に切り替えてから報告することを推奨する。" } },
        { "Intro_10", new() { "Good luck.", "그럼 행운을 빌겠네.", "幸運を祈る。" } },

        { "Ending_Result", new() { "You have fixed _2_ out of _1_ anomalies.", "_1_개의 이상 현상 중 _2_개를 해결했군.", "_1_件の異常現象のうち、_2_件を解決した。" } },

        { "Ending_GameOver_1", new() { "You have failed the mission, Agent K.", "임무에 실패했군 요원 K.", "任務に失敗したな、エージェントK。" } },
        { "Ending_GameOver_2", new() { "Please be more attentive next time.", "다음엔 조금 더 신경써주게.", "次はもう少し注意してくれ。" } },

        { "Ending_GameEnd_1", new() { "The process has been completed.", "작업이 완료되었네.", "作業は完了した。" } },
        { "Ending_GameEnd_2", new() { "Thank you for your service, Agent K.", "감사를 표하지 요원 K.", "感謝する、エージェントK。" } },

        { "ESCtoClose", new() { "Press ESC to close the window", "ESC를 눌러 창 닫기", "ESCキーを押してウィンドウを閉じる" } },
        { "ClicktoClose", new() { "Click to close the window", "클릭하여 창 닫기", "クリックしてウィンドウを閉じる" } },
        { "Label_Menu", new() { "Menu", "메뉴", "メニュー" } },
    };

    //(Model Name, User Name, Address, CC)
    public static List<(string, string, string, string)> credits = new()
    {
        ("Canonical Hologra Office", "Aeri", "https://skfb.ly/oTyAG", "Creative Commons Attribution"),
        ("Shuba Duck Dinghy", "BobbieVR", "https://skfb.ly/o7uYH", "Creative Commons Attribution"),
        ("Sakura Miko Raft", "BobbieVR", "https://skfb.ly/o7uZF", "Creative Commons Attribution"),
        ("Japanese Tea Cup", "Sam Feng", "https://skfb.ly/6Trov", "Creative Commons Attribution-NonCommercial"),
        ("Japanese snack Dango handpainted", "Bunkichi80", "https://skfb.ly/oz7rp", "Creative Commons Attribution"),
        ("Picture Frame", "Xander Morningstar", "https://skfb.ly/6RDqw", "Creative Commons Attribution"),
        ("Moai", "AtheneaAtlas", "https://skfb.ly/pzDpP", "Creative Commons Attribution-NonCommercial"),
        ("Danganronpa Megaphone Hacking Gun", "Spellkaze", "https://skfb.ly/6WMSx", "Creative Commons Attribution"),
        ("Trash-bin", "Dailsave", "https://skfb.ly/oWuOF", "Free Standard"),
        ("Low Poly | Wall Clock", "VyrianStudios", "https://skfb.ly/6XTut", "Creative Commons Attribution"),
        ("Broom PSX", "Bonvikt", "https://skfb.ly/pwrps", "Creative Commons Attribution"),
        ("Chattini Model", "GustavoMagno", "https://skfb.ly/p86ru", "Creative Commons Attribution"),
        ("Fubuki Shirakami Sukonbu", "Ikxi", "https://skfb.ly/ooCSW", "Creative Commons Attribution-NonCommercial"),
        ("[Hololive EN] Baelz's Baerats", "Welloy", "https://skfb.ly/o9oSv", "Creative Commons Attribution"),
        ("Poyoyo1 - Nakiri Ayame", "drei_icari", "https://skfb.ly/oJJ7B", "Creative Commons Attribution"),
        ("Houshou Marine Pirate Ship", "BobbieVR", "https://skfb.ly/o7uYp", "Creative Commons Attribution"),
        ("fire extinguisher", "Lilith", "https://skfb.ly/p8Ixo", "Creative Commons Attribution"),
        ("1990s Low Poly Camera", "elomation", "https://skfb.ly/oxr7C", "Creative Commons Attribution"),
        ("Low Poly Cartoon Football Ball Free", "chroma3d", "https://skfb.ly/6UyIM", "Creative Commons Attribution"),
        ("Water Cooler", "小林 団那紀", "https://skfb.ly/puOIu", "Free Standard"),
        ("Unbranded conventional Fridge", "assetfactory", "https://skfb.ly/o6WVw", "Free Standard"),

        ("ハコス・ベールズ Hololive MMD", "Hololive Production", "https://3d.nicovideo.jp/works/td94990",  "Cover Corp."),
        ("ラプラス・ダークネス Hololive MMD",  "Hololive Production", "https://3d.nicovideo.jp/works/td84838",  "Cover Corp."),
        ("常闇トワ Hololive MMD",  "Hololive Production", "https://3d.nicovideo.jp/works/td84870",  "Cover Corp."),
        ("フワワ・アビスガード Hololive MMD",  "Hololive Production", "https://3d.nicovideo.jp/works/td84838",  "Cover Corp."),

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