using UnityEngine.UI;
using UnityEngine;
using agora_gaming_rtc;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using agora_utilities;
using Photon.Pun;
using System.Collections;
//말 그대로 메인씬을 컨트롤 하는 스크립트 
public class MainSceneController : MonoBehaviour
{
    //채널이름
    protected string mChannel;
    //캠기능
    private IRtcEngine mRtcEngine;
    //아고라 윈도우 리스트
    readonly List<AgoraNativeBridge.RECT> WinDisplays = new List<AgoraNativeBridge.RECT>();
    //AppID
    public string AppID = "3ab0f8434af0422fbabfdd0b4e24c506";
    //AppID 초기화 여부
    private bool _initialized = false;
    //화면공유중인 인원
    private int CurrentDisplay=0;
    //나의 아이디
    private uint myuid=0;
    //유저 비디오 프리펩
    public GameObject userVideoPrefab;
    //캠 스폰 포인트
    public Transform spawnPoint;
    //스크롤 뷰 content
    public RectTransform content;
    //비디오 캠 포지션 간격
    private float spaceBetweenUserVideos = 300f;
    //유저 캠 리스트
    private List<GameObject> playerVideoList;
    public GameObject P_Menu;
    
    void Start()
    {
        CheckAppId();                               //AppID 확인
        playerVideoList = new List<GameObject>();   //유저 캠 리스트 초기화

        if (mRtcEngine != null)                     //엔진이 있으면 삭제
        {
            IRtcEngine.Destroy();
        } 

        mRtcEngine = IRtcEngine.GetEngine(AppID);   //아고라 엔진 불러오기
        AgoraAtivation();                           //아고라 엔진 활성화
    }
    
    protected bool Menu { get; set; }
    public void Btn_MenuOn()
    {
        Button button = GameObject.Find("Btn_Menu").GetComponent<Button>();
        Menu = !Menu;
        Text text = button.GetComponentInChildren<Text>();
        text.text = Menu ? "메뉴" : "메뉴 닫기";
        if(text.text=="메뉴")
        {
            P_Menu.SetActive(false);
        }
         if(text.text=="메뉴 닫기")
        {
            P_Menu.SetActive(true);
        }      
    }

    #region 아고라 엔진 및 채널 입장
    //아고라 엔진 활성화
    private void AgoraAtivation()
    {
        //string channelName = PhotonNetwork.CurrentRoom.name;    //채널 나누기
        string channelName = "please";                            //채널이름 초기화  

        //채널이름 체크
        if (string.IsNullOrEmpty(channelName))
        {
            Debug.LogError("Channel name can not be empty!");
            return;
        }
        //AppID 체크 
        if (!_initialized)
        {
            Debug.LogError("AppID null or app is not initialized properly!");
            return;
        }

        // 채널 가입
        Join(channelName);

        var winDispInfoList = AgoraNativeBridge.GetWinDisplayInfo();
        if (winDispInfoList != null)
        {
            foreach (var dpInfo in winDispInfoList)
            {
                WinDisplays.Add(dpInfo.MonitorInfo.monitor);
            }
        }
         mRtcEngine.EnableVideo();
         mRtcEngine.EnableVideoObserver();
    }

        //채널 조인
    public void Join(string channel)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        mChannel = channel;

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccess;
        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;
        

            // join channel
            mRtcEngine.JoinChannel(channel, null, 0);

            Debug.Log("initializeEngine done");
        }
        //AppId체크 함수
    private void CheckAppId()
    {
        //조건이 false면 메시지를 호출한다.
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
        if (AppID.Length > 10)
        {

            _initialized = true; //초기화 ok
        }
    }
    #endregion
   
    #region 아고라 엔진 및 채널 떠나기
    public void OnLeaveButtonClicked()  
    {
        Leave();
        UnloadEngine();
    }

    //RTC 채널 떠나기
    private void Leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // 비디오 탐색을 중지
        mRtcEngine.DisableVideoObserver();
    }

    // 아고라 엔진 끄기
    private void UnloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  //엔진 Destroy
            mRtcEngine = null;
        }
    }

    #endregion 
  
    #region 화면공유&중단
    //화면공유
    public void ShareDisplayScreen()
    {
        ScreenCaptureParameters sparams = new ScreenCaptureParameters
        {
            captureMouseCursor = true, //마우스커서까지 화면공유에 포함시키기
            frameRate = 15             //프레임 딜레이
        };

        ShareWinDisplayScreen(CurrentDisplay); //송출
        CurrentDisplay = (CurrentDisplay + 1) % WinDisplays.Count; 
    }

    //화면공유 중단
    public void UnShareDisplayScreen()
    {
            mRtcEngine.StopScreenCapture(); //스크린캡처 중단
            RemoveUserVideoSurface(myuid);
            CheckAppId();                               //AppID 확인
            playerVideoList = new List<GameObject>();   //유저 캠 리스트 초기화

            if (mRtcEngine != null)                     //엔진이 있으면 삭제
            {
                IRtcEngine.Destroy();
            } 

            mRtcEngine = IRtcEngine.GetEngine(AppID);   //아고라 엔진 불러오기
            
            AgoraAtivation();  

            
        
    }

    //해상도 맞추어주면서 송출
    void ShareWinDisplayScreen(int index)
    {
        var screenRect = new Rectangle
        {
            x = WinDisplays[index].left,
            y = WinDisplays[index].top,
            width = WinDisplays[index].right - WinDisplays[index].left,
            height = WinDisplays[index].bottom - WinDisplays[index].top
        };
        Debug.Log(string.Format(">>>>> Start sharing display {0}: {1} {2} {3} {4}", index, screenRect.x,
            screenRect.y, screenRect.width, screenRect.height));
        var ret = mRtcEngine.StartScreenCaptureByScreenRect(screenRect,
            new Rectangle { x = 0, y = 0, width = 0, height = 0 }, default(ScreenCaptureParameters));
    }
    #endregion

    #region  마이크 뮤트 기능(버튼 값 가져오기)
    protected bool MicMuted { get; set; }

    public void Mute()
    {
        Button button = GameObject.Find("Btn_Mute").GetComponent<Button>();

        MicMuted = !MicMuted;
        mRtcEngine.EnableLocalAudio(!MicMuted);
        mRtcEngine.MuteLocalAudioStream(MicMuted);
        Text text = button.GetComponentInChildren<Text>();
        text.text = MicMuted ? "마이크켜기" : "마이크끄기";
    }
    #endregion
    
    #region 어플 관련
    //어플이 렉 먹었을 때 비디오 멈춤 기능
    private void OnApplicationPause(bool paused)
    {
        EnableVideo(paused);
    }
    //강제종료 할 때
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit, clean up...");
        UnloadEngine();
        IRtcEngine.Destroy();
    }
    
    #endregion
  
    #region 캠 켜고 끄기
    //캠 켜고 끄기 
    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null)
        {
            if (!pauseVideo) //만약, 비디오가 안멈췄으면
            {
                //비디오 켜주고
                mRtcEngine.EnableVideo();
            }
            else
            {   //비디오 꺼져
                mRtcEngine.DisableVideo();
            }
        }
    }
    #endregion
    
    #region 콜백
    // 채널 입장 성공시 호출되는 콜백
    protected virtual void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        myuid = uid;  
        CreateUserVideoSurface(uid, true);
    }

    // 나를 기준으로 유저가 들어온 후에 호출되는 콜백
    protected virtual void OnUserJoined(uint uid, int elapsed)  //elapsed : 지연시간 
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);  
        CreateUserVideoSurface(uid, false);    
    }

    // 사용자가 나갈때 콜백
    protected virtual void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {    
       foreach (GameObject player in playerVideoList)
        {
            if (player.name == uid.ToString())
            {
                playerVideoList.Remove(player);
                Destroy(player.gameObject);
                break;
            }
        }
        RemoveUserVideoSurface(uid);   
    }
    #endregion
   
    #region 콜백에서 사용하는 메서드
    //캠 만들기
    private void CreateUserVideoSurface(uint uid, bool isLocalUser)
    {
         // Avoid duplicating Local player VideoSurface image plane.
        for (int i = 0; i < playerVideoList.Count; i++)
        {
            if (playerVideoList[i].name == uid.ToString())
            {
                return;
            }
        }

        // Get the next position for newly created VideoSurface to place inside UI Container.
        float spawnY = playerVideoList.Count * spaceBetweenUserVideos;
        Vector3 spawnPosition = new Vector3(0, -spawnY, 0);

        // Create Gameobject that will serve as our VideoSurface.
        GameObject newUserVideo = Instantiate(userVideoPrefab, spawnPosition, spawnPoint.rotation);
        if (newUserVideo == null)
        {
            Debug.LogError("CreateUserVideoSurface() - newUserVideoIsNull");
            return;
        }
        newUserVideo.name = uid.ToString();
        newUserVideo.transform.SetParent(spawnPoint, false);
        newUserVideo.transform.rotation = Quaternion.Euler(180,180,0);

        playerVideoList.Add(newUserVideo);

        // Update our VideoSurface to reflect new users
        VideoSurface newVideoSurface = newUserVideo.GetComponent<VideoSurface>();
        if(newVideoSurface == null)
        {
            Debug.LogError("CreateUserVideoSurface() - VideoSurface component is null on newly joined user");
            return;
        }

        if (isLocalUser == false)
        {
            newVideoSurface.SetForUser(uid);
        }
        newVideoSurface.SetGameFps(30);

        // Update our "Content" container that holds all the newUserVideo image planes
        content.sizeDelta = new Vector2(0, playerVideoList.Count * spaceBetweenUserVideos + 140);

        UpdatePlayerVideoPostions();
    }

    private void RemoveUserVideoSurface(uint deletedUID)
    {
        foreach (GameObject player in playerVideoList)
        {
            if (player.name == deletedUID.ToString())
            {
                playerVideoList.Remove(player);
                Destroy(player.gameObject);
                break;
            }
        }

        // update positions of new players
        UpdatePlayerVideoPostions();

        Vector2 oldContent = content.sizeDelta;
        content.sizeDelta = oldContent + Vector2.down * spaceBetweenUserVideos;
        content.anchoredPosition = Vector2.zero;
    }

    private void UpdatePlayerVideoPostions()
    {
        for (int i = 0; i < playerVideoList.Count; i++)
        {
            playerVideoList[i].GetComponent<RectTransform>().anchoredPosition = Vector2.down * spaceBetweenUserVideos * i;
        }
    }
    
    bool IDCardChk = false;
    public void clickIDCard()
    {
        RectTransform rectTran = gameObject.GetComponent<RectTransform>();
        GameObject obj = GameObject.Find("신분증");
        Vector3 position = obj.transform.localPosition;
        if (IDCardChk == false)
        {
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1200);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 700);
            position.x = 0;
            position.y = 0;
            obj.transform.localPosition = position;
            IDCardChk = true;
        }
        else
        {
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 470);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
            position.x = -580;
            position.y = 10;
            obj.transform.localPosition = position;
            IDCardChk = false;
        }
        
    }  
   
    

    #endregion

}