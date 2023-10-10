using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorder : MonoBehaviour
{
    //영역 카메라 번호
    [SerializeField] int enterCamIndex;
    

    //플레이어 진입시 카메라 설정
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("TempPlayer"))
        {
            JKCamera.instance.SetActiveCam(enterCamIndex);
        }
    }
}
