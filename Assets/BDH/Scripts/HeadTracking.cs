using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadTracking : MonoBehaviour
{
    public Transform Target; // 타겟으로 지정할 Transform 객체를 설정.
    public GameObject spotLight; // 후레쉬 GameObject 객체 설정.
    public GameObject flashy;
    public Rig HeadRig; // Rigging을 적용할 Rig 객체 설정.
    public GameObject rigPlayer;// 리깅 플레이어 오브젝트.
    private RigBuilder rigBuilder;

    public float Radius = 10f; // 트래킹을 수행할 반경을 설정하는 변수.
    public float RetargetSpeed = 5f; // 타겟 위치를 추적하는 속도를 설정하는 변수.
    public float MaxAngle = 180f; // Rig 객체가 바라볼 수 있는 최대 각도 설정.
    List<LookAtEnemy> POIs; // LookAtEnemy 스크립트를 가지고 있는 오브젝트 리스트.
    float RadiusSqr; // 트래킹을 수행할 반경의 반지름만큼 구성하고 있는 구 형태의 반경.
    float rigWeight = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        // LookAtEnemy 스크립트를 가지고 있는 오브젝트 리스트를 찾아서 POIs 변수에 저장한다.
        POIs = FindObjectsOfType<LookAtEnemy>().ToList();

        rigBuilder = rigPlayer.GetComponent<RigBuilder>();

        // 트래킹을 수행할 반경을 계산한다. 
        RadiusSqr = Radius * Radius;
    }

    // 매 프레임마다 호출되며, POI들과 거리 및 각도를 비교하여 헤드 트래킹을 수행합니다.
    void Update()
    {
        Transform tracking = null;
        Vector3 targetPos;

        // foreach 루프를 통해 POIs들의 위치와 헤드의 위치 사이의 거리를 계산하고 , 반경 내에 있는 POIs를 찾는다. 
        foreach (LookAtEnemy poi in POIs)
        {
            Vector3 delta = poi.transform.position - transform.position;

            // delta의 sqrMagnitude는 두 점간의 거리를 구한다.
            // 트래킹을 수행할 반경 RadiusSqr 내에 있는 경우.
            if (delta.sqrMagnitude < RadiusSqr)
            {
                // 트래킹 반경에 있고, MaxAngle 내에 위치한 경우 .
                float angle = Vector3.Angle(transform.forward, delta);
                if (angle < MaxAngle)
                {
                    // 찾은 poi를 타겟으로 지정한다. 
                    tracking = poi.transform;

                    break;
                }


            }


        }


        if (tracking != null)
        {
            rigWeight = 1;
            targetPos = tracking.position;
            Vector3 dir = (tracking.position - spotLight.transform.position).normalized;

            // 가짜 후레쉬의 위치는 실제 후레쉬 위치와 동일하게 위치/.
            spotLight.transform.position = flashy.transform.Find("LightPosition").gameObject.transform.position;

            // 후레쉬 앞 방향을 AimTarget 오브젝트를 향하게 한다. 
            spotLight.transform.forward = dir;
            flashy.transform.forward = dir;

            if (spotLight.activeSelf == true)
            {
                // 두손으로 잡는 Rigging 실행. 
                rigBuilder.layers[0].active = true;
            }

            //타겟이 있는 경우, 타겟의 위치로 부드럽게 이동하도록 보간합니다.
            Target.position = Vector3.Lerp(Target.position, targetPos, Time.deltaTime * RetargetSpeed);

            //HeadRig의 weight 값을 이용하여 Rigging을 적용하여 헤드의 회전을 타겟 방향으로 매끄럽게 이동시킵니다.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);

        }

        if (tracking == null && spotLight.activeSelf == true)
        {
            rigWeight = 1;

            // 두손으로 잡는 Riggint 종료.
            rigBuilder.layers[0].active = false;

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
            targetPos = Target.position;

            //타겟이 있는 경우, 타겟의 위치로 부드럽게 이동하도록 보간합니다.
            Target.position = Vector3.Lerp(Target.position, targetPos, Time.deltaTime * RetargetSpeed);

            //HeadRig의 weight 값을 이용하여 Rigging을 적용하여 헤드의 회전을 타겟 방향으로 매끄럽게 이동시킵니다.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);


        }

        if (tracking == null || spotLight.activeSelf == false)
        {
            // 아무것도 입력이 없는 경우

            // 두손으로 잡는 Riggint 종료.
            rigBuilder.layers[0].active = false;

            rigWeight = 0;
            //HeadRig의 weight 값을 이용하여 Rigging을 적용하여 헤드의 회전을 타겟 방향으로 매끄럽게 이동시킵니다.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);
        }

      

    }

   /* private void OnDrawGizmos()
    {
        // 적 감지하는 범위 gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spotLight.GetComponent<Light>().range);
    }*/

}
