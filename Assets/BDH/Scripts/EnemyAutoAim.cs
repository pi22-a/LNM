using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class EnemyAutoAim : MonoBehaviour
{
    public Transform Target; // 자동에임 타겟으로 지정할 Transform 객체를 설정.
    public GameObject spotLight; // 후레쉬 오브젝트의 하위 오브젝트 .
    public GameObject flashy; // 가짜 후레쉬 오브젝트.
    public GameObject rigPlayer;// 리깅 플레이어 오브젝트.
    public Rig HeadRig; // Rigging을 적용할 Rig 객체 설정.
    public float RetargetSpeed = 5f; // 타겟 위치를 추적하는 속도를 설정하는 변수.d
    private RigBuilder rigBuilder;


    //private GameObject chaseTarget; // 자동 에임할 대상 적.
    private int layer; // 적 레이어 변수.

    float rigWeight = 1;

    private void Awake()
    {
        layer = 1 << LayerMask.NameToLayer("Enemy"); // 적 레이어
        rigBuilder = rigPlayer.GetComponent<RigBuilder>();

    }


    // Update is called once per frame
    void Update()
    {
        // 자동 에임할 대상 적.
        GameObject chaseTarget = null;
        // 자동 에임할 대상 적의 위치. 
        Vector3 chaseTargetPos;

        // OverlapSphere의 spotLight.GetComponent<Light>().range 범위로 적(Enemey)를 모두 식별함 
        Collider[] colAry = Physics.OverlapSphere(spotLight.transform.position, spotLight.GetComponent<Light>().range, layer);

        // OverlapSphere의 범위에 적(Enemey)이 존재하면.
        if (colAry.Length > 0)
        {
            // 식별한 적 중에 가장 가까운 적을 가져온다.d
            chaseTarget = colAry[0].gameObject;
            float minDist = Vector3.Distance(spotLight.transform.position, colAry[0].gameObject.transform.parent.position);

            // 각도 계산.GetToLightDeg && 거리 계산이후 내부 범위에 있다면 
            if (CheckExposed(chaseTarget) == true)
            {

                // 식별한 적들 중 가장 가까운 적을 계속적으로 업데이트.
                for (int i = 1; i < colAry.Length; i++)
                {
                    float dist = Vector3.Distance(spotLight.transform.position, colAry[i].gameObject.transform.parent.position);


                    if (dist < minDist)
                    {
                        minDist = dist;
                        chaseTarget = colAry[i].gameObject;

                    }

                }

                // 후레쉬를 chaseTarget를 향해서 에임 처리를 한다. 
                // 조건 : 플레이어의 앞 방향에서 가장 가까운 거리에 있는 적만 에임할 것인지 ,,,? 
                // 타겟 향하는 방향을 계산.
                //Vector3 targetDirection = (chaseTarget.transform.parent.position - spotLight.transform.position).normalized;

                // 후레쉬 앞을 타겟 적으로 향하게 함.
                //spotLight.transform.forward = new Vector3(targetDirection.x, chaseTarget.GetComponent<BoxCollider>().center.y + 0.25f, targetDirection.z);

                //print(minDist + "가장 가까운 적은 : " + chaseTarget.transform.parent.name);


            }



        }

        // 자동에임 타겟팅한 적을 찾는 경우 상태에서 후레쉬만 켜져있는 경우.
        // if(chaseTarget != null && spotLight.activeSelf == true)
        //{
        //   rigWeight = 1;
        //   chaseTargetPos = chaseTarget.transform.position;
        //  Vector3 targetDirection = (chaseTarget.transform.parent.position - spotLight.transform.position).normalized;

        // 가짜 후레쉬의 위치는 실제 후레쉬 위치와 동일하게 위치/.
        //  spotLight.transform.position = flashy.transform.Find("LightPosition").gameObject.transform.position;

        // 후레쉬 앞 방향을 AimTarget 오브젝트를 향하게 한다. 
        //   spotLight.transform.forward = targetDirection;
        //   flashy.transform.forward = targetDirection;

        //   if (spotLight.activeSelf == true)
        //   {
        // 두손으로 잡는 Rigging 실행. 
        //       rigBuilder.layers[0].active = true;
        //  }

        //타겟이 있는 경우, 타겟의 위치로 부드럽게 이동하도록 보간합니다.
        //  Target.position = Vector3.Lerp(Target.position, chaseTargetPos, Time.deltaTime * RetargetSpeed);

        //HeadRig의 weight 값을 이용하여 Rigging을 적용하여 헤드의 회전을 타겟 방향으로 매끄럽게 이동시킵니다.
        // HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);
        // }


        //자동에임 타겟팅한 적을 찾지 못한 상태에서 후레쉬만 켜져있는 경우.
        //chaseTarget == null && spotLight.activeSelf == true
        if ( spotLight.activeSelf == true)
        {
            rigWeight = 1;

            // 두손으로 잡는 Riggint 종료.
            //rigBuilder.layers[0].active = false;

            // +new Vector3(0.2f,0f,0f);
            // 가짜 후레쉬의 위치는 실제 후레쉬 위치와 동일하게 위치/.
            spotLight.transform.position = flashy.transform.Find("LightPosition").gameObject.transform.position;

            // 후레쉬의 앞 방향을 AimTarget의 앞방향과 일치시킨다. 
            spotLight.transform.forward = Target.transform.forward;
            flashy.transform.forward = Target.transform.forward;

            // 스포트라이트의 위치와 방향을 얻습니다.
            Vector3 position = spotLight.transform.position;
            Vector3 direction = spotLight.transform.forward;

            // 스포트라이트의 범위 절반을 계산합니다.
            float halfRange = spotLight.GetComponent<Light>().range / 2f;

            // 스포트라이트의 중심 좌표를 계산합니다.
            Vector3 centerCoordinates = position + direction * halfRange;


            Target.position = centerCoordinates;
            chaseTargetPos = Target.position;

            //타겟이 있는 경우, 타겟의 위치로 부드럽게 이동하도록 보간합니다.
            Target.position = Vector3.Lerp(Target.position, chaseTargetPos, Time.deltaTime * RetargetSpeed);

            //HeadRig의 weight 값을 이용하여 Rigging을 적용하여 헤드의 회전을 타겟 방향으로 매끄럽게 이동시킵니다.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);
        }


        //자동에임 타겟팅한 적을 찾지 못한 상태에서 후레쉬가 꺼져있는 경우. 
        if (spotLight.activeSelf == false)
        {
            // 아무것도 입력이 없는 경우

            // 두손으로 잡는 Riggint 종료.
            rigBuilder.layers[0].active = false;

            rigWeight = 0;
            //HeadRig의 weight 값을 이용하여 Rigging을 적용하여 헤드의 회전을 타겟 방향으로 매끄럽게 이동시킵니다.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);
        }









    }

    private float GetToLightDeg(GameObject spotLight, GameObject chaseTarget)
    {
        // 조명 - 적 방향 
        Vector3 toLightDir = chaseTarget.transform.parent.position - spotLight.transform.position;

        // 조명 - 적 각도 
        float toLightDeg = Vector3.Angle(spotLight.transform.position, toLightDir);

        return toLightDeg;
    }

    bool CheckExposed(GameObject chaseTarget)
    {
        float dist = Vector3.Distance(spotLight.transform.position, chaseTarget.transform.parent.position);

        // 후레쉬 Range 사거리 내에 있는 지 확인.
        if (dist < spotLight.GetComponent<Light>().range)
        {
            float deg = GetToLightDeg(spotLight, chaseTarget);

            //-spotangle/2 <= deg <= spotangle/2  인지 확인
            if (deg <= spotLight.GetComponent<Light>().spotAngle / 2)
            {
                return true;
            }
        }

        return false;
    }


}
