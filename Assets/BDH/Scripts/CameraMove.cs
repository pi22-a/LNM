using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/*.플레이어의 고정으로 메인 카메라가 비춘다.*/
/* 카메라는 Y축으로 고정되어 있고 X,Z 축은 플레이어의 이동에 따라 카메라가 따라간다. 
/*. 사용자가 마우스 오른쪽 버튼을 클릭한 상태의 시점으로 카메라가 회전(rotation)을 조정한다.-> 오일러 앵글을 사용.! */
/*. 사용자가 마우스 오른쪽 버튼을 클릭을 놓으면 다시 플레이어의 고정으로 메인 카메라가 바뀐다.  Nerp()함수를 사용.!  */

public class CameraMove : MonoBehaviour
{
    public GameObject target; // 카메라가 따라다닐 오브젝트 타겟. 
    public GameObject aimTarget;
    public GameObject spotLight; // 후레쉬 오브젝트의 하위 오브젝트 .

    public GameObject rigPlayer;// 리깅 플레이어 오브젝트.
    public float rotateSpeed = 0.5f; // 마우스 입력에 따른 카메라 회전 속도.

    // 카메라 좌표 (1, 1, -5)
   /* public float offsetX = 1f;
    public float offsetY = 1f;
    public float offsetZ = -5f;
    public float cameraSpeed = 10.0f; // 카메라 속도.*/
    public float focusSmoothSpeed = 5f; // 메인 카메라 회전 포커싱 속도. 

    //public float x_limitAngle = 10f; // 마우스 입력 제한 X축 
    //public float y_limitAngle = 10f; // 마우스 입력 제한 Y축 



   // private Vector3 targetPos; // 타겟의 위치. 
    private Quaternion CameraRotation; // 카메라 회전 변수
    //private new Transform transform; // 카메라 오브젝트의 Transform 컴포넌트. 
    private bool isRotate; // 회전 여부 변수. 
    private RigBuilder rigBuilder;


   // float mouseX;
    //float mouseY;

    float flashMouseX = 0;
    float flashMouseY = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 마우스 커서 숨기기.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

       // transform = GetComponent<Transform>(); //부모의 오브젝트(ObjectCamera)의 Transform 컴포넌트를 가져온다. 
        rigBuilder = rigPlayer.GetComponent<RigBuilder>();

        // 마우스의 오일러 앵글 설정.
       // mouseX = transform.rotation.eulerAngles.y; // 마우스 가로(x)는 세로축 (y)이 중심.
        //mouseY = -transform.rotation.eulerAngles.x; // 마우스 세로(y)는 가로축(-x)이 중심.
    }

    // 매 프레임마다 호출.( 물리 효과가 적용되지 않은 오브젝트의 움직임이나 단순한 타이머, 키 입력.
    void Update()
    {
        // 마우스 좌표를 받는다.
        float getAxisMouseX = Input.GetAxis("Mouse X");
        float getAxisMouseY = Input.GetAxis("Mouse Y");

        //마우스 오른쪽 클릭 시 (마우스 왼쪽 : 0, 마우스 오른쪽 : 1, 마우스 가운데 : 2) 회전시킴
        // 조건 : rigBuilder != null 이면서 후레쉬가 켜져있을 때 사용가능.! 
        if (Input.GetMouseButton(1) && rigBuilder != null && spotLight.activeSelf == true)
        {

            rigBuilder.layers[0].active = true;
            isRotate = true;

            if (isRotate)
            {
                Rotation(getAxisMouseX, getAxisMouseY);
            }

        }

    }





    // 모든 Update 함수가 호출된 후, 마지막으로 호출된다. 주로 오브젝트를 따라가게 설정한 카메라 사용.
    private void LateUpdate()
    {

        if (Input.GetMouseButtonUp(1) && rigBuilder != null)
        {

            rigBuilder.layers[0].active = false;
            isRotate = false;
            cameraFocus();

            flashMouseX = 0;
            flashMouseY = 0;

        }





    }




    /// <summary>
    /// 플레이어에 따른 카메라 이동 메소드.X,Z축만 이동함.
    /// </summary>
   /* private void FixedUpdate()
    {
        targetPos = new Vector3(target.transform.position.x + offsetX,
            offsetY,
             target.transform.position.z + offsetZ
            );

        // Lerp 함수를 이용하여 선형 보간 기법으로 카메라의 이동을 부드럽게 한다.
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
    }*/


    private void Rotation(float getAxisMouseX, float getAxisMouseY)
    {



        // Mathf.Clamp 함수 : 최소/ 최대값을 설정하여 float값이 범위 이외의 값을 갖지 못하게 함. 
        // -limitAngle ~ limitAngle 사이의 각도만 허용.! 

        // 카메라 각도 제한 설정.
       // mouseX = Mathf.Clamp(mouseX + getAxisMouseX * rotateSpeed, -x_limitAngle, x_limitAngle);
        //mouseY = Mathf.Clamp(mouseY + getAxisMouseY * rotateSpeed, -y_limitAngle, y_limitAngle);


        // 후레쉬 각도 제한 설정. 

        // 마우스 입력으로부터 Y축 회전 값 계산

        flashMouseX -= getAxisMouseY * rotateSpeed;

        // 회전 값을 -160도에서 160도 사이로 제한
        flashMouseX = Mathf.Clamp(flashMouseX, -60f, 60f);

        // 마우스 입력으로부터 X축 회전 값 계산
        flashMouseY += getAxisMouseX * rotateSpeed;

        // 회전 값을 -160도에서 160도 사이로 제한
        flashMouseY = Mathf.Clamp(flashMouseY, -60f, 60f);

        aimTarget.transform.localEulerAngles = new Vector3(flashMouseX, flashMouseY, 0f);


        // 새로운 회전 값을 적용하여 대상 카메라 회전
        //transform.rotation = Quaternion.Euler(transform.rotation.x - mouseY, transform.rotation.y + mouseX, 0.0f);


    }

    /// <summary>
    /// 메인 카메라 플레이어 회전 포커싱 기능 구현 메소드 
    /// </summary>
    private void cameraFocus()
    {
        if (target != null)
        {

            StartCoroutine(calCamera());

        }
    }

    IEnumerator calCamera()
    {

        //CameraRotation = this.transform.rotation;
        CameraRotation = aimTarget.transform.rotation;

        // 타겟 향하는 방향을 계산.
        //Vector3 targetDirection = target.transform.position - this.transform.position;
        Vector3 targetDirection = aimTarget.transform.position - target.transform.position;
        targetDirection.Normalize();

        // 타겟 향하는 회전을 계산.
        Quaternion targetRotaion = Quaternion.LookRotation(targetDirection);

        while (CameraRotation != targetRotaion)
        {
            // 보간된 회전을 계산
            CameraRotation = Quaternion.Slerp(CameraRotation, targetRotaion, focusSmoothSpeed * Time.deltaTime);

            // 부드러운 회전을 적용
            //transform.rotation = CameraRotation;
           
            // 플래쉬 회전도 포커싱 기능 적용.
            aimTarget.transform.rotation = CameraRotation;

            // 포커싱 기능 적용 시 플레이어의 앞 방향으로 설정.
            aimTarget.transform.transform.forward = target.transform.forward;


            yield return null; // 다음 프레임에 실행됨. 
        }
    }







}