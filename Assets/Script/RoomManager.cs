using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;
//룸 기능수행(insert, update, delete)
public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameIF;

    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    //룸 목록을 표시할 프리팹
    private GameObject roomItemPrefab;
    //RoomItem 프리팹이 추가될 ScrollContent
    public Transform scrollContent;

    public GameObject roomUI;

    void Awake()
    {
        roomItemPrefab = Resources.Load<GameObject>("RoomItem");
    }   

    //룸 리스트 업데이트 콜백 메서드
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
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

}