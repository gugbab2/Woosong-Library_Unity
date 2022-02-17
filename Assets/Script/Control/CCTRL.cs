using UnityEngine;
using Photon.Pun;
using System.Collections;

//캐릭터 컨트롤
public class CCTRL : MonoBehaviour
{
    //포톤 동기화 
    private PhotonView pv;
    public static CCTRL instance;
    public GameObject MainCamera;
    public GameObject player;

    //player sit
    private bool check_Sit = false;
    //마우스 체크
    private bool check_Mouse = true;
    //player rigidbody
    public static Rigidbody rg_Player;
    //앉기 이전 값
    Vector3 priviousPosition;

    //클릭한 오브젝트 값
    GameObject target = null;
    //의자 배열
    GameObject[] chairs;
    //의자 객체
    static GameObject chair;

    private Transform tr;
    public float moveSpeed = 5.0f;
    public float turnSpeed = 1.5f;
    private Animator anim;

    private Transform Panel_RealBook;
    private Transform Panel_Lobby;
    private Transform Panel_BestSeller;

     public GameObject literature;
    public GameObject SF;
    public GameObject history;
    public GameObject non_Fiction;
    public GameObject BookLit;
    public GameObject BookSF;
    public GameObject BookHis;
    public GameObject BookNon;
    public GameObject Floor1Panel;
    public GameObject Floor2Panel;


    private EmotionCtrl emocctrl;


    void Awake()
    {
        pv = GetComponent<PhotonView>();
        rg_Player = player.GetComponent<Rigidbody>();
    }

    void Start()
    {
        emocctrl = player.GetComponent<EmotionCtrl>();

        chairs = GameObject.FindGameObjectsWithTag("Chair1");
        if(PhotonNetwork.CurrentRoom.Name == "WooSoong Library")
        {
            Panel_RealBook = GameObject.Find("Canvas"). transform.Find("RealbookPanel");
            Panel_Lobby =  GameObject.Find("Canvas").transform.Find("MeetingroomPanel");
            Panel_BestSeller = GameObject.Find("Canvas").transform.Find("BestSeller");
        }
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        
        if(!pv.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(MainCamera);
        }
    }


    void Update()
    {
        if (pv.IsMine)
        {
            if (check_Sit == false)
            {
                anim.SetBool("Idle", true);
                //StartCoroutine(CheckIdleState());
                CharacterMove();
                
            }

            if (Input.GetMouseButtonDown(0) && (GetClickedObject().tag == "Chair1") && check_Mouse == true)
            {
                priviousPosition = player.transform.position;
                SitDown();
                anim.SetBool("Idle", false);
                anim.SetBool("isSit", true);
                StartCoroutine(CheckAnimationState());
                rg_Player.constraints = RigidbodyConstraints.FreezeAll;

            }
            else if (Input.GetMouseButtonDown(1) && check_Sit == true && check_Mouse == false)
            {
                standUp();
                anim.SetBool("isSit", false);
            }
            //NPC콜라이더에 태그 달아주기
            if (Input.GetMouseButtonDown(0) &&  (GetClickedObject().tag == "NPC_EBook"))
            {
                Panel_RealBook.gameObject.SetActive(true);
            }
            else if (Input.GetMouseButtonDown(0) &&  (GetClickedObject().tag == "NPC_Lobby"))
            {

                Panel_Lobby.gameObject.SetActive(true);
            }
            else if (Input.GetMouseButtonDown(0) && (GetClickedObject().tag == "Bestseller"))
            {

                Panel_BestSeller.gameObject.SetActive(true);
            }
        }

        else if(!pv.IsMine)
            return;
    }

    //의자에 앉아있을때 감정표현(이모티콘) 막기
    IEnumerator CheckAnimationState()
    {

	    while (anim.GetCurrentAnimatorStateInfo(0).IsName("isSit")) 
	    { 
            anim.SetBool("isCry", false);
            anim.SetBool("isClap", false);
            anim.SetBool("isLaugh", false);
            anim.SetBool("isDizzy", false);
		    yield return null;
	    }

    }

    IEnumerator CheckIdleState()
    {

	    while (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) 
	    { 
            if(emocctrl.enabled == false)
                
		    yield return null;
	    }
        emocctrl.enabled = true;
    }

    //캐릭터 이동
    void CharacterMove()
    {
        float h = Input.GetAxis("SideMove");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Horizontal");

        if (Mathf.Approximately(h, 0) && Mathf.Approximately(v, 0) && Mathf.Approximately(r, 0))
        {
            anim.SetBool("isWalk", false);
        }
        else
        {
            anim.SetBool("isWalk", true);
        }

        anim.SetFloat("xDir", h);
        anim.SetFloat("yDir", v);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);
    }

    //레이캐스팅을 사용해 의자 객체 가져오기
    private GameObject GetClickedObject()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);      
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))     
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
    
    //앉기 로직
    private  void SitDown()
    {
        check_Sit = true;
        check_Mouse = false;
        if (pv.IsMine && check_Sit == true)
        {
            //의자 객체 가져와서 포지션 변경(의자에 맞추어서)
            chair = GetClickedObject();
            player.transform.position = new Vector3(chair.transform.position.x,
                                                    chair.transform.position.y - 1.8f,
                                                    chair.transform.position.z);
            //방향을 변경한다.
            player.transform.rotation = chair.transform.rotation;

            chair.SetActive(false);
        }
    }

    //일어서기 로직
    private void standUp()
    {
        check_Sit = false;
        check_Mouse = true;
        if (pv.IsMine && check_Sit == false)
        {
            player.transform.position = new Vector3(priviousPosition.x,
                                                    priviousPosition.y,
                                                    priviousPosition.z);

            chair.SetActive(true);
        }
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if(pv.IsMine)
        {
            if(other.tag=="Literature")
            {
                literature.SetActive(true);
            }
            else if(other.tag=="SF")
            {
                SF.SetActive(true);
            }
            else if(other.tag=="History")
            {
                history.SetActive(true);
            }
            else if(other.tag=="Non-Fiction")
            {
                non_Fiction.SetActive(true);
            }
            else if(other.tag=="1F")
            {
                Floor1Panel.SetActive(true);
            }
            else if(other.tag=="2F")
            {
                Floor2Panel.SetActive(true);
            }
        }
              
    }


    public void BookBtnLit()
    {
        if(pv.IsMine)
        {
            BookLit.SetActive(true);
        }
    }
    public void BookBtnSF()
    {
        if(pv.IsMine)
        {   
            BookSF.SetActive(true); 
        }
    }

    public void BookBtnHis()
    {
        if(pv.IsMine)
        {
            BookHis.SetActive(true); 
        }
    }

    public void BookBtnNon()
    {
        if(pv.IsMine)
        {
            BookNon.SetActive(true);
        }
    }



    public void ExitBtn()
    {
        if(pv.IsMine)
        {
            literature.SetActive(false);
            SF.SetActive(false);
            history.SetActive(false);
            non_Fiction.SetActive(false);
        }
    }
}