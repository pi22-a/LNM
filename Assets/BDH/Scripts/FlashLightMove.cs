using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class FlashLightMove : MonoBehaviour
{
    private Transform playerTargetTransform; 
    public GameObject aimTarget; // 후레쉬의 에임 오브젝트
    public GameObject spotLight; // 후레쉬 오브젝트의 하위 오브젝트 .
    public GameObject rigPlayer;// 리깅 플레이어 오브젝트.

   
    private float rotateSpeed = 0.5f; // 마우스 입력에 따른 후레쉬 회전 속도.
    private float focusSmoothSpeed = 0.01f; // 메인 카메라 회전 포커싱 속도.
    private Quaternion FlashRotation; // 후레쉬 회전 변수
    private bool isRotate; // 회전 여부 변수. 
    private RigBuilder rigBuilder;

    // 후레쉬 회전 좌표 변수. 
    private float flashMouseX = 0;
    private float flashMouseY = 0;

    private void Awake()
    {
        // 마우스 커서 숨기기.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerTargetTransform = this.gameObject.transform;
        rigBuilder = rigPlayer.GetComponent<RigBuilder>();
       
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 좌표를 받는다.
        float getAxisMouseX = Input.GetAxis("Mouse X");
        float getAxisMouseY = Input.GetAxis("Mouse Y");

        //마우스 오른쪽 클릭 시 (마우스 왼쪽 : 0, 마우스 오른쪽 : 1, 마우스 가운데 : 2) 회전시킴
        // 조건 : rigBuilder != null 이면서 후레쉬가 켜져있을 때 사용가능.! 
        if (Input.GetMouseButton(1) && rigBuilder != null && spotLight.activeSelf == true)
        {
            print(rigBuilder.layers[0].active);
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
            print("작동");
            rigBuilder.layers[0].active = false;
            isRotate = false;
            FlashFocus();

            flashMouseX = 0;
            flashMouseY = 0;

        }
    }


    private void Rotation(float getAxisMouseX, float getAxisMouseY)
    {
        // 후레쉬 각도 제한 설정. 

        // 마우스 입력으로부터 Y축 회전 값 계산

        flashMouseX -= getAxisMouseY * rotateSpeed;

        // 회전 값을 -160도에서 160도 사이로 제한
        flashMouseX = Mathf.Clamp(flashMouseX, -150f, 150f);

        // 마우스 입력으로부터 X축 회전 값 계산
        flashMouseY += getAxisMouseX * rotateSpeed;

        // 회전 값을 -160도에서 160도 사이로 제한
        flashMouseY = Mathf.Clamp(flashMouseY, -150f, 150f);

        aimTarget.transform.localEulerAngles = new Vector3(flashMouseX, flashMouseY, 0f);
    }


    private void FlashFocus()
    {
       // StartCoroutine(calFlashFocus());
    }

    IEnumerator calFlashFocus()
    {
        FlashRotation = aimTarget.transform.rotation;

        Vector3 targetDirection = (aimTarget.transform.position - playerTargetTransform.position).normalized;

        // 타겟 향하는 회전을 계산.
        Quaternion targetRotaion = Quaternion.LookRotation(targetDirection);

        while (FlashRotation != targetRotaion)
        {
            // 보간된 회전을 계산
            // FlashRotation = Quaternion.Slerp(FlashRotation, targetRotaion, focusSmoothSpeed * Time.deltaTime);
            FlashRotation = Quaternion.Slerp(FlashRotation, targetRotaion, focusSmoothSpeed * Time.deltaTime);
            // 부드러운 회전을 적용
            //transform.rotation = CameraRotation;

            // 플래쉬 회전도 포커싱 기능 적용.
            aimTarget.transform.rotation = FlashRotation;

            // 포커싱 기능 적용 시 플레이어의 앞 방향으로 설정.
            aimTarget.transform.transform.forward = playerTargetTransform.forward;
            


            yield return null; // 다음 프레임에 실행됨. 
        }

    }




}
