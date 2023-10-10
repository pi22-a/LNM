using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellingLight : MonoBehaviour
{
    public GameObject player;
    public GameObject L_light;
    public GameObject L_light2;
    public float waitTime;
    public float animSpeed = 0.5f;
    private Animator anim;
    public AudioClip soundEffect;
    private Animator playerAnim;
    private bool animationExit;
    private bool hasTriggered = false; // OnTrigger�� ����Ǿ����� ��ô�ϴ� ���� ���� 
    public Transform hangParent; // �÷��̾ �ٴ� ��ġ�� ������Ʈ.
    private AudioSource audioSource; 
    private bool isLeverEffect = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        playerAnim = player.GetComponentInChildren<Animator>();
    }

   
    // Update is called once per frame
    void Update()
    {
        anim.speed = animSpeed;

        float dist = Vector3.Distance(player.gameObject.transform.position, this.transform.position);
        

        if (dist < 3f)
        {
      
            // �Ŵ޸��� �ִϸ��̼� ����. 
            playerAnim.SetTrigger("StandToFreehang");

            if (Input.GetKey(KeyCode.Mouse0))
            {
                playerAnim.SetBool("HangingIdle", true);
            }

            if (PlayerMove.ground)
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

    //@@JBS ���� : ���� ����
    [SerializeField] AudioSource bulbSound;

    // �浹�� �۵�, ���߿� ���� ���������� �����ϴ� ������ ����.
    public IEnumerator OnAndOff(float waitTime)
    {
        int count = 0;
        while (count <= 6)
        {
            yield return new WaitForSeconds(0.1f);
            L_light.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            L_light.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            L_light.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            L_light.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
        //@@ JBS ���� : ���������� ��� �����ϴ� �����鼭 ������ ����
        L_light.SetActive(true);
        yield return new WaitForSeconds(1f);
        //���� ������ ���� ���
        bulbSound.Play();
        L_light.SetActive(false);
        print("=============�������");
        //
        yield return new WaitForSeconds(0.5f);
        L_light2.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {

       
        if (other.gameObject.name.Contains("Player"))
        {
            if (isLeverEffect == false)
            {
                // �÷��̾ ������ ��� ���尡 ����ȴ�.
                //audioSource.PlayOneShot(soundEffect);
                isLeverEffect = true;
            }
        

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
        animationExit = true;

        // ���� �ִϸ��̼� �۵��� ������
        player.transform.SetParent(hangParent, false);
        player.transform.SetParent(null);
        player.transform.GetComponent<Rigidbody>().isKinematic = false;

        // ������ ������.  
        L_light.SetActive(false);
        StartCoroutine(OnAndOff(waitTime));
    }

 
    private IEnumerator OnAndOff()
    {
        throw new NotImplementedException();
    }
}
