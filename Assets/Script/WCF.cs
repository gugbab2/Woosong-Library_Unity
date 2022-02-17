using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine.UI;

//WCF 연결 및 UI
public class WCF : MonoBehaviourPunCallbacks
{

    
    public GameObject Char1;
    public GameObject Char2;
    public GameObject Char3;
    public GameObject Char4;
    public GameObject Char5;
    public GameObject Char6;
    public GameObject Char7;
    public GameObject Char8;
    public GameObject Char9;
    public GameObject Char10;
    public GameObject Char11;
    public GameObject Char12;

    public GameObject FirstPanel;
    public GameObject S_M_P;
    public GameObject S_F_P;
    public GameObject BtnSet_M;

    public enum Custom
    {
        male1,
        male2,
        male3,
        male4,
        male5,
        male6,
        female1,
        female2,
        female3,
        female4,
        female5,
        female6,
    };

    //백준 한단계 다풀기
    //커스텀 정보 저장
    public static Custom  custom;
    //DB 서비스 url
    public static string url = "http://localhost:59755/WSUforestService.svc/";
    //유저 아이디
    public static int UserID = 11111111;    //변경 필요
    //유저 이름
    public static string Name;
    //캐릭터
    public static int Character;
    //선택 여부
    private static bool Check = false;
    //WCF에 커스텀 정보 보내는 변수
    static int cusnum;

    void Awake()
    {
        if (Check == false)
        {
            GetPlayerData();
            
        }
        else
        {
            return;
        }
    }
    
    //게임 종료 콜백 메서드
    public override void OnDisable()
    {
        Gameunjoin();
    }

    #region WCF
    //커스텀 정보 얻어 오는 기능
    private void GetPlayerData()
    {
        string sendurl = url + "Unity_GameJoin";

        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        string msg = "{\"id\":" + UserID + "}";

        byte[] bytes = Encoding.UTF8.GetBytes(msg);
        httpWebRequest.ContentLength = (long)bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);

        string result = null;
        try
        {
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                result = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();

            string[] result2 = result.Split('"');

            string[] player = result2[1].Split('@');
            //이름 받아오기
            Name = player[1];
            Character = int.Parse(player[2]);

            switch(Character)
                {
                    case 1:
                    FirstPanel.SetActive(false);
                    BtnSet_M.SetActive(true);
                    Char1.SetActive(true);
        
                    custom = Custom.male1;
                    Check = true;
                    break;

                    case 2:
                    FirstPanel.SetActive(false);
                    Char2.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.male2;
                    Check = true;
                    break;

                    case 3:
                    FirstPanel.SetActive(false);
                    Char3.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.male3;
                    Check = true;
                    break;

                    case 4:
                    FirstPanel.SetActive(false);
                    Char4.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.male4;
                    Check = true;
                    break;

                    case 5:
                    FirstPanel.SetActive(false);
                    Char5.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.male5;
                    Check = true;
                    break;

                    case 6:
                    FirstPanel.SetActive(false);
                    Char6.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.male6;
                    Check = true;
                    break;

                    case 7:
                    FirstPanel.SetActive(false);
                    BtnSet_M.SetActive(true);
                    Char7.SetActive(true);
                    custom = Custom.female1;
                    Check = true;
                    break;

                    case 8:
                    FirstPanel.SetActive(false);
                    Char8.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.female2;
                    Check = true;
                    break;

                    case 9:
                    FirstPanel.SetActive(false);
                    Char9.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.female3;
                    Check = true;
                    break;

                    case 10:
                    FirstPanel.SetActive(false);
                    Char10.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.female4;
                    Check = true;
                    break;

                    case 11:
                    FirstPanel.SetActive(false);
                    Char11.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.female5;
                    Check = true;
                    break;
                    
                    case 12:
                    FirstPanel.SetActive(false);
                    Char12.SetActive(true);
                    BtnSet_M.SetActive(true);
                    custom = Custom.female6;
                    Check = true;
                    break;
                }
        }
        catch
        {
            Application.Quit();
        }
    }
    //게임 나가기 기능
    private void Gameunjoin()
    {
        string sendurl = url + "Unity_GameUnjoin";

        //송신
        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        //메시지 형식 : {"id":int"}
        string msg = "{\"id\":" + UserID + "}";

        byte[] bytes = Encoding.UTF8.GetBytes(msg);
        httpWebRequest.ContentLength = (long)bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);

        string result = null;
        try
        {
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                result = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();

            if (result == "\"[게임 종료 성공]\"")
            {
                //프로그램 종료
                Debug.Log(result);
            }
        }
        catch
        {
            Debug.Log(result);
        }
    }
    //커스텀 변경 하면 업데이트
    private void CustomUpdate(int SelCustom)
    {
        try
        {
            Debug.Log("selcus" + SelCustom);
            string sendurl = url + "Unity_UpdateCustom";

            // 송신
            HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            // 메시지 형식 : {"id":int,"custom":int}
            string msg = "{\"id\":" + UserID + ",\"custom\":" + SelCustom + "}";

            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            httpWebRequest.ContentLength = (long)bytes.Length;
            using (Stream requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);

            // 수신
            string result;
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                result = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();

            Debug.Log(result);
        }

        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion

    #region UI 버튼
  
    public void FinBtn()
    {   
        CustomUpdate(cusnum);
        BtnSet_M.SetActive(false);
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
    }

    public void ToFirstPanel()
    {
        FirstPanel.SetActive(true);
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        BtnSet_M.SetActive(false);
    }


    public void MaleBtnClick1()
    {
        cusnum=1;
        Char1.SetActive(true);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.male1;
    }

    public void MaleBtnClick2()
    {
        cusnum=2;
        Char2.SetActive(true);
        Char1.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.male2;
    }

    public void MaleBtnClick3()
    {
        cusnum=3;
        Char3.SetActive(true);
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.male3;
    }

    public void MaleBtnClick4()
    {
        cusnum=4;
        Char4.SetActive(true);
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.male4;
    }

    public void MaleBtnClick5()
    {
        cusnum=5;
        Char5.SetActive(true);
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.male5;
    }

    public void MaleBtnClick6()
    {
        cusnum=6;
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(true);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.male6;
    }
    public void FemaleBtnClick1()
    {
        cusnum=7;
        Debug.Log(cusnum);
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char7.SetActive(true);         
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.female1;
    }

    public void FemaleBtnClick2()
    {
        cusnum=8;
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char8.SetActive(true);
        Char7.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.female2;
    }

    public void FemaleBtnClick3()
    {
        cusnum=9;
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char9.SetActive(true);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.female3;
    }

    public void FemaleBtnClick4()
    {
        cusnum=10;
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char10.SetActive(true);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char11.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.female4;
    }

    public void FemaleBtnClick5()
    {
        cusnum=11;
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char11.SetActive(true);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char12.SetActive(false);
        custom = Custom.female5;
    }

    public void FemaleBtnClick6()
    {
        cusnum=12;
        Char1.SetActive(false);
        Char2.SetActive(false);
        Char3.SetActive(false);
        Char4.SetActive(false);
        Char5.SetActive(false);
        Char6.SetActive(false);
        Char12.SetActive(true);
        Char7.SetActive(false);
        Char8.SetActive(false);
        Char9.SetActive(false);
        Char10.SetActive(false);
        Char11.SetActive(false);
        custom = Custom.female6;
    }

    public void S_M_Btn()
    {
        S_M_P.SetActive(false);
        S_F_P.SetActive(true);
    }
    public void S_F_Btn()
    {
        S_M_P.SetActive(true);
        S_F_P.SetActive(false);
    }
    #endregion
}
