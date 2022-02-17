using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

//포톤 서버와 연결 로직
public class PhotonManager : MonoBehaviourPunCallbacks
{
     //게임의 버전
     private readonly string version = "1.0";

     //룸 이름을 입력할 TextMeshPro Input Field
     public TMP_InputField roomNameIF;

    [SerializeField]
    GameObject roomChoice;
    [SerializeField]
    GameObject first_Cus;

    //룸 목록에 대한 데이터를 저장하기 위한 딕셔너리 자료형
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
     //룸 목록을 표시할 프리팹
     private GameObject roomItemPrefab;
     //RoomItem 프리팹이 추가될 ScrollContent
     public Transform scrollContent;

     void Awake()
     {
         PhotonNetwork.NickName = WCF.Name;
         //마스터 클라이언트 씬 자동 동기화 옵션
         PhotonNetwork.AutomaticallySyncScene = true;
         //게임의 버전 설정
         PhotonNetwork.GameVersion = version;
         
         //RommItem 프리팹 로드
         roomItemPrefab = Resources.Load<GameObject>("RoomItem");

         if(PhotonNetwork.IsConnected == false)
             PhotonNetwork.ConnectUsingSettings();
     }

    void Start()
    {
        if(SpawnManager.Check)
        {
            roomChoice.SetActive(true); 
            first_Cus.SetActive(false);
        }
    }

    //룸 명의 입력 여부를 확인하는 로직
    public string SetRoomName()
    {
        if(string.IsNullOrEmpty(roomNameIF.text))
            roomNameIF.text = $"ROOM_{UnityEngine.Random.Range(1,101):000}";

        return roomNameIF.text;
    }
    
    #region 포톤 콜백 메서드
    //포톤 서버에 접속 후 호출되는 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");

        PhotonNetwork.JoinLobby();
    }

    //로비의 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
    }

    //룸 입장에 실패했을 경우 호출되는 콜백 함수 
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRoom Filed{returnCode}:{message}");

        MainRoomCreate("WooSoong Library");
    }

    //룸 생성이 완료된 후 호출되는 콜백 함수 
    public override void OnCreatedRoom()
    {      
         Debug.Log($"Create Room , Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }
   
    //룸에 입장한 후 호출되는 콜백 함수 
    public override void OnJoinedRoom()
    {
         Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}, Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");
        
         foreach (var player in PhotonNetwork.CurrentRoom.Players)
             Debug.Log($"{player.Value.NickName},{player.Value.ActorNumber}");
         
         if(PhotonNetwork.CurrentRoom.Name == "WooSoong Library")
             PhotonNetwork.LoadLevel("Library");
        
         if(PhotonNetwork.CurrentRoom.Name == roomNameIF.text)
            PhotonNetwork.LoadLevel("MeetingRoom");
    }
    //룸 리스트 업데이트
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //삭제된 RoomItem 프리팹을 저장할 임시변수
        GameObject tempRoom = null;
       foreach(var roomInfo in roomList)
       {
          //룸이 삭제된 경우
          if(roomInfo.RemovedFromList == true)
          {
              //딕션너리에서 룸 이름으로 검색해 저장된 RommItem 프리팹를 추출
              rooms.TryGetValue(roomInfo.Name,out tempRoom);

              //RoomItem 프리팹 삭제
              Destroy(tempRoom);

              //딕셔너리에서 해당 룸 이름의 데이터를 삭제
              rooms.Remove(roomInfo.Name);
          }
          else //룸 정보가 변경된 경우
          {
              //룸 이름이 딕셔너리에 없는 경우 새로추가
              if(rooms.ContainsKey(roomInfo.Name)==false)
              {
                  //RoomInfo 프리팹을 scrollContent 하위에 생성
                  GameObject roomPrefab = Instantiate(roomItemPrefab,scrollContent);
                  //룸 정보를 표시하기 위해 RoomInfo 정보 전달
                  roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;

                  //딕셔너리 자료형에 데이터 추가
                   rooms.Add(roomInfo.Name,roomPrefab);
              }
              else //룸 이름이 딕셔너리에 없는 경우에 룸 정보를 갱신
              {
                  rooms.TryGetValue(roomInfo.Name, out tempRoom);
                  tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;
              }
          }
          Debug.Log($"Room={roomInfo.Name}({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})");
       }
    }
    #endregion

    #region UI_BUTTON_EVENT
    //미팅 룸 생성
    public void OnMakeRoomClick()
    {
        RoomOptions ro = new RoomOptions();
        
        ro.MaxPlayers = 5;                  //룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;                   //룸의 오픈 여부
        ro.IsVisible = true;                //로비에서 룸 목록에 노출시킬지 여부
        PhotonNetwork.CreateRoom(SetRoomName(),ro);
    }

    //메인룸 고정
    public void MainRoomCreate(string room)
    {
        //룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;                 //룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true;                   //룸의 오픈 여부
        ro.IsVisible = false;               //로비에서 룸 목록에 노출시킬지 여부

        //룸 생성
        PhotonNetwork.CreateRoom(room, ro);
    }

    //도서관 룸으로 이동
    public void JoinRoom_Library()
    {
        PhotonNetwork.JoinRoom("WooSoong Library");
    }
    #endregion
}