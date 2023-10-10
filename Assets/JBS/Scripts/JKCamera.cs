using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class JKCamera : MonoBehaviour
{
    //시네머신 메인
    CinemachineBrain cmBrain;
    //가상 카메라들
    [SerializeField] GameObject[] vCams;


    public static JKCamera instance;
    
    private void Awake() {
        instance = this;
        //시네머신 브레인 가져오기
        cmBrain = GetComponent<CinemachineBrain>();
        ////가상 카메라들 리스트 가져오기
        //vCams = GameObject.FindGameObjectsWithTag("VirtualCams");
        
    }

    private void Start() {
        //리스폰시 위치에 따른 카메라 설정
        //0번 스폰시 0번 캠
        switch(GameManager.instance.spawnIdx)
        {
            case 0:
                SetActiveCam(0);
                break;
            case 1:
                SetActiveCam(1);
                break;
        }
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.LeftBracket))
        {
            SetActiveCam(0);
        }
        else if(Input.GetKeyUp(KeyCode.RightBracket))
        {
            SetActiveCam(1);
        }

    }

    //현재 카메라 비활성화 후 요청받은 카메라 활성화
    public void SetActiveCam(int camIndex)
    {
        if(cmBrain.ActiveVirtualCamera != null)
        {
            GameObject curCam = cmBrain.ActiveVirtualCamera.VirtualCameraGameObject;
            print($"{curCam.name}에서 {vCams[camIndex].name}으로 전환");
        }
        //모든 카메라 비활성화
        foreach(GameObject vCam in vCams)
        {
            vCam.SetActive(false);
        }
        //요청 받은 카메라 활성화
        vCams[camIndex].SetActive(true);
    }


    //카메라 앞 방향 레이 그리기
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward*100);
    }
}
