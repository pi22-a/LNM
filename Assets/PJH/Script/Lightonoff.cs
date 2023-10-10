using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Lightonoff : MonoBehaviour
{
    public GameObject flashy;
    GameObject flash_light;
    GameObject flash_light2;


    Transform tr;
    KeyCode[] KeyCode_List; //키코드값 케싱
    private  RigBuilder rigBuilder;
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] flashClips;

    void Awake()
    {

        //오디오 가져오기
        audioSource = GetComponent<AudioSource>();

        flash_light = gameObject.transform.Find("Spot Light").gameObject;
        flash_light2 = gameObject.transform.Find("Point Light").gameObject;
        tr = this.transform;

        rigBuilder = this.gameObject.GetComponentInParent<RigBuilder>();
        Key_Depoly();
    }

    private void Start()
    {
       
        
    }

    void Key_Depoly()
    {
        //키 배열
        KeyCode_List = new KeyCode[10];

        //코드값 케싱하기
        KeyCode_List[0] = KeyCode.E;
        KeyCode_List[1] = KeyCode.Escape;
    }

    // Update is called once per frame
    void Update()
    {
        KeyCode result = User_Input();

        if (result == KeyCode_List[0])
        {
            if (flash_light.activeSelf)
            {
                flash_light.SetActive(false);
                flashy.SetActive(false);

                if (rigBuilder != null)
                {
                    // 라이트 비활성화 효과음  
                    audioSource.PlayOneShot(flashClips[1]);
                    rigBuilder.layers[1].active = false;

                }


            }
            else
            {
                flash_light.SetActive(true);
                flashy.SetActive(true);
            
                if (rigBuilder != null)
                {
                    // 라이트 활성화 효과음. 
                    audioSource.PlayOneShot(flashClips[0]);
                    rigBuilder.layers[1].active = true;

                }
            }
            // point light추가
            if (flash_light2.activeSelf)
            {
                flash_light2.SetActive(false);
               
                if (rigBuilder != null)
                {
                    rigBuilder.layers[1].active = false;

                }


            }
            else
            {
                flash_light2.SetActive(true);
                if (rigBuilder != null)
                {
                    rigBuilder.layers[1].active = true;

                }
            }
        }
    }
    KeyCode User_Input()
    {
        KeyCode result = KeyCode_List[1];

        for (int i = 0; i < KeyCode_List.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode_List[i]))
            {
                result = KeyCode_List[i];
            }
        }

        return result;

    }

}
