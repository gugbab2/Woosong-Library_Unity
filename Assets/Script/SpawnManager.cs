using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

//미팅룸에 객체 생성
public class SpawnManager : MonoBehaviourPunCallbacks
{
    //로비UI 숨김 여부
    public static bool Check;
    //DisConnect을 위한 변수
    private ChatManager chat;

    //트리거 사용을 위한 랜덤 값
    public static int myCheck;

    void Awake()
    {
        CreatePlayers();
    }

    void Start()
    {
        chat = GameObject.Find("ChatManager").GetComponent<ChatManager>();
    }

    //미팅룸에 객체 생성(커스터마이즈 정보에 따라서)
    void CreatePlayers()
    {
        //출현 위치 정보를 배열에 저장
        Transform[] points = GameObject.Find("StartPoint").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1,points.Length);

        switch (WCF.custom)
        {
            case WCF.Custom.male1:
                PhotonNetwork.Instantiate("CustomizePrefabs/BoyCharacter1", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.male2:
                PhotonNetwork.Instantiate("CustomizePrefabs/BoyCharacter2", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.male3:
                PhotonNetwork.Instantiate("CustomizePrefabs/BoyCharacter3", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.male4:
                PhotonNetwork.Instantiate("CustomizePrefabs/BoyCharacter4", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.male5:
                PhotonNetwork.Instantiate("CustomizePrefabs/BoyCharacter5", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.male6:
                PhotonNetwork.Instantiate("CustomizePrefabs/BoyCharacter6", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.female1:
                PhotonNetwork.Instantiate("CustomizePrefabs/GirlCharcter1", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.female2:
                PhotonNetwork.Instantiate("CustomizePrefabs/GirlCharcter2", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.female3:
                PhotonNetwork.Instantiate("CustomizePrefabs/GirlCharcter3", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.female4:
                PhotonNetwork.Instantiate("CustomizePrefabs/GirlCharcter4", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.female5:
                PhotonNetwork.Instantiate("CustomizePrefabs/GirlCharcter5", points[idx].position, points[idx].rotation, 0);
                break;
            case WCF.Custom.female6:
                PhotonNetwork.Instantiate("CustomizePrefabs/GirlCharcter6", points[idx].position, points[idx].rotation, 0);
                break;
        }
    }
    //씬 나기 콜백 메서드 
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Customize");
    }
    //나가기 기능
    public void LeaveRoom()
    {
        //방장권한 다음으로 들어온 사람에게 넘기기
        PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
        //방 떠나기
        PhotonNetwork.LeaveRoom(true);

        Check = true;
        chat.DIsConnect();
    }
}
