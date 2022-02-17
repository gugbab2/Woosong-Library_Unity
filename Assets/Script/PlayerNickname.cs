using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerNickname : MonoBehaviourPunCallbacks
{
    public TextMesh Nickname;
    public PhotonView pv;

    //사용자 닉네임 동기화 및 자기 닉네임 비활성화
    private void Start()
    {
        if(pv.IsMine)
        {
            gameObject.SetActive(false);
        }

        Nickname.text = pv.Owner.NickName;
    }
}
