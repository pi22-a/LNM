using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairObjectSound : MonoBehaviour
{

    [SerializeField]
    private AudioClip chairClips;
    private AudioSource audioSource;

    private void Awake()
    {
        //오디오 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("발판에 충돌 했니,,?");
        if (collision.gameObject.CompareTag("TempPlayer"))
        {
            // 플레이어 up_to_bed 사운드가 실행된다.
            audioSource.PlayOneShot(chairClips);
           
        }
    }

   
}
