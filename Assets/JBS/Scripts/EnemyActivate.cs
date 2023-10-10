using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivate : MonoBehaviour
{
    //이동을 활성화할 적 객체의 스탯 리스트
    [Tooltip("해당 범위에 플레이어가 지날때\n활성화할 적 리스트")]
    public List<EnemyStat> ActivateEnemyList;
    //이동을 비활성화할 적 객체의 스탯 리스트
    [Tooltip("해당 범위에 플레이어가 지날때\n비활성화할 적 리스트")]
    public List<EnemyStat> DeactivateEnemyList;

    //타입 값에 따라 리스트 안 적을 활/비활성화
    //타입 activeType = true, false
    void ActivateEnemy(List<EnemyStat> emList, bool activeType)
    { 
        foreach(EnemyStat emStat in emList)
        {
            emStat.IS_ENEMY_MOVE_ACTIVE = activeType;
            //적 2,3 이고 비활성화시 강제 이동
            if((emStat.gameObject.name == "Enemy002" || emStat.gameObject.name == "Enemy003")
                && activeType == false)
            {
                emStat.EnemyForceMove();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        //충돌한게 플레이어 라면
        //XXX temp
        if(other.gameObject.CompareTag("TempPlayer"))
        {
            //활성화 리스트 적 활성화
            ActivateEnemy(ActivateEnemyList, true);
            //비활성화 리스트 적 비활성화
            ActivateEnemy(DeactivateEnemyList, false);
            //작동 후 자신 비활성화
            gameObject.SetActive(false);
        }
    }    
}


