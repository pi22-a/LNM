using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField]
    private AudioClip[] walkClips;
    [SerializeField]
    private AudioClip[] crouchClips;
    [SerializeField]
    private AudioClip[] RunClips;
    [SerializeField]
    private AudioClip[] slideClips;
    [SerializeField]
    private AudioClip[] jumpLandClips;
    [SerializeField]
    private AudioClip[] jumpStartClips;
    [SerializeField]
    private AudioClip[] ladderStartClips;



    private AudioSource audioSource;

   
    private void Awake()
    {
        //오디오 가져오기
        audioSource = GetComponent<AudioSource>();
    }
  
   
    public void Ladder()
    {
        AudioClip ladderStartClips = GetRandomLadderClip();
        audioSource.PlayOneShot(ladderStartClips);
    }

    public void Step()
    {
        AudioClip walkClips = GetRandomWalkClip();
        audioSource.PlayOneShot(walkClips);
    }

    public void Crouch()
    {
        AudioClip crouchClips = GetRandomCrouchClip();
        audioSource.PlayOneShot(crouchClips);
    }



    public void Sprint()
    {
        AudioClip runClips = GetRandomRunClip();
        audioSource.PlayOneShot(runClips);
    }

    public void Slide()
    {
        AudioClip slideClips = GetRandomSlideClip();
        audioSource.PlayOneShot(slideClips);
    }

    public void jumpLand()
    {
        AudioClip jumpLandClips = GetRandomLandClip();
        audioSource.PlayOneShot(jumpLandClips);
    }
    public void jumpStart()
    {
        AudioClip jumpStartClips = GetRandomjumpStartClips();
        audioSource.PlayOneShot(jumpStartClips);
    }

   
   
    private AudioClip GetRandomLadderClip()
    {
        return ladderStartClips[UnityEngine.Random.Range(0, ladderStartClips.Length)];
    }

    private AudioClip GetRandomWalkClip()
    {
        return walkClips[UnityEngine.Random.Range(0, walkClips.Length)];
    }

    private AudioClip GetRandomRunClip()
    {
        return RunClips[UnityEngine.Random.Range(0, RunClips.Length)];
    }

    private AudioClip GetRandomSlideClip()
    {
        return slideClips[UnityEngine.Random.Range(0, slideClips.Length)];
    }

    private AudioClip GetRandomLandClip()
    {
        return jumpLandClips[UnityEngine.Random.Range(0, jumpLandClips.Length)];
    }
    private AudioClip GetRandomjumpStartClips()
    {
        return jumpStartClips[UnityEngine.Random.Range(0, jumpStartClips.Length)];
    }
    private AudioClip GetRandomCrouchClip()
    {
        return crouchClips[UnityEngine.Random.Range(0, crouchClips.Length)];
    }
  
}
