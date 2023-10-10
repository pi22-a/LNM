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
        //����� ��������
        audioSource = GetComponent<AudioSource>();
    }



    private void OnCollisionEnter(Collision collision)
    {
    
        if (collision.gameObject.CompareTag("TempPlayer") && isCollision == false && PlayerMove.hanging)
        {
            // �÷��̾� Hanging ���尡 ����ȴ�.
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
