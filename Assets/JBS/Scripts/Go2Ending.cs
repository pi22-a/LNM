using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Go2Ending : MonoBehaviour
{
    //페이드아웃용 패널
    public Image padeOutPanelImg;

    //알파값
    float panelAlpha = 0;

    //프레임당 페이드 아웃 퍼센트
    public float padeOutPercent = 0.01f;

    private void Awake() {
        padeOutPanelImg.color = new Color(0,0,0,panelAlpha);
    }
    private void OnTriggerEnter(Collider other) {
        //print($"================={other.name}");
        //플레이어와 충돌시 엔딩씬 전환
        if(other.name.Contains("Player"))
        {
            //플레이어 이동 정지
            Transform player = other.transform;
            Animator playerAni = player.Find("LittleNightMares").GetComponent<Animator>();
            //플레이어 이동,충돌,리지드바디 비활성화
            player.GetComponent<PlayerMove>().enabled = false;
            player.GetComponent<BoxCollider>().enabled = false;
            player.GetComponent<Rigidbody>().useGravity = false;
            //플레이어 이동,오르기,앉기 상태 해제 = 대기 애니메이션 실행
            playerAni.SetBool("Move",false);
            playerAni.SetBool("ClimbingUpWall",false);
            playerAni.SetBool("CrouchWalk",false);
            //플레이어 위치 고정
            player.position = transform.position;
            //페이드 아웃후 엔딩 씬 전환 코루틴
            StartCoroutine(IEPadeOutEnding());
            
        }
    }

    //페이드 아웃후 엔딩 씬 전환
    IEnumerator IEPadeOutEnding()
    {
        //페이드 아웃
        while(panelAlpha < 1.0f)
        {
            panelAlpha += padeOutPercent;
            yield return null;
            padeOutPanelImg.color = new Color(0,0,0,panelAlpha);
        }
        //엔딩 씬 로드
        SceneManager.LoadScene(3);
        yield return null;
    }

}
