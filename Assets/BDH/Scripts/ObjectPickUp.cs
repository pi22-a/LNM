using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    [Tooltip("��� ����� ��� ���� ������Ʈ")]
    public GameObject pickUpObject;
    [Tooltip("��� ����� ������ �÷��̾��� ��")]
    public GameObject playerHand;
    [Tooltip("��� ������ ������ �������� ��ġ ")]
    public GameObject dropOffPoint;
    [Tooltip("������Ʈ�� ���� �� �ִ� �Ÿ� ")]
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
        // ���� �÷��̾�� ������Ʈ ��ü���� �Ÿ��� ������ �Ÿ� �̳��϶� ��밡���ϵ���.!
        float dist = Vector3.Distance(this.gameObject.transform.position, pickUpObject.transform.position);
       

        if(dist < pickDistance)
        {
            // �÷��̾ ���� ���콺 Ŭ���� ������ ���� pickUpObject ������Ʈ�� �÷��̾ ��´�.
            // ���̾ �ٲٸ鼭 ������Ʈ�� �浹, �� ������ �����Ѵ�. 
            if (Input.GetKey(KeyCode.Mouse0))
            {
                // ��� ���� ���̾ PickUpObject�� �����Ͽ� pickUpObject ������Ʈ�� �÷��̾��� layer�� �����Ͽ� �浹�� ���� velocity ������ ���ش�. 
                pickUpObject.gameObject.layer = pickUpMask;

                if (pickUpObject != null)
                {
                    // �ش� ���� ������Ʈ�� Rigidbody�� gravity�� false 
                    rb.useGravity = false;
                    // �ش� ���� ������Ʈ�� Rigidbody�� is Kinematic�� true
                    rb.isKinematic = true;

                    // �÷��̾��� ���� ��� ������Ʈ ������ ������ �����Ѵ�.
                    pickUpObject.transform.SetParent(playerHand.transform);
                    // ��� ������ ũ�⸦ �θ��� ������Ʈ�� �°� localScale�� ������ ���ΰ� ,,,? 

                    // ������ ���� ������ ��ġ�� �÷��̾��� ���� ��ġ�� �����Ѵ�.
                    pickUpObject.transform.position = playerHand.transform.position;
                }


            }

            // �÷��̾ ���� ���콺 Ŭ���� ���� ���� pickUpObject ������Ʈ�� �÷��̾ ����߸���.
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                // �θ��� �÷��̾��� ���� ������ ��� ������Ʈ null�� ����.
                pickUpObject.transform.parent = null;
                // ������ ��ġ dropOffPoint�� ������ ��� ������Ʈ�� ��ġ��Ų��.
                dropOffPoint.transform.position = pickUpObject.transform.position;

                // RigidBody�� �߷��� Ȱ��ȭ
                rb.useGravity = true;
                // Rigidbody�� is Kinematic�� false 
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
