using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RealbookUI : MonoBehaviour
{
    private static string url = "http://localhost:59755/WSUforestService.svc/";
         
    //WCF_ID
    private static int bookID;
    //WCF_type
    private static string type;
    //WCF_title 
    private static string title;
    //WCF_Contents
    private static string contents;
    //WCF_isbn
    private static string isbn;
    //WCF_author
    private static string author;
    //WCF_Publisher
    private static string publisher;
    //WCF_translators
    private static string translators;
    //WCF_thumbnail
    private static string thumnail;
    //WCF_status
    private static string status;
    //WCF_Bestseller
    private static int bestSeller;

    //input Title
    public InputField inputField;

    //Realbook_title
    public Text t_Title;
    //Realbook_Contents
    public Text t_Contents;
    //Realbook_Author
    public Text t_Author;
    //Realbook_Publisher
    public Text t_Publisher;
    //Realbook_Thumnail
    public RawImage ri_Thumnail;

    //RealbookPanel
    public GameObject RealbookPanel;
    //infoPanel
    public GameObject infoPanel;

    //p_exception
    public GameObject p_exception;

    #region Search Event
    
    public void e_realBookSearch()
    {
        //enter key
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            b_realBookSearch();
        }
    }

    private static bool differentWord = true;
    public void b_realBookSearch()
    {
        if (differentWord)
        {
            InfoExit();
            p_exception.SetActive(true);
        }

        if (StringAvailable(inputField.text))
        {
            //bring DB 
            GetBookList(inputField.text);

            //initialization DB
            t_Title.text = title;
            t_Contents.text = contents;
            t_Author.text = author;
            t_Publisher.text = publisher;
            StartCoroutine(GetTexture(ri_Thumnail));
            
            if (!differentWord)
            {
                p_exception.SetActive(false);
                InfoBtn();
                differentWord = true;
            }
        }

        this.inputField.text = "";
    }

    IEnumerator GetTexture(RawImage img)
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

    #region bring DB 

    //WCF Bring DB 
    private void GetBookList(string _title)
    {
        string sendurl = url + "Unity_BookSelect";

        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        string msg = "{\"title\":\"" + _title + "\",\"type\":\"" + "real" + "\"}";

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

            bookID = int.Parse(bookInfo[0]);
            type = bookInfo[1];
            title = bookInfo[2];
            contents = bookInfo[3];
            isbn = bookInfo[4];
            author = bookInfo[5];
            publisher = bookInfo[6];
            translators = bookInfo[7];
            thumnail = bookInfo[8];
            status = bookInfo[9];
            bestSeller = int.Parse(bookInfo[10]);

            if (title == "")
                differentWord = true;
            else if (title != "")
                differentWord = false;

            //thumnail Rearrange
            string[] s_thumnail = thumnail.Split('\\');
            thumnail = s_thumnail[0] + s_thumnail[1] + s_thumnail[2] + s_thumnail[3]
                + s_thumnail[4] + s_thumnail[5] + s_thumnail[6];
            //https:\/\/library.wsu.ac.kr\/Sponge\/Images\/bookDefaults\/MMbookdefaultsmall.png
        }
        catch (WebException e)
        {
            Debug.Log(e.Message);
        }
    }

    //check InputField
    bool StringAvailable(string inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField)) return false;
        return true;
    }



    #endregion


    #region another method
    //panel Setactive
    public void ExitBtn()
    {
        RealbookPanel.SetActive(false);
    }
    public void InfoBtn()
    {
        infoPanel.SetActive(true);
    }
    public void InfoExit()
    {
        infoPanel.SetActive(false);
    }

    //OpenURL
    public void OpenURL()
    {
        Application.OpenURL("https://wsuforestupdate.netlify.app/");
    }
    #endregion
}
