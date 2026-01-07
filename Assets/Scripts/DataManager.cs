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
            anomalyType = AnomalyType.Intruder,
        } }
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

        { "Ending_GameOver_1", new() { "You have failed the mission, Agent K.", "임무에 실패했군 요원 K.", "任務に失敗したな、エージェントK。" } },
        { "Ending_GameOver_2", new() { "Please be more attentive next time.", "다음엔 조금 더 신경써주게.", "次はもう少し注意してくれ。" } },

        { "Ending_GameEnd_1", new() { "The process has been completed.", "작업이 완료되었네.", "作業は完了した。" } },
        { "Ending_GameEnd_2", new() { "Thank you for your service, Agent K.", "감사를 표하지 요원 K.", "感謝する、エージェントK。" } },
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