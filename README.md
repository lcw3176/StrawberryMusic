# StrawberryMusic
유튜브를 통한 음악 플레이어

## WPF, MVVM 패턴 적용
이번에 가장 고민한 것이 데이터와 로직의 분리였다.
디자인 패턴을 배웠던 목적이 코드의 명확한 용도별 분류를 통해 유지보수를 편하게 하는 것이 목적이었으나,
Model 쪽에서 데이터와 데이터를 가공하는 로직들이 섞이는 모습을 보고 다른 방식을 써보게 되었다.
Model을 데이터 파트와 각 데이터를 가공하는 Service들로 나누기 위해 노력했다. 

### 기존 내 방식
* Model 클래스에 모든 자료들과 메소드를 작성함
* 클래스의 크기가 커져 가독성이 안좋음
* 만약 Model 클래스에서 데이터를 변경할 경우 ViewModel에 따로 delegate 같은 방식으로 알림을 줘야함

### 새로 해본 방식
* Data와 Service를 분리함
* 데이터와 함수가 나뉘어져 비교적 보기 용이해짐
* Data 클래스 내에 INotifyPropertyChanged를 상속받아, 데이터 변경시 ViewModel에 통보
* Service쪽 클래스에서 데이터를 변경해도 Data 클래스에서 PropertyChanged 이벤트를 발생시켜 주기 때문에 가공이 용이해짐

## Model.Data
* 기능별로 나눠진, 데이터들만 존재하는 클래스들 Ex) YoutubeSearch --> 검색된 비디오 Id, 제목

## Model.~Model.cs
* 각 ViewModel마다 필요한 메소드들이 존재
* Service 클래스들 모임



## 작동 모습
<div>
  <img width="700" src="https://user-images.githubusercontent.com/59993347/91431427-501eeb80-e89b-11ea-81c4-296ca1ba5ca9.png">
  <img width="700" src="https://user-images.githubusercontent.com/59993347/91431428-50b78200-e89b-11ea-9c18-7b244ee9fb2e.png">
  <img width="700" src="https://user-images.githubusercontent.com/59993347/91431430-51501880-e89b-11ea-99a1-2450bb829f85.png">
</div>
