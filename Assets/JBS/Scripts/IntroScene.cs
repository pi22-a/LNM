using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroScene : MonoBehaviour
{
    //시작할때 영상이 시작되고 끝나면 다음 씬을 로드한다.
    VideoPlayer vid;

    private void Awake() {
        vid = GetComponent<VideoPlayer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        vid.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        print("Video is over");
        SceneManager.LoadScene(2);
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
