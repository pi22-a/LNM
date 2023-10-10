using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{

    [Header("References")]
    public Transform orientation; // �÷��̾ �����ִ� ����
    public Rigidbody rb;
    public LayerMask whatIsWall; // �������⿡ ���Ǵ� ���� �����ϴ� ���̾� ���� .
    public GameObject wall;
    public PlayerMove mv; // �÷��̾� �̵� ��ũ��Ʈ ����. 
    private Animator anim; // �÷��̾� �ִϸ�����

    [Header("Climbing")]
    private float climbSpeed = 2.9f; // ��� �ӵ�. -> �⺻ 10f
    public float maxClimbTime; // �ӵ� �ִ� ��� �ð�  -> 0.75f
    private float climbTimer; // ��� Ÿ�̸�
    private bool climbing; // ��� ������ Ȯ���ϴ� bool ����

    [Header("Detection")]
    public float detectionLength; // �� ������ ���� �����ϴ� ���� -> 0.7f
    public float sphereCastRadius; // ��ü ĳ��Ʈ �ݰ� -> 0.25f
    public float maxWallLookAngle; // �ִ� �� ���� ���� -> 30f 
    private float wallLookAngle;  // ���� �� ���� ����
    private RaycastHit frontWallHit;// ���� �� ����� ������ �����ϱ� ���� ����ĳ��Ʈ ���� ����
    private bool wallFront; // �տ� ���� �ִ� ��  Ȯ���ϴ� bool ���� 


    // climbSpeed : 10f
    // max Climb Time : 0.75
    
    private void Awake()
    {
        anim = GetComponentInChildren < Animator > ();
    }


    private void Update()
    {
        // �÷��̾� �տ� ���� �ִ� �� �˻��ϴ� �޼��� 
        WallCheck();

        // ���� üũ -> �÷��̾ �������� ������ Ȯ���ϰ�, ��� ������ ���� Ÿ�̸� ���� 
        StateMachine();

        if (climbing )
        {
            ClimbingMovement();
        }

      
    }

    private void WallCheck()
    {     
        // �ִ� ��� ���� ������ ���� ����� �����Ϸ��� ������ ������.
        // �� ���⿡�� �ִ� ���� 30�� �̳����� �÷��̾ �ø����� �� ������ üũ��� wallFront Boolean ���� .
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        // �÷��̾ ���� ��� �ִ� ��� .
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

        // ����� ��ġ�� �����ϱ� �ٷ� ������ collider�� üũ�Ѵ�.
        // �����ΰ� �浹�� �־ ���� �����ϸ�
        // Braced Hang To Crouch �ִϸ��̼��� �۵��ϰ� 
        // �������� �÷��̾ �̵��Ѵ�. 
        
         
    }

    private void StopClimbing()
    {
        climbing = false;
        anim.SetBool("ClimbingUpWall", false);
        // particle effect 
    }

    


}
