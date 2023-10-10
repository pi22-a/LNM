using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 후레쉬 깜박거리는 효과는 기본적으로 30초 이상 ~ 1분이내의 랜덤 타이머 변수를 받는다.
// for문으로 깜박거리는 횟수는 랜덤값 변수로 실행한다. 

public class FlickeringFlashLight : MonoBehaviour
{
    public GameObject flash_light;
    private  Light flashlight;
    private float minTime = 30f; // 30
    private float maxTime = 60f; // 60
    private float timer;
    private float stopTimer; 
    

    public AudioSource AS;
    public AudioClip LightAudio;

    // Start is called before the first frame update
    void Start()
    {
        flashlight = flash_light.GetComponent<Light>();
        timer = Random.Range(minTime, maxTime);
    


    }

    // Update is called once per frame
    void Update()
    {
        if(flash_light.activeSelf == true)
        {
            FlickerLight();
        }
       
    }

    void FlickerLight()
    {
        if (timer > 0)
        {
           
            // 랜덤 타이머에서 정해진 시간동안 대기.
            timer -= Time.deltaTime;
           

        }

        if (timer <= 0)
        {
            // 여기는 한번만 실행됨. 

            AS.PlayOneShot(LightAudio);
            StartCoroutine(Flicker());

          
            timer = Random.Range(minTime, maxTime);
            
        }


    }

    private IEnumerator Flicker()
    {
        
        for (stopTimer = 0; stopTimer <= LightAudio.length; stopTimer += Time.deltaTime)
        {
            flashlight.enabled = !flashlight.enabled;
            yield return null;
        }
    }


}
