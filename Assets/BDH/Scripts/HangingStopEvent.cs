using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingStopEvent : MonoBehaviour
{
    //public float playTime = 0.25f;
    public bool stop = false;
    public bool start = false;
    public void HangingStartAnimation()
    {
        start = true;
        
    }

    public void HangingStopAnimation()
    {
        // �ö󰡴� �ִϸ��̼��� ����� ����
      
        stop = true;

    }
    

}
