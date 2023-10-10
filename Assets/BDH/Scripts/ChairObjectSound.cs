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
        //����� ��������
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("���ǿ� �浹 �ߴ�,,?");
        if (collision.gameObject.CompareTag("TempPlayer"))
        {
            // �÷��̾� up_to_bed ���尡 ����ȴ�.
            audioSource.PlayOneShot(chairClips);
           
        }
    }

   
}
