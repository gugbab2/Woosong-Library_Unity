using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BestSellerUI : MonoBehaviour
{
    private static string url = "http://localhost:59755/WSUforestService.svc/";

    //WCF_title
    private string[] title = new string[6];
    //WCF_thumbnail
    private string[] thumnail = new string[6];
    //WCF_Bestseller
    private int[] bestSeller = new int[6];

    //BestSellerPanel
    public GameObject BestsellerPanel;

    //BestsellerTitle
    public Text b_text1;
    public Text b_text2;
    public Text b_text3;
    public Text b_text4;
    public Text b_text5;
    public Text b_text6;

    //BestsellerThumbnail
    public RawImage rawImage1;
    public RawImage rawImage2;
    public RawImage rawImage3;
    public RawImage rawImage4;
    public RawImage rawImage5;
    public RawImage rawImage6;

    private void Start()
    {
        GetBestList();

        //thumbnail 출력
        StartCoroutine(GetTexture(rawImage1, thumnail[0]));
        StartCoroutine(GetTexture(rawImage2, thumnail[1]));
        StartCoroutine(GetTexture(rawImage3, thumnail[2]));
        StartCoroutine(GetTexture(rawImage4, thumnail[3]));
        StartCoroutine(GetTexture(rawImage5, thumnail[4]));
        StartCoroutine(GetTexture(rawImage6, thumnail[5]));

        //Text 출력
        b_text1.text = bestSeller[0] + " : " + title[0];
        b_text2.text = bestSeller[1] + " : " + title[1];
        b_text3.text = bestSeller[2] + " : " + title[2];
        b_text4.text = bestSeller[3] + " : " + title[3];
        b_text5.text = bestSeller[4] + " : " + title[4];
        b_text6.text = bestSeller[5] + " : " + title[5];
    }


    //WCF로 실물책 데이터 가져오기
    private void GetBestList()
    {
        string sendurl = url + "Unity_BestSeller";

        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        //string msg = "{\"number\":" + _number + "}";
        string msg = "";

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

            string[] bookInfo1 = result2[1].Split('@');
            string[] bookInfo2 = result2[3].Split('@');
            string[] bookInfo3 = result2[5].Split('@');
            string[] bookInfo4 = result2[7].Split('@');
            string[] bookInfo5 = result2[9].Split('@');
            string[] bookInfo6 = result2[11].Split('@');

            //베스트셀러 제목
            title[0] = bookInfo1[0];
            title[1] = bookInfo2[0];
            title[2] = bookInfo3[0];
            title[3] = bookInfo4[0];
            title[4] = bookInfo5[0];
            title[5] = bookInfo6[0];

            //베스트셀러 썸네일
            thumnail[0] = URLImage(bookInfo1[1]);
            thumnail[1] = URLImage(bookInfo2[1]);
            thumnail[2] = URLImage(bookInfo3[1]);
            thumnail[3] = URLImage(bookInfo4[1]);
            thumnail[4] = URLImage(bookInfo5[1]);
            thumnail[5] = URLImage(bookInfo6[1]);

            //베스트셀러 순위
            bestSeller[0] = int.Parse(bookInfo1[2]);
            bestSeller[1] = int.Parse(bookInfo2[2]);
            bestSeller[2] = int.Parse(bookInfo3[2]);
            bestSeller[3] = int.Parse(bookInfo4[2]);
            bestSeller[4] = int.Parse(bookInfo5[2]);
            bestSeller[5] = int.Parse(bookInfo6[2]);

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

    //패널 비/활성화
    public void ExitBtn()
    {
        BestsellerPanel.SetActive(false);
    }

    //OpenURL
    public void OpenURL()
    {
        Application.OpenURL("https://wsuforestupdate.netlify.app/");
    }
}
