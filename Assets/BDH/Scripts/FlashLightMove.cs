using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class FlashLightMove : MonoBehaviour
{
    private Transform playerTargetTransform; 
    public GameObject aimTarget; // �ķ����� ���� ������Ʈ
    public GameObject spotLight; // �ķ��� ������Ʈ�� ���� ������Ʈ .
    public GameObject rigPlayer;// ���� �÷��̾� ������Ʈ.

   
    private float rotateSpeed = 0.5f; // ���콺 �Է¿� ���� �ķ��� ȸ�� �ӵ�.
    private float focusSmoothSpeed = 0.01f; // ���� ī�޶� ȸ�� ��Ŀ�� �ӵ�.
    private Quaternion FlashRotation; // �ķ��� ȸ�� ����
    private bool isRotate; // ȸ�� ���� ����. 
    private RigBuilder rigBuilder;

    // �ķ��� ȸ�� ��ǥ ����. 
    private float flashMouseX = 0;
    private float flashMouseY = 0;

    private void Awake()
    {
        // ���콺 Ŀ�� �����.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerTargetTransform = this.gameObject.transform;
        rigBuilder = rigPlayer.GetComponent<RigBuilder>();
       
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ��ǥ�� �޴´�.
        float getAxisMouseX = Input.GetAxis("Mouse X");
        float getAxisMouseY = Input.GetAxis("Mouse Y");

        //���콺 ������ Ŭ�� �� (���콺 ���� : 0, ���콺 ������ : 1, ���콺 ��� : 2) ȸ����Ŵ
        // ���� : rigBuilder != null �̸鼭 �ķ����� �������� �� ��밡��.! 
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

    // ��� Update �Լ��� ȣ��� ��, ���������� ȣ��ȴ�. �ַ� ������Ʈ�� ���󰡰� ������ ī�޶� ���.
    private void LateUpdate()
    {

        if (Input.GetMouseButtonUp(1) && rigBuilder != null)
        {
            print("�۵�");
            rigBuilder.layers[0].active = false;
            isRotate = false;
            FlashFocus();

            flashMouseX = 0;
            flashMouseY = 0;

        }
    }


    private void Rotation(float getAxisMouseX, float getAxisMouseY)
    {
        // �ķ��� ���� ���� ����. 

        // ���콺 �Է����κ��� Y�� ȸ�� �� ���

        flashMouseX -= getAxisMouseY * rotateSpeed;

        // ȸ�� ���� -160������ 160�� ���̷� ����
        flashMouseX = Mathf.Clamp(flashMouseX, -150f, 150f);

        // ���콺 �Է����κ��� X�� ȸ�� �� ���
        flashMouseY += getAxisMouseX * rotateSpeed;

        // ȸ�� ���� -160������ 160�� ���̷� ����
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

        // Ÿ�� ���ϴ� ȸ���� ���.
        Quaternion targetRotaion = Quaternion.LookRotation(targetDirection);

        while (FlashRotation != targetRotaion)
        {
            // ������ ȸ���� ���
            // FlashRotation = Quaternion.Slerp(FlashRotation, targetRotaion, focusSmoothSpeed * Time.deltaTime);
            FlashRotation = Quaternion.Slerp(FlashRotation, targetRotaion, focusSmoothSpeed * Time.deltaTime);
            // �ε巯�� ȸ���� ����
            //transform.rotation = CameraRotation;

            // �÷��� ȸ���� ��Ŀ�� ��� ����.
            aimTarget.transform.rotation = FlashRotation;

            // ��Ŀ�� ��� ���� �� �÷��̾��� �� �������� ����.
            aimTarget.transform.transform.forward = playerTargetTransform.forward;
            


            yield return null; // ���� �����ӿ� �����. 
        }

    }




}
