using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy001_AnimEvent : MonoBehaviour
{
    EnemyMove emMove;
    
    private void Awake() {
        emMove = GetComponentInParent<EnemyMove>();
    }

    //적 마지막 잡기
    public void OnEnemyLastCatch()
    {
        emMove.OnEnemyLastCatch();
    }

    //적 공격
    public void OnEnemyAttackEnd()
    {
        emMove.OnEnemyAttackEnd();
    }

    //적 이동
    public void OnEnemyFootstep()
    {
        emMove.OnEnemyFootstep();
    }
    
}
