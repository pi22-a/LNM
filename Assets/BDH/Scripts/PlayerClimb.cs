using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{

    [Header("References")]
    public Transform orientation; // 플레이어가 보고있는 방향
    public Rigidbody rb;
    public LayerMask whatIsWall; // 기어오르기에 사용되는 벽을 감지하는 레이어 변수 .
    public GameObject wall;
    public PlayerMove mv; // 플레이어 이동 스크립트 참조. 
    private Animator anim; // 플레이어 애니메이터

    [Header("Climbing")]
    private float climbSpeed = 2.9f; // 상승 속도. -> 기본 10f
    public float maxClimbTime; // 속도 최대 상승 시간  -> 0.75f
    private float climbTimer; // 상승 타이머
    private bool climbing; // 상승 중인지 확인하는 bool 변수

    [Header("Detection")]
    public float detectionLength; // 벽 감지를 위해 감지하는 길이 -> 0.7f
    public float sphereCastRadius; // 구체 캐스트 반경 -> 0.25f
    public float maxWallLookAngle; // 최대 벽 보기 각도 -> 30f 
    private float wallLookAngle;  // 현재 벽 보기 각도
    private RaycastHit frontWallHit;// 전면 벽 헤드의 정보를 저장하기 위한 레이캐스트 적중 변수
    private bool wallFront; // 앞에 벽이 있는 지  확인하는 bool 변수 


    // climbSpeed : 10f
    // max Climb Time : 0.75
    
    private void Awake()
    {
        anim = GetComponentInChildren < Animator > ();
    }


    private void Update()
    {
        // 플레이어 앞에 벽이 있는 지 검사하는 메서드 
        WallCheck();

        // 상태 체크 -> 플레이어가 기어오르는 조건을 확인하고, 기어 오르는 동안 타이머 관리 
        StateMachine();

        if (climbing )
        {
            ClimbingMovement();
        }

      
    }

    private void WallCheck()
    {     
        // 최대 등반 보기 각도에 따라 등반을 시작하려는 각도가 정해짐.
        // 앞 방향에서 최대 각도 30도 이내에서 플레이어가 올르려고 할 때인지 체크라는 wallFront Boolean 변수 .
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        // 플레이어가 지상에 닿아 있는 경우 .
        if (PlayerMove.ground)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StateMachine()
    {
        // state 1 - climbing
        if(wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle )
        {
            if(!climbing && climbTimer > 0) StartClimbing();
          
            if (climbTimer > 0) climbTimer -= Time.deltaTime;

            if (climbTimer < 0) StopClimbing();
        }
        else 
        {
            if (climbing) StopClimbing();
        
        }

      
    }

   

    private void StartClimbing()
    {
        climbing = true;

        // camera fov change 
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        anim.SetBool("ClimbingUpWall", true);

        // 꼭대기 위치에 도착하기 바로 직전에 collider를 체크한다.
        // 무엇인가 충돌이 있어난 것을 감지하면
        // Braced Hang To Crouch 애니메이션을 작동하고 
        // 꼭대기까지 플레이어가 이동한다. 
        
         
    }

    private void StopClimbing()
    {
        climbing = false;
        anim.SetBool("ClimbingUpWall", false);
        // particle effect 
    }

    


}
