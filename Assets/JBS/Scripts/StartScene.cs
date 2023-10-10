using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartScene : MonoBehaviour
{
    //시작할 때 영상이 시작된다.
    //스페이스를 한 번누르면 1 영상을 스킵한다.
    //버튼 클릭이나 스페이스를 누르면 다음 씬을 로드한다.
    VideoPlayer vid;
    //게임재개, 새로하기 버튼
    public GameObject restartUnHover,restartHover,newStart;

    //클릭, 호버 오디오 소스
    public AudioSource clickSound,hoverSound;
    
    //영상 리스트
    public List<VideoClip> videoList;
    
    //영상 ui
    public RectTransform startPlayer;

    private void Awake() {
        vid = GetComponent<VideoPlayer>();
        vid.clip = videoList[0];
        startPlayer.SetAsFirstSibling();
    }

    void Start()
    {
        restartUnHover.SetActive(false);
        restartHover.SetActive(true);
        newStart.SetActive(false);
        vid.loopPointReached += CheckOver;
    }

    //다음 씬 로드
    public void OnStartGame()
    {
        StartCoroutine(IEStartGame());
    }
    //클릭 소리 끝날때 까지 재생 후 게임 시작
    IEnumerator IEStartGame()
    {
        clickSound.Play();
        while(clickSound.isPlaying)
        {
            yield return null;
        }
        print("다음 씬 시작");
        //버튼 비활성화에 따라 스폰 포인트 변경
        //게임 재개 시
        if(restartHover.activeSelf)
        {
        }
        //새로 하기 시
        else if(newStart.activeSelf)
        {
            PlayerPrefs.SetInt("Respawn", 0);
        }
        SceneManager.LoadScene(1);
    }
    //1 영상 끝나면 2영상 실행
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        vid.clip = videoList[1];
        vid.isLooping = true;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //0번 영상 중에는 스페이스로 스킵
        if(vid.clip == videoList[0])
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                hoverSound.Play();
                CheckOver(vid);
            }
        }
        //1번 영상 중
        else if(vid.clip == videoList[1])
        {
            //스페이스시 게임 시작
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OnStartGame();
            }
            //위 화살표로 게임재개 활성화
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                restartHover.SetActive(true);
                restartUnHover.SetActive(false);
                newStart.SetActive(false);
                hoverSound.Play();
            }
            //아래 화살표로 새로하기 활성화
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                restartUnHover.SetActive(true);
                restartHover.SetActive(false);
                newStart.SetActive(true);
                hoverSound.Play();
            }
        }
    }
}
