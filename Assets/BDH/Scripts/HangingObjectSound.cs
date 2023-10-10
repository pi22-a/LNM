using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingObjectSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip hangingClips;
    private AudioSource audioSource;
    private bool isCollision = false;

    private void Awake()
    {
        //오디오 가져오기
        audioSource = GetComponent<AudioSource>();
    }



    private void OnCollisionEnter(Collision collision)
    {
    
        if (collision.gameObject.CompareTag("TempPlayer") && isCollision == false && PlayerMove.hanging)
        {
            // 플레이어 Hanging 사운드가 실행된다.
            audioSource.PlayOneShot(hangingClips);
            isCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("TempPlayer"))
        {
            isCollision = false;
        }
    }

}
