using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
public static MenuManager Instance;
    //메뉴 스크립트가 들어있는 게임오브젝트
    [SerializeField] Menu menus;

    private void Awake()
    {
        Instance = this;
    }
    //기능 가져와서 실행
    void Start()
    {
        menus.Open();
    }
}
