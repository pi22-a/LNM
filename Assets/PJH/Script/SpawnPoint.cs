using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ������ ��ġ ������ ����. ��.
// ���ӸŴ����� ��ġ ������ �ҷ��ð�.
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
        // �÷��̾ ������ 
        if (other.gameObject.name.Contains("Player"))
        {
            //���� spawnpoint �� ��������.
            PlayerPrefs.SetInt("Respawn", spawnpoint);
        }

    }


}
