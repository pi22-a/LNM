using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 닿을시 위치 정보를 저장. 끝.
// 게임매니저가 위치 정보를 불러올것.
public class SpawnPoint : MonoBehaviour
{
    public int spawnpoint = 0;
  

    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 닿으면 
        if (other.gameObject.name.Contains("Player"))
        {
            //나의 spawnpoint 를 저장하자.
            PlayerPrefs.SetInt("Respawn", spawnpoint);
        }

    }


}
