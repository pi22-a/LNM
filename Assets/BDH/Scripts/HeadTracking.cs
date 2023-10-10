using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadTracking : MonoBehaviour
{
    public Transform Target; // Ÿ������ ������ Transform ��ü�� ����.
    public GameObject spotLight; // �ķ��� GameObject ��ü ����.
    public GameObject flashy;
    public Rig HeadRig; // Rigging�� ������ Rig ��ü ����.
    public GameObject rigPlayer;// ���� �÷��̾� ������Ʈ.
    private RigBuilder rigBuilder;

    public float Radius = 10f; // Ʈ��ŷ�� ������ �ݰ��� �����ϴ� ����.
    public float RetargetSpeed = 5f; // Ÿ�� ��ġ�� �����ϴ� �ӵ��� �����ϴ� ����.
    public float MaxAngle = 180f; // Rig ��ü�� �ٶ� �� �ִ� �ִ� ���� ����.
    List<LookAtEnemy> POIs; // LookAtEnemy ��ũ��Ʈ�� ������ �ִ� ������Ʈ ����Ʈ.
    float RadiusSqr; // Ʈ��ŷ�� ������ �ݰ��� ��������ŭ �����ϰ� �ִ� �� ������ �ݰ�.
    float rigWeight = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        // LookAtEnemy ��ũ��Ʈ�� ������ �ִ� ������Ʈ ����Ʈ�� ã�Ƽ� POIs ������ �����Ѵ�.
        POIs = FindObjectsOfType<LookAtEnemy>().ToList();

        rigBuilder = rigPlayer.GetComponent<RigBuilder>();

        // Ʈ��ŷ�� ������ �ݰ��� ����Ѵ�. 
        RadiusSqr = Radius * Radius;
    }

    // �� �����Ӹ��� ȣ��Ǹ�, POI��� �Ÿ� �� ������ ���Ͽ� ��� Ʈ��ŷ�� �����մϴ�.
    void Update()
    {
        Transform tracking = null;
        Vector3 targetPos;

        // foreach ������ ���� POIs���� ��ġ�� ����� ��ġ ������ �Ÿ��� ����ϰ� , �ݰ� ���� �ִ� POIs�� ã�´�. 
        foreach (LookAtEnemy poi in POIs)
        {
            Vector3 delta = poi.transform.position - transform.position;

            // delta�� sqrMagnitude�� �� ������ �Ÿ��� ���Ѵ�.
            // Ʈ��ŷ�� ������ �ݰ� RadiusSqr ���� �ִ� ���.
            if (delta.sqrMagnitude < RadiusSqr)
            {
                // Ʈ��ŷ �ݰ濡 �ְ�, MaxAngle ���� ��ġ�� ��� .
                float angle = Vector3.Angle(transform.forward, delta);
                if (angle < MaxAngle)
                {
                    // ã�� poi�� Ÿ������ �����Ѵ�. 
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

            // ��¥ �ķ����� ��ġ�� ���� �ķ��� ��ġ�� �����ϰ� ��ġ/.
            spotLight.transform.position = flashy.transform.Find("LightPosition").gameObject.transform.position;

            // �ķ��� �� ������ AimTarget ������Ʈ�� ���ϰ� �Ѵ�. 
            spotLight.transform.forward = dir;
            flashy.transform.forward = dir;

            if (spotLight.activeSelf == true)
            {
                // �μ����� ��� Rigging ����. 
                rigBuilder.layers[0].active = true;
            }

            //Ÿ���� �ִ� ���, Ÿ���� ��ġ�� �ε巴�� �̵��ϵ��� �����մϴ�.
            Target.position = Vector3.Lerp(Target.position, targetPos, Time.deltaTime * RetargetSpeed);

            //HeadRig�� weight ���� �̿��Ͽ� Rigging�� �����Ͽ� ����� ȸ���� Ÿ�� �������� �Ų����� �̵���ŵ�ϴ�.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);

        }

        if (tracking == null && spotLight.activeSelf == true)
        {
            rigWeight = 1;

            // �μ����� ��� Riggint ����.
            rigBuilder.layers[0].active = false;

            // +new Vector3(0.2f,0f,0f);
            // ��¥ �ķ����� ��ġ�� ���� �ķ��� ��ġ�� �����ϰ� ��ġ/.
            spotLight.transform.position = flashy.transform.Find("LightPosition").gameObject.transform.position;

            // �ķ����� �� ������ AimTarget�� �չ���� ��ġ��Ų��. 
            spotLight.transform.forward = Target.transform.forward;
            flashy.transform.forward = Target.transform.forward;

            // ����Ʈ����Ʈ�� ��ġ�� ������ ����ϴ�.
            Vector3 position = spotLight.transform.position;
            Vector3 direction = spotLight.transform.forward;

            // ����Ʈ����Ʈ�� ���� ������ ����մϴ�.
            float halfRange = spotLight.GetComponent<Light>().range / 2f;

            // ����Ʈ����Ʈ�� �߽� ��ǥ�� ����մϴ�.
            Vector3 centerCoordinates = position + direction * halfRange;


            Target.position = centerCoordinates;
            targetPos = Target.position;

            //Ÿ���� �ִ� ���, Ÿ���� ��ġ�� �ε巴�� �̵��ϵ��� �����մϴ�.
            Target.position = Vector3.Lerp(Target.position, targetPos, Time.deltaTime * RetargetSpeed);

            //HeadRig�� weight ���� �̿��Ͽ� Rigging�� �����Ͽ� ����� ȸ���� Ÿ�� �������� �Ų����� �̵���ŵ�ϴ�.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);


        }

        if (tracking == null || spotLight.activeSelf == false)
        {
            // �ƹ��͵� �Է��� ���� ���

            // �μ����� ��� Riggint ����.
            rigBuilder.layers[0].active = false;

            rigWeight = 0;
            //HeadRig�� weight ���� �̿��Ͽ� Rigging�� �����Ͽ� ����� ȸ���� Ÿ�� �������� �Ų����� �̵���ŵ�ϴ�.
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, rigWeight, Time.deltaTime * 2);
        }

      

    }

   /* private void OnDrawGizmos()
    {
        // �� �����ϴ� ���� gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spotLight.GetComponent<Light>().range);
    }*/

}
