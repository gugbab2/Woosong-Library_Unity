using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    //타겟이 된 게임 오브젝트 활성화 기능
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }
}
