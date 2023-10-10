using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySound : MonoBehaviour
{
    //배터리 오디오 클립 배열 | 0 : fall, 1 : in
    public AudioClip[] AudioClips;
    //오디오 소스
    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        //배경과 부딪히면 떨어지는 소리 재생
        if(other.gameObject.CompareTag("TempBackground"))
        {
            if(!audioSource.isPlaying)
            {
                audioSource.clip = AudioClips[0];
                audioSource.Play();

            }
        }
    }


    public void PlaySoundBatteryIn()
    {
        audioSource.clip = AudioClips[1];
        audioSource.Play();
    }
}
