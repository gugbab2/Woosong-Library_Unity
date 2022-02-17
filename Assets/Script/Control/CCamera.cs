using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 카메라
public class CCamera : MonoBehaviour
{
    
    public Transform target;
    public float targetY = 1.5f;

    //회전속도
    public float rotSpeed = 80.0f;
    //줌 인/아웃 속도
    public float scrollSpeed = 80.0f;

    //거리
    public float mouseDist = 3.0f;
    public float mouseminDist= 0.1f;
    public float mousemaxDist= 5.0f;
    private Vector3 closeDist; 

    //Y축 회전
    private float yRot;
    //타겟위치
    private Vector3 targetPos;
    //회전값
    private Vector3 dir;

    private void Start()
    {
        closeDist = new Vector3(0, 0, 1.5f);

    }
    void Update()
    {
        mouseControl();
        lookPlayer();
    }

    //카메라 컨트롤 
    void mouseControl()
    {
        yRot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        mouseDist += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        mouseDist = Mathf.Clamp(mouseDist, mouseminDist, mousemaxDist);

        targetPos = target.position + Vector3.up * targetY;

        dir = Quaternion.Euler(0, yRot, 0) * Vector3.forward;
        gameObject.transform.rotation = Quaternion.Euler(30, 0, 0);

        transform.position = targetPos + dir * -mouseDist;
        transform.LookAt(targetPos);
    }

    //카메라 이동 
    void lookPlayer()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, dir, Color.red);
        Physics.Raycast(transform.position, dir, out hit, dir.magnitude * 3, LayerMask.GetMask("Wall"));

        if(hit.point != Vector3.zero)
        {
            transform.position += closeDist;
            transform.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }
}

