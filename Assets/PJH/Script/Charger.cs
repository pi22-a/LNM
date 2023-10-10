using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public GameObject battery;
    public GameObject chargeIn;
    public static Charger instance;
    public GameObject door;
    private Vector3 pos;
    float angle;
    bool open = false;
    

    //public Rigidbody rb;
    // 1. ���͸��� ������ ��ġ �����Ȱ�ó�� ���� ��ġ�� �̵���Ű�� ����

    // 2. ���͸��� ������ �� ���������� ���� ���� ����
    private void Awake()
    {
        instance = this;
        pos = chargeIn.transform.position;
    }
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* ������ �������� �Լ�
        if(v.magnitude > 0)
        {

        }
        */
        if (open)
        {
            //Debug.Log("���̿����ϴ�.");
            door.transform.Rotate(0, -10 * Time.deltaTime, 0);
            angle += -10 * Time.deltaTime;
            if (angle < -100)
            {
                open = false;
            }
            //Debug.Log("���� ���Ƚ��ϴ�.");

        }
    }

    // Vector3 v;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Battery")
        {
            // ���͸��� �̵��Ѵ�.
            other.transform.position = pos;
            
            // ���� �����Ѵ�.
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            // other.transform.Rotate(90, 0, 0);
            //@@ JBS ���� : ���͸� ȸ��
            other.transform.rotation = Quaternion.Euler(90,0,0);
            //���͸� ���� ���
            BatterySound bs = other.transform.GetComponent<BatterySound>();
            bs.PlaySoundBatteryIn();
            //
            Debug.Log("���͸� �����Ϸ�");
            open = true;

        }
        
    }
    
}
