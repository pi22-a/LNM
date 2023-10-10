using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryLever : MonoBehaviour
{
    // 임시 배터리 조작코드
    public GameObject player;
    public GameObject Battery;
    private Animator anim;
    private Animator playerAnim;
    private bool animationExit;
    private bool hasTriggered = false; // OnTrigger가 실행되었는지 추척하는 상태 변수 
    public Transform hangParent; // 플레이어가 붙는 위치의 오브젝트.

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAnim = player.GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.gameObject.transform.position, this.transform.position);

        if (dist < 3f)
        {

            // 매달리는 애니메이션 실행. 
            playerAnim.SetTrigger("StandToFreehang");

            if (Input.GetKey(KeyCode.Mouse0))
            {
                playerAnim.SetBool("HangingIdle", true);
            }
            else
            {
                playerAnim.SetBool("HangingIdle", false);
            }
        }


        if (hasTriggered == true && animationExit == false)
        {
            //print("플레이어의 위치 고정.");
            //player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            player.transform.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Button")
        {
            // 버튼에 닿으면 하늘에서 박스가 열리고 배터리 낙하.
            // 임시 배터리 코드
            Rigidbody rb = Battery.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // 플레이어가 닿으면 
        if (other.gameObject.name.Contains("Player"))
        {
            // 플레어어가 마우스 오른쪽 마우스를 클릭하는 동안 플레이어가 매달려 있는다. 
            hasTriggered = true;

            // 플레이어가 레버에 붙는 코드        
            other.transform.SetParent(hangParent);
            other.transform.position = hangParent.transform.position;

            // 레버 애니메이션을 작동시킨다.
            anim.SetTrigger("LeverStart");
        }
    }
    public void StopLeverAnimationEvent()
    {
        print("레버 이벤트가 종료됨.");
        // 버튼에 닿으면 하늘에서 박스가 열리고 배터리 낙하.
        //@@ JBS 수정
        Rigidbody rb = Battery.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        //
        animationExit = true;

        // 레버 애니메이션 작동이 끝나면
        player.transform.SetParent(hangParent, false);
        player.transform.SetParent(null);
        player.transform.GetComponent<Rigidbody>().isKinematic = false;
    }

    public bool IsCurrentAnimationOver(Animator ani)
    {
        //  애니메이션 시작 : 0 , 애니메이션 종료 : 1 
        // 애니메이션 종료 상태시 : true 반환, 
        return ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;
    }
    
}
