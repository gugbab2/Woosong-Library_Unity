using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CustomizeUI : MonoBehaviour
{
    public GameObject CPanel;
    public GameObject MPanel;
   
    //나가기 선택
    public void ExitBtnClick()
    {  
        Application.Quit();
    }
    //타이틀 숨기기
    public void TitleBtnClick()
    {
        CPanel.SetActive(false); 
        MPanel.SetActive(true);
    }
}
