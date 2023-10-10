using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbleObject : MonoBehaviour
{



    public float pushPower = 0.1f;
    public GameObject player;
   

    private AudioSource audioSource;
    private Animator anim;
    private Rigidbody rb;
    private bool isPush = true;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        anim = player.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// �б� ���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        //print(PlayerMove.isClick);

        if (PlayerMove.isClick == false && isPush == true)
        {
            if (collision.gameObject.CompareTag("TempPlayer"))
            {

                if (rb != null)
                {

                    // �б� �ִϸ��̼� ���� ����.
                    anim.SetBool("Pushing", true);

                    // �б� ���� ����
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                    

                    Vector3 forceDirection = transform.position - collision.gameObject.transform.position;
                    forceDirection.y = 0;
                    forceDirection.Normalize();

                    rb.AddForceAtPosition(forceDirection * pushPower, transform.position, ForceMode.Impulse);
                }


            }
        }



        if (collision.gameObject.CompareTag("Walls"))
        {


            //print("�۵���");
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponentInChildren<BoxCollider>() != null)
                {
                    transform.GetChild(i).gameObject.GetComponent<BoxCollider>().enabled = true;
                    isPush = false;
                }
            }


        }

      
    }



    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("TempPlayer"))
        {
            // �б� �ִϸ��̼� ���� ����.
            anim.SetBool("Pushing", false);
        }
    }



}
