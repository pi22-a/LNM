using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class EndingScene : MonoBehaviour
{
    //시작할때 영상이 시작되고 끝나면 다음 씬을 로드한다.
    VideoPlayer vid;

    //페이드 아웃 전 딜레이
    public float delayTime;

    public float fadeOutDuration;

    //페이드아웃용 패널
    public Image padeOutPanelImg;

    //알파값
    float panelAlpha = 0;

    ////프레임당 페이드 아웃 퍼센트
    //public float padeOutPercent = 0.01f;


    private void Awake() {
        vid = GetComponent<VideoPlayer>();
        padeOutPanelImg.color = new Color(0,0,0,panelAlpha);
    }
    // Start is called before the first frame update
    void Start()
    {
        vid.loopPointReached += CheckOver;
        StartCoroutine(IEDelayedPadeOut());
        
    }

    //delay 초 흐른 후 페이드 아웃
    IEnumerator IEDelayedPadeOut()
    {
        yield return new WaitForSeconds(delayTime);
        //페이드 아웃
        print("페이드 아웃 시작");
        for(float time = 0; time <= fadeOutDuration; time += Time.deltaTime)
        //while(panelAlpha < 1.0f)
        {
            panelAlpha = time/fadeOutDuration;
            yield return null;
            padeOutPanelImg.color = new Color(0,0,0,panelAlpha);
        }
        yield return null;
        panelAlpha = 1;
        padeOutPanelImg.color = new Color(0,0,0,panelAlpha);
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        print("Video is over");
        //SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CheckOver(vid);
        }
    }
}
