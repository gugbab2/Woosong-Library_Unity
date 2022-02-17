using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using UnityEngine.Networking;

public class BookSelect : MonoBehaviour
{
    private PhotonView pv;
    //북ID
    private static int bookID;
    //북type
    private static string type;
    //북title 
    private static string title;
    //북Contents
    private static string contents;
    //북isbn
    private static string isbn;
    //북author
    private static string author;
    //북Publisher
    private static string publisher;
    //북translators
    private static string translators;
    //북thumbnail
    private static string thumnail;
    //북status
    private static string status;
    //북Bestseller
    private static int bestSeller;

    //Bookinfo
    public GameObject Bookinfo_P;
    public Text title1;
    public Text contents1;
    public Text translators1;
    public Text author1;
    public Text publisher1;
    public RawImage thumbnail1;

    public static string url = "http://localhost:59755/WSUforestService.svc/";
    public Text PP;

    // 책 정보 띄우는 패널
    public void Btn_Book1()
    {
        Unity_BookSelect("Java의정석");
        StartCoroutine(GetTexture(thumbnail1, thumnail));
        Bookinfo_P.SetActive(true);
    }
    public void Btn_Book2()
    {
        Unity_BookSelect("혼자공부하는자바");
        StartCoroutine(GetTexture(thumbnail1, thumnail));
        Bookinfo_P.SetActive(true);
    }
    public void Btn_Book3()
    {
        Unity_BookSelect("윤성우열혈C++ 프로그래밍");
        StartCoroutine(GetTexture(thumbnail1, thumnail));
        Bookinfo_P.SetActive(true);
    }
    public void Btn_Book4()
    {
        Unity_BookSelect("조엘온소프트웨어");
        StartCoroutine(GetTexture(thumbnail1, thumnail));
        Bookinfo_P.SetActive(true);
    }
    public void Btn_Book5()
    {
        Unity_BookSelect("Do it! 점프투파이썬");
        StartCoroutine(GetTexture(thumbnail1, thumnail));
        Bookinfo_P.SetActive(true);
    }
    //찜 목록 추가
    public void Btn_AddWishList()
    {
        if(title1.text == "Java의정석")
        {
            Unity_AddWish("10");
        }
        else if(title1.text == "혼자공부하는자바")
        {
            Unity_AddWish("4");
        }
        else if(title1.text == "윤성우열혈C++ 프로그래밍")
        {
            Unity_AddWish("2");
        }
        else if(title1.text == "조엘온소프트웨어")
        {
            Unity_AddWish("5");
        }
        else if(title1.text == "Do it! 점프투파이썬")
        {
            Unity_AddWish("7");
        }
    }
    //찜 목록 해제
    public void Btn_RemoveWish()
    {
        if(title1.text == "Java의정석")
        {
            Unity_RemoveWish("10");
        }
        else if(title1.text == "혼자공부하는자바")
        {
            Unity_RemoveWish("4");
        }
        else if(title1.text == "윤성우열혈C++ 프로그래밍")
        {
            Unity_RemoveWish("2");
        }
        else if(title1.text == "조엘온소프트웨어")
        {
            Unity_RemoveWish("5");
        }
        else if(title1.text == "Do it! 점프투파이썬")
        {
            Unity_RemoveWish("7");
        }
    }
    //찜 추가
    public void Unity_AddWish(string b_id)
    {
        string sendurl = url + "Unity_BookHeart"; 

        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        string msg = "{\"W_id\":" + WCF.UserID.ToString() + ",\"b_id\":" + b_id + "}";
        Debug.Log(msg);

        byte[] bytes = Encoding.UTF8.GetBytes(msg);
        httpWebRequest.ContentLength = (long)bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);

        string result = null;
        
        try{
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                result = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();
            Debug.Log(result);

            string[] result2 = result.Split('"');
            string[] bookInfo = result2[1].Split(',');

            PP.text=bookInfo[1];

        }
        catch(WebException e)
        {
            Debug.Log(e.Message);
        } 
    } 


    //찜 해제
    public void Unity_RemoveWish(string b_id)
    {
        string sendurl = url + "Unity_BookUnHeart"; 

        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        string msg = "{\"W_id\":" + WCF.UserID.ToString() + ",\"b_id\":" + b_id + "}";
        Debug.Log(msg);

        byte[] bytes = Encoding.UTF8.GetBytes(msg);
        httpWebRequest.ContentLength = (long)bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);

        string result = null;
        
        try{
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                result = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();
            Debug.Log(result);
            string[] result2 = result.Split('"');
            string[] bookInfo = result2[1].Split(',');

            PP.text=bookInfo[1];

        }
        catch(WebException e)
        {
            Debug.Log(e.Message);
        }
    }

    #region 책 정보
    public void Unity_BookSelect(string title)
    {
        string sendurl = url + "Unity_BookSelect";

        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        string msg = "{\"title\":\"" + title + "\",\"type\":\"" + "e" + "\"}";
        Debug.Log(msg);

        byte[] bytes = Encoding.UTF8.GetBytes(msg);
        httpWebRequest.ContentLength = (long)bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);

        string result = null;

        try
        {
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                result = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();
            Debug.Log(result);

            string[] result2 = result.Split('"');
            string[] bookInfo = result2[1].Split('@');

            //bookID = int.Parse(bookInfo[0]);
            //type = bookInfo[1];
            title1.text = bookInfo[2];
            title = bookInfo[2];
            contents1.text = bookInfo[3];
            //isbn = bookInfo[5];
            author1.text = bookInfo[5];
            publisher1.text = bookInfo[6];
            translators1.text = bookInfo[7];
            thumnail = bookInfo[8];
            //status = bookInfo[10];
            //bestSeller = int.Parse(bookInfo[11]);

            thumnail = URLImage(thumnail);

        }
        catch (WebException e)
        {
            Debug.Log(e.Message);
        }
    }

    //thumbnail string 재정렬
    private string URLImage(string _thumnail)
    {
        string[] s_thumnail = _thumnail.Split('\\');
        return _thumnail = s_thumnail[0] + s_thumnail[1] + s_thumnail[2] + s_thumnail[3]
                        + s_thumnail[4] + s_thumnail[5] + s_thumnail[6];
    }

    //URL 이미지 텍스쳐로 변경
    IEnumerator GetTexture(RawImage img, string thumnail)
    {
        var url = thumnail;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    #endregion

    //OpenURL
    public void OpenURL()
    {
        Application.OpenURL("https://wsuforestupdate.netlify.app/");
    }

}