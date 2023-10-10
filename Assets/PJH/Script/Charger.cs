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
    // 1. 배터리가 닿을시 마치 장착된것처럼 일한 위치로 이동시키고 싶음

    // 2. 배터리가 장착된 후 엘레베이터 문을 열고 싶음
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
        /* 강제로 가져오는 함수
        if(v.magnitude > 0)
        {

        }
        */
        if (open)
        {
            //Debug.Log("문이열립니다.");
            door.transform.Rotate(0, -10 * Time.deltaTime, 0);
            angle += -10 * Time.deltaTime;
            if (angle < -100)
            {
                open = false;
            }
            //Debug.Log("문이 열렸습니다.");

        }
    }

    // Vector3 v;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Battery")
        {
            // 배터리를 이동한다.
            other.transform.position = pos;
            
            // 그후 고정한다.
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            // other.transform.Rotate(90, 0, 0);
            //@@ JBS 수정 : 배터리 회전
            other.transform.rotation = Quaternion.Euler(90,0,0);
            //배터리 사운드 재생
            BatterySound bs = other.transform.GetComponent<BatterySound>();
            bs.PlaySoundBatteryIn();
            //
            Debug.Log("배터리 장착완료");
            open = true;

        }
        
    }
    
}
