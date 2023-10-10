using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryLever : MonoBehaviour
{
    // �ӽ� ���͸� �����ڵ�
    public GameObject player;
    public GameObject Battery;
    private Animator anim;
    private Animator playerAnim;
    private bool animationExit;
    private bool hasTriggered = false; // OnTrigger�� ����Ǿ����� ��ô�ϴ� ���� ���� 
    public Transform hangParent; // �÷��̾ �ٴ� ��ġ�� ������Ʈ.

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAnim = player.GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.gameObject.transform.position, this.transform.position);

        if (dist < 3f)
        {

            // �Ŵ޸��� �ִϸ��̼� ����. 
            playerAnim.SetTrigger("StandToFreehang");

            if (Input.GetKey(KeyCode.Mouse0))
            {
                playerAnim.SetBool("HangingIdle", true);
            }
            else
            {
                playerAnim.SetBool("HangingIdle", false);
            }
        }


        if (hasTriggered == true && animationExit == false)
        {
            //print("�÷��̾��� ��ġ ����.");
            //player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            player.transform.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Button")
        {
            // ��ư�� ������ �ϴÿ��� �ڽ��� ������ ���͸� ����.
            // �ӽ� ���͸� �ڵ�
            Rigidbody rb = Battery.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // �÷��̾ ������ 
        if (other.gameObject.name.Contains("Player"))
        {
            // �÷��� ���콺 ������ ���콺�� Ŭ���ϴ� ���� �÷��̾ �Ŵ޷� �ִ´�. 
            hasTriggered = true;

            // �÷��̾ ������ �ٴ� �ڵ�        
            other.transform.SetParent(hangParent);
            other.transform.position = hangParent.transform.position;

            // ���� �ִϸ��̼��� �۵���Ų��.
            anim.SetTrigger("LeverStart");
        }
    }
    public void StopLeverAnimationEvent()
    {
        print("���� �̺�Ʈ�� �����.");
        // ��ư�� ������ �ϴÿ��� �ڽ��� ������ ���͸� ����.
        //@@ JBS ����
        Rigidbody rb = Battery.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        //
        animationExit = true;

        // ���� �ִϸ��̼� �۵��� ������
        player.transform.SetParent(hangParent, false);
        player.transform.SetParent(null);
        player.transform.GetComponent<Rigidbody>().isKinematic = false;
    }

    public bool IsCurrentAnimationOver(Animator ani)
    {
        //  �ִϸ��̼� ���� : 0 , �ִϸ��̼� ���� : 1 
        // �ִϸ��̼� ���� ���½� : true ��ȯ, 
        return ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;
    }
    
}
