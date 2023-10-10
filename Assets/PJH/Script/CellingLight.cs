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
    private bool hasTriggered = false; // OnTrigger가 실행되었는지 추척하는 상태 변수 
    public Transform hangParent; // 플레이어가 붙는 위치의 오브젝트.
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
      
            // 매달리는 애니메이션 실행. 
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
            //print("플레이어의 위치 고정.");
            //player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            player.transform.GetComponent<Rigidbody>().isKinematic = true;
        }



    }

    //@@JBS 수정 : 전등 사운드
    [SerializeField] AudioSource bulbSound;

    // 충돌시 작동, 나중에 레버 내려갔을때 전달하는 식으로 연결.
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
        //@@ JBS 수정 : 마지막으로 길게 유지하다 꺼지면서 꺼지는 사운드
        L_light.SetActive(true);
        yield return new WaitForSeconds(1f);
        //전등 깨지는 사운드 재생
        bulbSound.Play();
        L_light.SetActive(false);
        print("=============빛종료됨");
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
                // 플레이어가 레버를 잡는 사운드가 실행된다.
                //audioSource.PlayOneShot(soundEffect);
                isLeverEffect = true;
            }
        

            // 플레어어가 마우스 오른쪽 마우스를 클릭하는 동안 플레이어가 매달려 있는다. 
            hasTriggered = true;

            // 플레이어가 레버에 붙는 코드        
            other.transform.SetParent(hangParent);
            other.transform.position = hangParent.transform.position;

            // 레버 애니메이션을 작동시킨다.
            anim.SetTrigger("LeverStart");


        }

    }

    public void StopLeverAnimationEvent()
    {
        print("레버 이벤트가 종료됨.");
        animationExit = true;

        // 레버 애니메이션 작동이 끝나면
        player.transform.SetParent(hangParent, false);
        player.transform.SetParent(null);
        player.transform.GetComponent<Rigidbody>().isKinematic = false;

        // 조명이 켜진다.  
        L_light.SetActive(false);
        StartCoroutine(OnAndOff(waitTime));
    }

 
    private IEnumerator OnAndOff()
    {
        throw new NotImplementedException();
    }
}
