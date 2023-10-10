using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //플레이어가 공격받아 사망 여부
    [SerializeField][Tooltip("플레이어가 공격받았는지 여부")]
    bool isPlayerAttacked = false;
    //플레이어
    Transform player;
    //플레이어 애니메이터
    Animator playerAni;

    public bool IS_PLAYER_ATTACKED
    {
        get{return isPlayerAttacked;}
        set
        {
            isPlayerAttacked = value;
        }
    }
    public static EnemyController instance;
    
    private void Awake() {
        instance = this;
        IS_PLAYER_ATTACKED = false;
        //플레이어 가져오기
        player = GameObject.FindWithTag("TempPlayer").transform;
        //플레이어의 애니메이터 가져오기
        playerAni = player.Find("LittleNightMares").GetComponent<Animator>();
    }

    //플레이어 사망
    public void PlayerDeath()
    {
        IS_PLAYER_ATTACKED = true;
        //모든 적 객체 가져오기
        GameObject[] allEnemyAry = GameObject.FindGameObjectsWithTag("Enemy");
        //모든 적 객체를 이동 비활성화
        foreach(GameObject enemy in allEnemyAry)
        {
            enemy.GetComponent<EnemyStat>().IS_ENEMY_MOVE_ACTIVE = false;
        }
        //플레이어 이동,충돌,리지드바디 비활성화
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<BoxCollider>().enabled = false;
        player.GetComponent<Rigidbody>().useGravity = false;
        //플레이어 이동,오르기,앉기 상태 해제 = 대기 애니메이션 실행
        playerAni.SetBool("Move",false);
        playerAni.SetBool("ClimbingUpWall",false);
        playerAni.SetBool("CrouchWalk",false);
    }
    //플레이어 제거 및 게임 오버 처리
    public void GameOver()
    {
        print("적이 플레이어를 사망시킴");
        Destroy(player.gameObject);
        GameManager.instance.EndGame();
    }
}
