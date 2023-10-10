using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class EnemyStat : MonoBehaviour
{
    //공격 사거리
    [Tooltip("적이 공격 상태가 되는 거리\n 단위 : 미터")]
    public float emAtkRange;
    //공격 유지 사거리
    [Tooltip("적이 공격할 수 있는 최대 사거리\n사거리 보단 커야합니다(작을시 게임 멈춤 현상 발생)\n 단위 : 미터")]
    public float emAtkRangeLimit;
    //사망 판정 지연시간
    [Tooltip("적이 공격으로 죽이기 전 딜레이\n플레이어를 공격해도 설정된 시간까지는 죽이지 않음\n단위 : 초")]
    public float emAtkDelay;
    //빛 노출 여부
    [Tooltip("빛 노출 여부\n빛에 노출되었을시 체크되어 적이 멈춤.")]
    public bool isEnemyStop = false;
    //이동 활성화 여부
    [Tooltip("이동 활성화 여부\n플레이어가 특정 지점 도달 or 일정 시간 이후 체크되어\n해당 적이 플레이어를 추적함.")]
    [SerializeField]bool isEnemyMoveActive = false;
    public bool IS_ENEMY_MOVE_ACTIVE
    {
        get{return isEnemyMoveActive;}
        set
        {
            isEnemyMoveActive = value;
            //이동 비활성화 && 비공격 사망시 적 비활성 행동 실행
            if(!isEnemyMoveActive && !EnemyController.instance.IS_PLAYER_ATTACKED)
            {
                emMove.OnDeactivate();
            }
        }
    }
    //이동 활성화 조건 타입
    public enum EnemyType
    {
        Activator, Timer, Catcher, Dummy
    }
    [Tooltip(@"적 활성화 타입 입니다.
    Activator : 활성화 객체가 활성화
    Timer : 특정 적이 활성화 되고 timerTime 후 활성화
    Catcher : 시작부터 활성화
    Dummy : 활성화 안됨")]
    public EnemyType enemyType;
    //타입 타이머 일때 활성화 시간
    [Tooltip("타입 Timer 일때 사용\n 단위 : 초")]
    public float timerTime;
    //타이머 타입이 활성화 되었는지 확인할 적
    [Tooltip("(타이머 타입만)\n활성화 되었는지 확인할 적")]
    public GameObject checkForTimerEnemy;
    //캐처 일때 이동할 위치
    [Tooltip("타입 Catcher 일때 잡기 시전 시 이동할 위치")]
    public Vector3 catcherPos;
    //'' 방향
    [Tooltip("타입 Catcher 일때 잡기 시전 시 회전")]
    public Vector3 catcherEuler;
    //em 001 (8.54f,0,5.24f);
    //0,90,0
    //em 006 61.51,1.05,9.09
    //18.642,90,0
    
    //이동전 대기 시간
    [Tooltip("이동전 대기 시간\n 단위 : 초")]
    public float enemyMoveDelay = .5f;
    
    //적 이동
    EnemyMove emMove;

    private void Awake() {
        //적 이동 스크립트 가져오기
        emMove = GetComponent<EnemyMove>();

        //오류 예방 : 사거리 > 최대사거리면 동일하게 변경
        if(emAtkRange > emAtkRangeLimit)
        {
            emAtkRangeLimit = emAtkRange;
        }
    }

    private void Start() {
        //리스폰 1이상일때 적 1 비활성화
        if(gameObject.name.Contains("Enemy001"))
        {
            if(GameManager.instance.spawnIdx > 0)
            {
                isEnemyMoveActive = false;
            }
        }
    }

    //적 2,3 비활성화시 목적지 이동 상태 변경
    public void EnemyForceMove()
    {
        emMove.ChangeState(EnemyMove.EnemyState.Move);
    }
}
