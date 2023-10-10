using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ķ��� ���ڰŸ��� ȿ���� �⺻������ 30�� �̻� ~ 1���̳��� ���� Ÿ�̸� ������ �޴´�.
// for������ ���ڰŸ��� Ƚ���� ������ ������ �����Ѵ�. 

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
           
            // ���� Ÿ�̸ӿ��� ������ �ð����� ���.
            timer -= Time.deltaTime;
           

        }

        if (timer <= 0)
        {
            // ����� �ѹ��� �����. 

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
