using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    [Tooltip("잡기 기능인 대상 물건 오브젝트")]
    public GameObject pickUpObject;
    [Tooltip("잡기 기능을 적용할 플레이어의 팔")]
    public GameObject playerHand;
    [Tooltip("잡기 해제시 물건이 떨어지는 위치 ")]
    public GameObject dropOffPoint;
    [Tooltip("오브젝트를 잡을 수 있는 거리 ")]
    public float pickDistance = 1f;

    private Rigidbody rb;
    private int pickUpMask;
    private int defaultMask;
   

    private void Awake()
    {
        rb = pickUpObject.GetComponent<Rigidbody>();
        pickUpMask = LayerMask.NameToLayer("PickUpObject");
        defaultMask = LayerMask.NameToLayer("Default");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PickAllowed();

    }

    private void PickAllowed()
    {
        // 수정 플레이어와 오브젝트 객체와의 거리가 일정한 거리 이내일때 사용가능하도록.!
        float dist = Vector3.Distance(this.gameObject.transform.position, pickUpObject.transform.position);
       

        if(dist < pickDistance)
        {
            // 플레이어가 왼쪽 마우스 클릭을 누르는 동안 pickUpObject 오브젝트를 플레이어가 잡는다.
            // 레이어를 바꾸면서 오브젝트를 충돌, 및 해제를 설정한다. 
            if (Input.GetKey(KeyCode.Mouse0))
            {
                // 잡는 순간 레이어를 PickUpObject로 변경하여 pickUpObject 오브젝트와 플레이어의 layer를 조정하여 충돌에 의한 velocity 영향을 없앤다. 
                pickUpObject.gameObject.layer = pickUpMask;

                if (pickUpObject != null)
                {
                    // 해당 잡은 오브젝트의 Rigidbody의 gravity를 false 
                    rb.useGravity = false;
                    // 해당 잡은 오브젝트의 Rigidbody의 is Kinematic를 true
                    rb.isKinematic = true;

                    // 플레이어의 팔을 대상 오브젝트 물건을 하위로 설정한다.
                    pickUpObject.transform.SetParent(playerHand.transform);
                    // 대상 물건의 크기를 부모의 오브젝트에 맞게 localScale를 적용할 것인가 ,,,? 

                    // 물건을 잡은 물건의 위치를 플레이어의 팔의 위치로 설정한다.
                    pickUpObject.transform.position = playerHand.transform.position;
                }


            }

            // 플레이어가 왼쪽 마우스 클릭을 놓는 순간 pickUpObject 오브젝트를 플레이어가 떨어뜨린다.
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                // 부모인 플레이어의 팔을 떨어질 대상 오브젝트 null로 설정.
                pickUpObject.transform.parent = null;
                // 떨어질 위치 dropOffPoint에 떨어질 대상 오브젝트를 위치시킨다.
                dropOffPoint.transform.position = pickUpObject.transform.position;

                // RigidBody의 중력을 활성화
                rb.useGravity = true;
                // Rigidbody의 is Kinematic를 false 
                rb.isKinematic = false;

                pickUpObject.gameObject.layer = defaultMask;

            }
        }

        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pickableObject"))
        {
            pickUpObject = collision.gameObject.gameObject;

        }
        
    }





}
