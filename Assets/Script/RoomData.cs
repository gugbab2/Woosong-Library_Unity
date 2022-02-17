using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

//룸 정보만 저장 
public class RoomData : MonoBehaviour
{
    private RoomInfo _roomInfo;
    // 하위에 있는 TMP_Text를 저장할 변수
    private TMP_Text roomInfoText;

    // 프로퍼티 정의
    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;

            // 룸 정보 표시
            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            // 버튼 클릭 이벤트에 함수 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        }
    }

    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
    }
    //룸 들어가는 콜백 메서드
    void OnEnterRoom(string roomName)
    {
        //룸의 속성 정의
        RoomOptions ro = new RoomOptions();

        ro.MaxPlayers = 20; //룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true; //룸의 오픈 여부
        ro.IsVisible = true;//로비에서 룸 목록에 노출시킬지 여부
        // 룸 접속
        PhotonNetwork.JoinOrCreateRoom(roomName,ro,TypedLobby.Default);
    }
}
