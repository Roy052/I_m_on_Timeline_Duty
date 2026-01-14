# I'm on Timeline Duty
<div>
    <h2> 게임 정보 </h2>
    <img src = "https://img.itch.zone/aW1nLzI0OTkxNjY4LnBuZw==/315x250%23c/tSBXZX.png"><br>
    <img src="https://img.shields.io/badge/Unity-yellow?style=flat-square&logo=Unity&logoColor=FFFFFF"/>
    <img src="https://img.shields.io/badge/Horror-black"/>
    <h4> 개발 일자 : 2026.01 <br><br>
    플레이 : https://goodstarter.itch.io/im-on-timeline-duty
    
  </div>
  <div>
    <h2> 게임 설명 </h2>
    <h3> 스토리 </h3>
     시간선 중첩 현상이 발생한 구역에서 <br><br>
     CCTV 모니터를 둘러보며 이상 현상이 발생한 장소를 찾고<br><br>
     그 현상을 제보해 현상을 수정하며 6시까지 버텨 중첩 현상 해결 장치 가동에 성공해야 합니다.
    <h3> 게임 플레이 </h3>
     좌우 화살표로 CCTV 장소를 변경합니다.<br><br>
     이상 현상을 발견한 경우 오른쪽 하단의 버튼으로 장소와 이상 현상 종류를 제보합니다.<br><br>
     침입자의 경우 CCTV 화면을 다른 곳으로 옮긴 뒤 제보합니다.<br><br>
     6시가 될 때까지 이 과정을 반복합니다.<br><br>
  </div> 
  <div>
    <h2> 게임 스크린샷 </h2>
      <table>
        <td><img src = "https://img.itch.zone/aW1hZ2UvNDE5MzEwOS8yNDk5MTY5MC5wbmc=/347x500/uejaCG.png"></td>
        <td><img src = "https://img.itch.zone/aW1hZ2UvNDE5MzEwOS8yNDk5MTY4OS5wbmc=/347x500/Y8FbMi.png"></td>
      </table>
  </div>
    <div>
    <h2> 게임 플레이 영상 </h2>
    
  </div>
  <div>
    <h2> 배운 점 </h2>
    Custom Shader를 활용해 다양한 화면 효과를 만들어보았다.(Noise, Distortion) <br><br>
    MMD 모델을 블렌더를 통해 VRM 모델로 바꾸고 그걸 Unity에 import하는 방법을 배웠다. <br><br>
    
  </div>

   <div>
       <h2> 주요 코드 </h2>
       <h4> Anomaly 시간 계산 </h4>
    </div>
    
```csharp
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

```

