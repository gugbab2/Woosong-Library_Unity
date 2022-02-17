using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FloorButton : MonoBehaviour
{
    public GameObject YButton;
    public GameObject NButton;
    public GameObject FloorUI;

    private GameObject Player;

    private Vector3 FirstFloor;
    private Vector3 SecondFloor;

    void Start()
    {   
        Player = GameObject.FindWithTag("Player");
        FirstFloor = new Vector3(5.0f, -0.04112241f,3.0f);
        SecondFloor = new Vector3(5.0f, 4.448883f, -9.0f);
    }
    #region 계단 올라가기,내려가기,창닫기 버튼 메서드
    public void Moveto1F()
    {
        Player.transform.position = FirstFloor;
        FloorUI.SetActive(false);
    }

    public void Moveto2F()
    {
        Player.transform.position = SecondFloor;
        FloorUI.SetActive(false);
    }

    public void CancleMove()
    {

        FloorUI.SetActive(false);
    }
    #endregion
}
