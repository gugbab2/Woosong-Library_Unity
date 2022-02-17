using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
public class NPCCtrl_Move : MonoBehaviour
{
    //Npc 이동 경록
    [SerializeField]
    Transform[] PatrolPath = null;
    //네비메쉬
    private NavMeshAgent NMA = null;
    private Animator anim = null;
    private Rigidbody rigid = null;
    private int m_Count = 0;

   

    void Start()
    {
        anim = GetComponent<Animator>();
        NMA = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        NMA.speed = 1.0f;
        InvokeRepeating("MoveToNextWayPoint", 1.0f, 1.0f);
    }
    private void Update()
    {
        InvokeRepeating("MoveToNextWayPoint", 1.0f, 1.0f);
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }
    //고정
    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    //경로 지정
    void MoveToNextWayPoint()
    {
        if (NMA.velocity == Vector3.zero)
        {
            NMA.SetDestination(PatrolPath[m_Count++].position);
            anim.SetBool("isWalk", true);

            if (m_Count >= PatrolPath.Length -1)
                m_Count = 0;
        }
    }
    //NPC모션 트리거
     private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("isWalk", false);
    }
}
