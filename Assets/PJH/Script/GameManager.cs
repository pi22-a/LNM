using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부, 게임 UI를 관리하는 게임 매니저
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤이 할당될 static 변수
    //public PrefabManager prefabManager;
    //public GameObject StartUI;

    public Transform[] spawnPoints;

    public int spawnIdx = 0; 



    private void Awake()
    {
        instance = this;
    }

    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");

        //캐릭터 위치 설정
        spawnIdx = PlayerPrefs.GetInt("Respawn", 0);

        Vector3 pos = spawnPoints[spawnIdx].position;
        player.transform.position = pos;
    }

    private void Update()
    {
        // 테스트용 코드
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerPrefs.SetInt("Respawn", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerPrefs.SetInt("Respawn", 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerPrefs.SetInt("Respawn", 2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerPrefs.SetInt("Respawn", 3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayerPrefs.SetInt("Respawn", 0);
        }
    }


    // 게임 오버 처리
    public void EndGame()
    {
        print(SceneManager.GetActiveScene().name);

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);




    }
}