using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LobbyUI : MonoBehaviour
{
    public GameObject MRPanel;
    //로비 유아이 나가기 버튼
    public void ExitBtn()
    {
        MRPanel.SetActive(false);
    }
}
