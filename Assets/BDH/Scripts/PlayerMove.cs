using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    const float walkSpeed = 1.5f; // 플레이어 걷기 이동 속도.20f
    const float runSpeed = 2f; // 플레이어 달리기 이동 속도.28f
    const float jumpPower = 4f;  // 플레이어 점프 파워.3.5f
    const float jumpDuration = 0.4f; // 플레이어 점프 지속시간.
    const float slideSpeed = 3f; // 플레이어 대쉬 속도.
    const float rotateSpeed = 3f; // 플레이어 회전 속도. 
    const float crouchPosY = 0.5f; // 앉은 상태 위치 
    const float crouchSpeed = 2f; // 플레이어 앉기 이동 속도.
    public GameObject firstHangingPosition; // 플레이어 매달리는 위치_1번.
    public GameObject secondHangingPosition; // 플레이어 매달리는 위치_2번.

    public GameObject threeHangingPosition; // 플레이어 매달리는 위치_3번.

    public HangingStopEvent hangingStopEvent; // 매달리는 애니메이션 종료 이벤트.
    public static bool ground; // 플레이어가 바닥에 착지했는지 여부
    public static bool hanging; // 플레이어 매달리는 여부 변수 
    public static bool isClick = false;
    private Animator anim; // 플레이어 애니메이터.
    private Vector3 dir = Vector3.zero; // 플레이어의 방향 벡터 
    private Vector3 prevDir = Vector3.zero; // 플레이어의 이전 방향 벡터 
    private float applySpeed; // 걷기, 달리기 여부에 따라 속도 처리 변수 
    private bool isRun = false; // 플레이어의 달리기 상태 여부 
    private bool isJumping = false; // 플레이어의 점프 상태 여부 
    private Vector3 jump; // 플레이어의 점프 방향 벡터
    private float jumpStartTime;
    private Rigidbody rb; // Rigidbody 컴포넌트 
    private int layerMask;
    private BoxCollider boxCollider;
    private float originBoxColliderSize; // 원래 상태 boxcollider의 size
    private float originBoxCollidercenter; // 원래 상태 boxcollider의 center
    private float applyCrouchPosY; // 앉은 상태  boxcollide Size Y
    private bool isDirection; // 방향 전환 체크 Boolean 변수 
    private float walkoffset = 0.5f;
    private bool isFall = false; // 플레이어 떨러지는 여부 Boolean 변수 .
    private bool canMove = true; // 플레이어 조작에 대한 컨트롤 허용 변수
    private bool isSliding = false; // 슬라이딩에 조작에 대한 컨트롤 허용 변수 

    RaycastHit downHit;
    RaycastHit fwdHit;


    private void Awake()
    {

        ground = false;
        anim = GetComponentInChildren<Animator>(); // 플레이어(오브젝트)의 컴포넌트(애니메이터)를 초기화한다. 
        boxCollider = GetComponent<BoxCollider>(); // 플레이어(오브젝트)의 BoxCollider(컴포넌트)를 초기화한다.  
        rb = GetComponent<Rigidbody>(); // 플레이어(오브젝트)의 Rigidbody(컴포넌트)를 초기화한다.  
        layerMask = (1 << LayerMask.NameToLayer("Ground")); // Ground 지면의 레이어를 초기화한다. 
    }


    // Start is called before the first frame update
    void Start()
    {
        applySpeed = walkSpeed; // 기본으로 걷는 속도로 적용한다. 
        originBoxColliderSize = boxCollider.size.y; // 현재 플레이어의 boxCollider size y를 초기화한다. 
        originBoxCollidercenter = boxCollider.center.y; // 현재 플레이어의 boxCollider center y를 초기화한다. 
        applyCrouchPosY = originBoxColliderSize; // 앉은 상태 위치 boxCollider y를 현재 플레이어의 boxCollider y로 초기화한다. 
    }

    // 입력은 Update(), 물리처리는 FixedUpdate()로 구분할 것.!

    // 입력은 Update()
    void Update()
    {
        TryMove();
        TryCrouch();
        CheckGround();
        TryJump();
        TryLedgeGrap(firstHangingPosition);
        TryLedgeGrap(secondHangingPosition);

        TryLedgeGrap(threeHangingPosition);

    }

    // 물리처리는 FixedUpdate()
    // Rigidbody를 사용한다면, 트랜스폼을 직접 조작하지 말고, 반드시 FiexedUpdate()에서 Rigidbody통해 사용.
    private void FixedUpdate()
    {

        TrySliding();


    }

    // 사용자가 점프, 마우스 왼쪽을 누르면 매달리기 동작이 실행된다. 
    // 매달린 idle 상태에서 w키를 누르면 올라간다. 
    private void TryLedgeGrap(GameObject ohterGameObject)
    {
        // 플레이어가 매달릴 수 있는 오브젝트와의 거리 측정.

        float dist = Vector3.Distance(this.gameObject.transform.position, ohterGameObject.transform.position); //hangingPosition.transform.position

        if (dist < 2.0f)
        {

            // 매달리는 애니메이션 실행. 
            anim.SetTrigger("StandToFreehang");

            // 사용자가 점프, 마우스 왼쪽을 누르면 매달리기 동작이 실행된다. 
            if (Input.GetKey(KeyCode.Mouse0))
            {
                isClick = true;
              
                // 실제로 매달리는 애니메이션 실행 중.
                anim.SetBool("HangingIdle", true);

                // 매달리는 실행 스크립트. 
                LedgeGrap();

                // 매달린 상태에서 사용자가  w키를 누르면 매달린 상태에서 위로 올라가는 상태로 변경하고
                if (hanging && Input.GetKeyDown(KeyCode.W))
                {

                    // 올라가는 애니메이션을 실행한다. 
                    anim.SetTrigger("BracedHangToCrouch");

                    // 실제로 올라가는 코루틴 실행.
                    StartCoroutine(CloseHangingCube(1.0f));

                    if (hangingStopEvent.start)
                    {
                        print("hangingStopEvent.start");
                        canMove = false;
                        boxCollider.isTrigger = true;

                    }

                }

            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                isClick = false;

            }



            IEnumerator CloseHangingCube(float duration)
            {
                var runTime = 0.0f;

                while (runTime < duration)
                {

                    runTime += Time.deltaTime;

                    transform.position = Vector3.Lerp(transform.position, ohterGameObject.transform.position, (runTime / duration) * 0.1f);
                    yield return null;
                }

                if (runTime >= (duration / 2))
                {
                    print(runTime);
                    canMove = true;
                    rb.useGravity = true;
                    boxCollider.isTrigger = false;
                    hanging = false;
                }


            }

            // 매달리는 애니메이션 종료.
            if (ground)
            {

                anim.SetBool("HangingIdle", false);

                //hanging = false;

            }
        }
    }

    private void LedgeGrap()
    {
        // 플레이어가 매달리지 않은 상태 이면서 아래 방향으로 떨어지는 경우에만 매달릴 수 있다. rb.velocity.y < 0 &&!hanging
        if (true)
        {


            // 첫 번 수직 광선을 발사하여 무엇인가 부딪히면 플레이어는 매달린 상태로 전환될 수 있다. 

            Vector3 lineDownStart = (transform.position + Vector3.up * 4.0f + transform.forward);//+ transform.forward
            Vector3 lineDownEnd = (transform.position + Vector3.up * 0.1f + transform.forward);//+ transform.forward
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, LayerMask.GetMask("HangingObject"));
            Debug.DrawLine(lineDownStart, lineDownEnd, Color.red);

            // 플레이어 첫 번째 수직 광선이 null이 아니면
            if (downHit.collider != null)
            {

                // 플레이어 기준 두 번째 수평 광선을 발사한다. 

                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, LayerMask.GetMask("HangingObject"));
                Debug.DrawLine(lineFwdStart, lineFwdEnd, Color.red);



                // 플레이어는 첫 번째 수직 광선, 두 번째 수평 광선에 무엇인가 부딪히는 조건을 만족하면
                if (fwdHit.collider != null)
                {


                    // 플레이어의 상태를 매달린 상태로 변경하고, 
                    canMove = false;
                    // 플레이어가 매달린 상태에서 갑자기 이동 MOVE 애니메이션이 실행되서 이를 방지하기 위해 종료.
                    anim.SetBool("Move", false);
                    hanging = true;


                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;

                    // 플레이어가 매달릴 위치 hangPos를 지정하고 매달린다. 
                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.2f + transform.up * -0.48f; //-0.1f

                    hangPos += offset;


                    transform.position = hangPos;
                    // normal -> 충돌한 지점에서 면에 수직인 법선벡터이다. 
                    transform.forward = -fwdHit.normal;



                    // 충돌 정보를 초기화한다.
                    downHit = new RaycastHit();
                    fwdHit = new RaycastHit();

                }



            }


        }

    }

    /// <summary>
    /// TrySliding는 플레이어가 달리는 중(Shit left)에 앉기(ctrl)를 누르면 슬라이드 기능이 실행된다.
    /// </summary>
    private void TrySliding()
    {
        if (isSliding == true && Input.GetKeyDown(KeyCode.F))
        {

            // 슬라이드 애니메이션 실행.
            anim.SetTrigger("Slide");
            //Vector3 dashPower = transform.forward * slideSpeed * -Mathf.Log(1 / rb.drag); // 공기저항값 추가.
            Vector3 slidingPower = transform.forward * slideSpeed; // 기본 대쉬 모드. 
            rb.AddForce(slidingPower, ForceMode.VelocityChange);


        }

    }

    /// <summary>
    /// TryJump는 플레이어가 점프하는 동작을 수행하는 메소드이다. 점프 Space를 누르는 시점에 따라 점프 강도가 다르게 설정되어 있다. 
    /// </summary>
    private void TryJump()
    {
        // 플레이어의 점프 중 키를 누르고 있는 시간에 따라 점프 높이를 조절. 
        // 대쉬 기능에서 drag (공기저항값을 추가했을 )
        // -> 해결 방법 : gravity (떨어지는 중력 ) , drag(공기저항값) 두가지를 잘 조절.
        if (Input.GetButtonDown("Jump") && ground && canMove)
        {
            isJumping = true;

            // 점프 사운드 클립 실행.

            // 점프 애니메이션 
            anim.SetTrigger("Jump");

            jump = Vector3.up * jumpPower;

            // 점프 동작이 실행.
            rb.AddForce(jump, ForceMode.VelocityChange);

            jumpStartTime = Time.time; // Time.time은 선언된 시점에서 카운트가 시작된다. 

        }

        if (isJumping && Input.GetButton("Jump") && (Time.time - jumpStartTime < jumpDuration))
        {
            // 첫 점프 누른 후 흐른 시간 - 사용자 점프 누르 시점 카운트 (TIme.time) < jumpDuration (0.5) 

            rb.AddForce(jump * Time.deltaTime, ForceMode.VelocityChange);

        }


        // 점프하고 떨어질때  
        if (rb.velocity.y <= -0.1f && !isFall)
        {
            isFall = true;
            isJumping = false;
            // 떨어지는 애니메이션 실행.
            anim.SetTrigger("Jump_Fall");
        }

        // 플레이어가 바닥에 닺을 때
        if (ground)
        {
            // 점프 사운드 클립 종료.

            // 점프 애니메이션 종료.
            anim.SetTrigger("Landing");
            isFall = false;


        }



    }

    /// <summary>
    /// TryCrouch는 플레이어의(ctrl)을 누른 상태에서 앉기 기능을 실행, ctrl + leftshift를 누르면 앉은 상태에서 달리기를 실행하는 메소드이다. 
    /// </summary>
    private void TryCrouch()
    {
        if (isSliding == false && Input.GetKey(KeyCode.LeftControl))
        {
            // 앉기 사운드 클립 실행.

            // 앉기 애니메이션 실행.
            anim.SetBool("CrouchWalk", true);
            StandingToCrouch();



        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            // 앉기 사운드 클립 종료.

            // 앉기 애니메이션 종료.
            anim.SetBool("CrouchWalk", false);
            CrouchToStanding();

        }
    }

    private void StandingToCrouch()
    {
        applySpeed = crouchSpeed;
        applyCrouchPosY = crouchPosY;

        // 사용자가 앉기 버튼 클릭시 boxCollider의 center : 0.07183719 -> -0.7f, size : 1.2 -> 0.5 낮춘다. 
        boxCollider.center = new Vector3(boxCollider.center.x, -0.7f, boxCollider.center.z);
        boxCollider.size = new Vector3(boxCollider.size.x, applyCrouchPosY, boxCollider.size.z);
    }

    private void CrouchToStanding()
    {
        applySpeed = walkSpeed;
        applyCrouchPosY = originBoxColliderSize;
        // 사용자가 앉기를 종료하면  boxCollider의 높이를 원래 상태로 바꾼다. 
        boxCollider.center = new Vector3(boxCollider.center.x, originBoxCollidercenter, boxCollider.center.z);
        boxCollider.size = new Vector3(boxCollider.size.x, applyCrouchPosY, boxCollider.size.z);
    }


    /// <summary>
    /// TryMove는 사용자의 입력을 받아 플레이어의 방향, 이동, 조작을 하는 메소드이다.  
    /// </summary>
    private void TryMove() // 물리적인 이동, 회전을 사용
    {

        if (canMove)
        {

            // 1-1. 사용자로 부터 플레이어 이동 입력을 받는다. 
            float h = Input.GetAxis("Horizontal"); // 플레이어 사용자 수평 이동 입력 (-1 ~ 1)
            float v = Input.GetAxis("Vertical"); // 플레이어 사용자 수직 이동 입력 (-1 ~ 1)


            // 1-2. 플레이어의 상하좌우 방향을 만든다. -> 현재 방향 계산
            dir = (Vector3.right * h + Vector3.forward * v).normalized;




            // 새로운 방향(dir)과 직전방향(prevDir)을 Dot해서 0.9보다 작다면 다른방향이라고 하고 "다른방향"이라는 bool지역변수에 담아놓고싶다.
            // false -> 같은 방향(이동) , true -> 다른 방향.(회전)
            if (dir != Vector3.zero)
            {
                isDirection = Vector3.Dot(prevDir, dir) < 0.9f;
            }


            // 직전의 방향을 담는 변수에 천천히(Lerp) 기억하고싶다.
            prevDir = Vector3.Lerp(dir, Vector3.forward, 5f * Time.deltaTime);


            if (dir != Vector3.zero)
            {


                // MoveTree 애니메이션 적용.
                anim.SetBool("Move", true);

                // 만약  "다른방향" 이라면 회전하고싶다.
                if (true == isDirection)
                {
                    // 회전 애니메이션 추가

                    Quaternion newRotation = Quaternion.LookRotation(dir);
                    rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, 300f * Time.deltaTime);
                }
                else
                {
                    Quaternion newRotation = Quaternion.LookRotation(dir);
                    rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, 5f * Time.deltaTime);
                }
            }
            else
            {
                // MoveTree 애니메이션 적용 해제.
                anim.SetBool("Move", false);
            }

            // 플레이어 달리기 isRun변수에 따라 applySpeed를 걷는 속도, 달리기 속도로 적용한다. 
            if (Input.GetKey(KeyCode.LeftShift))
            {

                isSliding = true;
                walkoffset = 2.0f;
                isRun = true;
                applySpeed = runSpeed;


            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {

                isSliding = false;
                walkoffset = 0.5f;
                isRun = false;
                applySpeed = walkSpeed;

            }


            // 만약  "다른방향" 이 아니라면
            if (isDirection == false)
            {

                Vector3 moveOffset = dir * (applySpeed * Time.deltaTime);

                //회전 애니메이션 종료 이벤트 발생시
                //{
                // 플레이어의 이동은 rigidbody를 이용하여 구현. P0 = P0 + VT ; 공식.
                // }
                // 플레이어 이동 애니메이션 적용. && 플레이어 앉은 상태에서 이동 애니메이션 적용,
                anim.SetFloat("Horizontal", h * walkoffset);
                anim.SetFloat("Vertical", v * walkoffset);


                rb.MovePosition(rb.position + moveOffset);
            }


        }
    }


    /// <summary>
    /// CheckGround는 Raycast 함수를 이용하여 Vector3.down의 방향으로 플레이어의 지면에 닺는 지 체크하는 메소드이다. 
    /// </summary>
    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, boxCollider.bounds.extents.y + 0.7f, layerMask))
        {
            ground = true;

        }
        else
        {

            ground = false;

        }


    }







}