using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    #region public, SerializeField 값
    //감지받을 부모 조명 객체 리스트
    [Tooltip("적이 감지 받을 조명 리스트\n자동으로 넣어집니다.")][SerializeField]
    List<GameObject> lightObjParentList;

    //Spot Light 객체 리스트
    [Tooltip("자동으로 넣어지니 수정하지 마세요.\n감지 받을 Spot Light 객체 리스트 입니다.")][SerializeField]
    List<GameObject> lightObjList;

    //@@ 테스트 플-적 거리
    [Tooltip("테스트용: 플레이어-적 간 거리\n단위 : 미터")]
    public float dis;

    //오디오 클립 리스트
    [Tooltip("사용할 오디오 클립 0 : 이동, 1 : 공격\n2 : 잡기")][SerializeField]
    List<AudioClip> emAudioClips;

    //이동 오디오 클립 리스트
    [Tooltip("이동 오디오 리스트")][SerializeField]
    List<AudioClip> emMoveAudioClips;

    //2,3 목적지
    [Tooltip("적2,3용 목적지")][SerializeField]
    Vector3 goal;

    //현재 상태
    [Tooltip("현재 적 객체 행동")][SerializeField]
    EnemyState state;

    #endregion
    //상태 목록
    public enum EnemyState
    {
        Stop, Trace, Attack, TryCatch, Move
    }
    //오디오
    AudioSource emAudio;
    //이동 오디오
    AudioSource emMoveAudio;
    //이전 오디오 인덱스
    int prevAudioIndex = -1;
    //이동 오디오 인덱스
    int emMoveAudioIndex = 0;
    //플레이어
    Transform player;
    //적 스탯
    EnemyStat emStat;
    //내비게이션
    NavMeshAgent emNav;
    //애니메이션 컨트롤러
    Animator emAni;
    //시간 재기1
    float cTime;
    //콜라이더 위치 값 리스트
    List<Vector3> emColPosList;
    //자신 콜라이더
    Collider emCol;
    //콜라이더 경계 값
    Vector3 ext;
    //플레이어 사망 위치
    Transform playerDeathPos;
    
    void Awake()
    {
        //적 스탯 가져오기
        emStat = GetComponent<EnemyStat>();
        if(emStat.enemyType != EnemyStat.EnemyType.Dummy)
        {
            //내비게이션 가져오기
            emNav = GetComponent<NavMeshAgent>();
            //플레이어 가져오기
            //XXX temp
            player = GameObject.FindWithTag("TempPlayer").transform;
            //애니메이터 가져오기
            emAni = GetComponentInChildren<Animator>();
            //오디오 컴포넌트 배열
            AudioSource[] audios = GetComponents<AudioSource>();
            //오디오 가져오기
            emAudio = audios[0];
            //이동 오디오 가져오기
            emMoveAudio = audios[1];
            //태그 Stoppable 가진 조명 부모 객체 가져오기
            GameObject[] lightObjParentAry = GameObject.FindGameObjectsWithTag("Stoppable");
            lightObjParentList = lightObjParentAry.ToList();
            //조명 리스트의 자식 객체인 Spot Light 객체 리스트 생성
            foreach(GameObject lo in lightObjParentList)
            {
                lightObjList.Add(lo.transform.Find("Spot Light").gameObject);
            }
            //자신 콜라이더
            emCol = transform.GetComponentInChildren<Collider>();
            //콜라이더 경계 값
            ext = emCol.bounds.extents;
            //플레이어 사망 위치 가져오기
            playerDeathPos = transform.GetChild(2);
        }
    }

    
    private void Start() {
        if(emStat.enemyType != EnemyStat.EnemyType.Dummy)
        {
            StartCoroutine(CheckRespawnId());
        }
    }
    //리스폰 ID 확인
    IEnumerator CheckRespawnId()
    {
        yield return null;
        int respawnId = GameManager.instance.spawnIdx;
        //리스폰 ID가 1 이상일때 캐처 애니메이션 실행
        if(respawnId == 1)
        {
            if(transform.gameObject.name.Contains("Enemy001"))
                OnDeactivate();
        }
        else if(respawnId > 1)
        {
            OnDeactivate();
        }
        
    }

    private void Update() {
        if(emStat.enemyType != EnemyStat.EnemyType.Dummy)
        {
            //@@ 테스트용
            //GetToPlayerDis();
            //이동 활성화 되면 빛 감지
            if(emStat.IS_ENEMY_MOVE_ACTIVE)
            {
                //계속 조명 활성화, 각도, 노출 확인
                UpdateCheckLight();
            }

            //상태 확인해서 변경
            if(state == EnemyState.Stop)
            {
                UpdateEnemyStop();
            }
            //코루틴으로 변경됨
            //else if(state == EnemyState.Trace)
            //{
            //    UpdateEnemyTrace();
            //}
        }
    }
    #region 정지 상태 UpdateEnemyStop
    //정지 상태
    void UpdateEnemyStop()
    {
        //빛 노출 상태 확인 && 이동 활성화 확인 : 움직일 수 있다면
        if(!emStat.isEnemyStop && emStat.IS_ENEMY_MOVE_ACTIVE)
        {
            ChangeState(EnemyState.Trace);
        }
        //움직일 수 없으면 이동 애니메이션 정지
        else
        {
            emAni.speed = 0;
        }
        //자신의 활성화 타입이 Timer 이고,
        if(emStat.enemyType == EnemyStat.EnemyType.Timer)
        {
            //특정 적 객체가 활성화 되었으면
            if(emStat.checkForTimerEnemy.GetComponent<EnemyStat>().IS_ENEMY_MOVE_ACTIVE)
            {
                //시간 재기
                cTime += Time.deltaTime;
                //시간이 타이머타임을 초과하면 활성화
                if(cTime > emStat.timerTime)
                {
                    cTime = 0;
                    emStat.IS_ENEMY_MOVE_ACTIVE = true;
                }
            }
        }
    }

    #endregion

    #region 오디오 재생 PlayAudio
    //오디오 재생
    ///<summary>
    ///0 : 이동 1 : 공격 2 : 잡기시도
    ///</summary>
    void PlayAudio(int audioIndex)
    {
        //현재 실행중인 클립이 요청받은 클립과 같으면 재생을 끊지 않음
        if(audioIndex == prevAudioIndex)
        {
            if(!emAudio.isPlaying)
            {
                emAudio.Play();
            }

        }
        else
        {
            //오디오 클립을 요청받은 클립으로 변경
            emAudio.clip = emAudioClips[audioIndex];
            //오디오 플레이
            emAudio.Play();
            prevAudioIndex = audioIndex;
        }
        
    }
    //적 이동 발걸음 이벤트
    //이동 오디오 순서대로 재생
    internal void OnEnemyFootstep()
    {
        //3. 해당 인덱스 오디오 클립 재생
        emMoveAudio.clip = emMoveAudioClips[emMoveAudioIndex];
        emMoveAudio.Play();
        //1. 인덱스 증가
        emMoveAudioIndex++;
        //2. 인덱스가 최대길이를 넘으면 0으로 초기화
        if(emMoveAudioIndex >= emMoveAudioClips.Count)
        {
            emMoveAudioIndex = 0;
        }
    }
    

    #endregion
    
    #region 추적, 이동 EnemyTrace
    //추적 상태
    IEnumerator IEEnemyTrace()
    {
        yield return new WaitForSeconds(emStat.enemyMoveDelay);

        while(state == EnemyState.Trace)
        {
            //플레이어와의 거리 재기
            GetToPlayer2DDis();
            //print("");
            //빛 노출 상태 확인 && 이동 활성화 확인 : 움직일 수 있다면
            //or 이동속도가 0이 아니면
            if(!emStat.isEnemyStop && emStat.IS_ENEMY_MOVE_ACTIVE )
            {
                //플레이어가 공격 사거리 내라면 공격 상태로 변경
                if(dis < emStat.emAtkRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                else
                {
                    //이동 사운드 재생
                    PlayAudio(0);
                    //목적지를 설정
                    SetDestination(player.position);
                    //이동 애니메이션 , 속도 1
                    emAni.SetTrigger("emMove");
                    emAni.speed = 1;

                }
            }
            //이동 및 공격 불가시 정지 상태로 변경
            else
            {
                ChangeState(EnemyState.Stop);
            }
            yield return null;
        }
        yield return null;
    }
    //void UpdateEnemyTrace()
    //{
    //    //빛 노출 상태 확인 && 이동 활성화 확인 : 움직일 수 있다면 
    //    if(!emStat.isEnemyStop && emStat.IS_ENEMY_MOVE_ACTIVE)
    //    {
    //        //이동 사운드 재생
    //        PlayAudio(0);
    //        //목적지를 플레이어 위치로 설정
    //        SetDestination(player.position);
    //        //이동 애니메이션 , 속도 1
    //        emAni.SetTrigger("emMove");
    //        emAni.speed = 1;
    //        //임시로 색 변경
    //        transform.GetChild(0).GetComponent<MeshRenderer>()
    //            .material.color = Color.green;
    //    }
    //    else
    //    {
    //        ChangeState(EnemyState.Stop);
    //    }
    //} 

    //특정 목적지까지 조건 없이 이동
    IEnumerator IEEnemyMove()
    {
        //목적지에 도착할때 까지 이동
        SetDestination(goal);
        //사운드
        PlayAudio(0);
        emAni.SetTrigger("emMove");
        emAni.speed = 1;
        //목적지 도착 확인
        while(transform.position != emNav.destination)
        {
            yield return null;
        }
        //도착시 정지 상태로 변경
        ChangeState(EnemyState.Stop);
        
        yield return null;
    }

    #endregion

    #region 빛 감지 관련 함수들
    //빛 감지
    ///<summary>
    /// 1. 현재 콜라이더 각 부분 위치 값 얻기
    /// 2. 해당 부분에 닿은 빛이 있는지 확인
    /// 3. 있다면 isEnemyStop = true
    ///</summary>
    void UpdateCheckLight()
    {
        for(int i = 0 ; i < lightObjList.Count; i++)
        {
            //print($"a1 : {lightObjList[i].activeInHierarchy}");
            //조명 활성화 체크
            if(lightObjList[i].activeInHierarchy)
            {
                //print($"a2 : {lightObjList[i].activeInHierarchy}");
                
                //조명에 노출되었는지 확인
                emStat.isEnemyStop = CheckExposed(lightObjList[i]);

                //하나라도 노출된 조명이 있으면 정지
                if(emStat.isEnemyStop)
                {
                    break;
                }
            }
            //조명 비활성화 이므로 이동 가능
            else
            {
                emStat.isEnemyStop = false;
            }
        }
    }

    //자신과 조명 간의 각도 구하기
    float GetToLightDeg(GameObject lightObj, Vector3 colPos)
    {
        //조명-적 방향 
        Vector3 toLightDir = colPos - lightObj.transform.position;
        
        //조명-적 각도
        float toLightDeg = Vector3.Angle(lightObj.transform.forward, toLightDir);
        //print($"{lightObj.name} 과의 각도 : {toLightDeg}");

        return toLightDeg;
    }

    //조명에 노출되었는지 확인
    //@@ 시간 남으면
    //조명.forward * 쿼터니언.euler(xyz각도) 전방벡터에서 회전된
    //벡터를 구할 수 있음 이걸로 조명의 상하좌우 벡터를 구해서
    //그 방향으로 레이를 쏴서 맞은게 적이면 멈추도록도 해보기
    bool CheckExposed(GameObject lightObj)
    {
        //Light 컴포넌트 가져오기
        Light l = lightObj.GetComponent<Light>();
        //해당 객체와 자신과의 거리
        float dis = Vector3.Distance(lightObj.transform.position, transform.position);
        //사거리 내에 있는지 확인
        if(dis <= l.range)
        {
            //콜라이더 각 부분 위치 구하기
            GetColPosList();
            //부분 마다 각도 구하기
            foreach(Vector3 emColPos in emColPosList)
            {
                float deg = GetToLightDeg(lightObj, emColPos);
                //-spotangle/2 <= deg <= spotangle/2  인지 확인
                if(deg <= l.spotAngle / 2)
                {
                //print($"빛: 각도 {l.spotAngle}");
                //print($"{lightObj.name}과의 콜라이더{emColPosList.IndexOf(emColPos)} 대 각도: {deg}");
                    return true;
                }
            }
        }
        //다 아니면 노출 안됨
        return false;
    }
    //현재 콜라이더 각 부분 위치 값 얻기
    void GetColPosList()
    {
        Vector3 center = emCol.bounds.center;
        Vector3 right = center + new Vector3(ext.x,0,0);
        Vector3 left = center + new Vector3(-ext.x,0,0);
        Vector3 up = center + new Vector3(0,ext.y,0);
        Vector3 down = center + new Vector3(0,-ext.y,0);

        Vector3 upLeft = center + new Vector3(-ext.x,ext.y,0);
        Vector3 upRight = center + new Vector3(ext.x,ext.y,0);
        Vector3 downLeft = center + new Vector3(-ext.x,-ext.y,0);
        Vector3 downRight = center + new Vector3(ext.x,-ext.y,0);

        Vector3 forward = center + new Vector3(0,0,ext.z);
        Vector3 backward = center + new Vector3(0,0,-ext.z);

        Vector3 fUp = center + new Vector3(0,ext.y,ext.z);
        Vector3 fDown = center + new Vector3(0,-ext.y,ext.z);
        Vector3 bUp = center + new Vector3(0,ext.y,-ext.z);
        Vector3 bDown = center + new Vector3(0,-ext.y,-ext.z);
        //콜라이더 리스트에 위치 값 넣기
        emColPosList = new List<Vector3>()
        {
            center,right,left,up,down
            ,upLeft,upRight,downLeft,downRight
            ,forward,backward
            ,fUp,fDown,bUp,bDown
        };
    }

    #endregion

    

    //비활성화 되었을때 작동
    public void OnDeactivate()
    {
        //공격 상태가 아니고
        if(state != EnemyState.Attack && state != EnemyState.TryCatch)
        {
            print(emStat.enemyType);
            //자신이 Catcher 이면
            if(emStat.enemyType == EnemyStat.EnemyType.Catcher)
            {
                ChangeState(EnemyState.TryCatch);
            }
        }
    }

    void EnemyTryCatch()
    {
        if(state != EnemyState.Attack)
        {
            transform.position = emStat.catcherPos;
            transform.rotation = Quaternion.Euler(emStat.catcherEuler);
            //잡기 시도 애니메이션 실행
            emAni.SetTrigger("emLast");
            //잡기 사운드 실행
            PlayAudio(2);

        }
        
    }

    ////현재 위치한 영역 확인 : 현재 미사용
    //void GetMyArea()
    //{
    //    NavMeshHit navMeshHit;
    //    //영역 확인 walkable : 1 , door : 8
    //    if(NavMesh.SamplePosition(emNav.transform.position, out navMeshHit, 1f, NavMesh.AllAreas)) 
    //    {
    //        //문 영역에 위치 했다면
    //        if(navMeshHit.mask == 8)
    //        {
                
    //            emAni.SetTrigger("emLast");
    //            //이후 애니메이션 이벤트로 정지함 OnEnemyLastCatch
    //            //rotation.y = 90
    //            transform.rotation = Quaternion.Euler(0,90,0);
    //            //행동 정지
    //            emStat.IS_ENEMY_MOVE_ACTIVE = false;
    //        }
    //        //print($"현재 영역 : {navMeshHit.mask}"); 
    //    }
    //}

    //상태 변경
    public void ChangeState(EnemyState e)
    {
        //현재 상태 변경
        state = e;
        print($"상태 변경 : {state}");
        //소리 정지
        emAudio.Stop();
        //애니메이션 재생 속도 정상화
        emAni.speed = 1;
        //상태에 따른 초기화 실행
        if(state == EnemyState.Stop)
        {
            //내비 초기화
            ResetNav();
        }
        else if(state == EnemyState.Trace)
        {
            emNav.enabled = true;
            emNav.updatePosition = true;
            StartCoroutine(IEEnemyTrace());
        }
        else if(state == EnemyState.Attack)
        {
            emNav.enabled = true;
            emNav.updatePosition = false;
            //사거리내 플레이어 잔존 확인
            StartCoroutine(IECheckPlayerInAtkRange());
        }
        else if(state == EnemyState.TryCatch)
        {
            emNav.enabled = false;
            //잡기 시도
            EnemyTryCatch();
        }
        else if(state == EnemyState.Move)
        {
            emNav.enabled = true;
            StartCoroutine(IEEnemyMove());
        }
    }
    
    

    

    

    //내비게이션 경로 설정
    void SetDestination(Vector3 pos)
    {
        emNav.destination = pos;
        //XXX lr 로 목적지 경로 그리기
        //lr.positionCount = navi.path.corners.Length;
        //lr.SetPositions(navi.path.corners); 
    }
    //내비게이션 초기화
    void ResetNav()
    {
        emNav.isStopped = true;
        emNav.ResetPath();
        //XXX lr 라인 초기화
        //lr.positionCount = 0;
    }

    #region 공격판정, 공격, 사망
    //플레이어가 공격 사거리 내에 있는지 확인
    IEnumerator IECheckPlayerInAtkRange()
    {
        //공격 애니메이션
        emAni.SetBool("emAttackable",true);
        //공격 딜레이만큼 기다림
        for(float time = 0; time <= emStat.emAtkDelay; time += Time.deltaTime)
        {
            emNav.SetDestination(player.position);
            //적-플 거리
            GetToPlayer2DDis();
            //사거리에서 벗어나면 추적 상태 전환
            if (dis > emStat.emAtkRangeLimit)
            {
                ChangeState(EnemyState.Trace);
                emAni.SetBool("emAttackable", false);
                break;
            }
            yield return null;
        }
        if(dis <= emStat.emAtkRangeLimit)
        {
            //플레이어 사망 코루틴
            StartCoroutine(IEPlayerDeath());
        }
        yield return null;
    }


    //플레이어 사망 처리
    IEnumerator IEPlayerDeath()
    {
        //적 컨트롤러에 플레이어 사망 알림
        EnemyController.instance.PlayerDeath();
        //플레이어를 자신의 자식으로 만듬
        player.parent = transform;
        //공격 사운드 재생
        PlayAudio(1);
        while(emAudio.isPlaying)
        {
            player.position = playerDeathPos.position;
            yield return null;    
        }
        //플레이어 제거 및 게임 오버 처리
        EnemyController.instance.GameOver();
        yield return null;
    }
    //XXX 공격 애니메이션 끝날때 이벤트 현재 미사용
    internal void OnEnemyAttackEnd(){}


    #endregion


    ///<summary>
    ///플레이어-적(자신) 간의 x,z 축 2D 거리 구하기
    ///</summary>
    void GetToPlayer2DDis()
    {
        Vector2 player2DPos = new Vector2(player.position.x,player.position.z);
        Vector2 enemy2DPos = new Vector2(transform.position.x,transform.position.z);

        dis = Vector2.Distance(player2DPos, enemy2DPos);
        //print($"현재 거리: {dis}");
    }
    
    
    //잡기 시도 애니메이션 끝나면 그대로 정지
    internal void OnEnemyLastCatch()
    {
        emAni.speed = 0;
    }
}
