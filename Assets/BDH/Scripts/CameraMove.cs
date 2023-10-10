using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/*.�÷��̾��� �������� ���� ī�޶� �����.*/
/* ī�޶�� Y������ �����Ǿ� �ְ� X,Z ���� �÷��̾��� �̵��� ���� ī�޶� ���󰣴�. 
/*. ����ڰ� ���콺 ������ ��ư�� Ŭ���� ������ �������� ī�޶� ȸ��(rotation)�� �����Ѵ�.-> ���Ϸ� �ޱ��� ���.! */
/*. ����ڰ� ���콺 ������ ��ư�� Ŭ���� ������ �ٽ� �÷��̾��� �������� ���� ī�޶� �ٲ��.  Nerp()�Լ��� ���.!  */

public class CameraMove : MonoBehaviour
{
    public GameObject target; // ī�޶� ����ٴ� ������Ʈ Ÿ��. 
    public GameObject aimTarget;
    public GameObject spotLight; // �ķ��� ������Ʈ�� ���� ������Ʈ .

    public GameObject rigPlayer;// ���� �÷��̾� ������Ʈ.
    public float rotateSpeed = 0.5f; // ���콺 �Է¿� ���� ī�޶� ȸ�� �ӵ�.

    // ī�޶� ��ǥ (1, 1, -5)
   /* public float offsetX = 1f;
    public float offsetY = 1f;
    public float offsetZ = -5f;
    public float cameraSpeed = 10.0f; // ī�޶� �ӵ�.*/
    public float focusSmoothSpeed = 5f; // ���� ī�޶� ȸ�� ��Ŀ�� �ӵ�. 

    //public float x_limitAngle = 10f; // ���콺 �Է� ���� X�� 
    //public float y_limitAngle = 10f; // ���콺 �Է� ���� Y�� 



   // private Vector3 targetPos; // Ÿ���� ��ġ. 
    private Quaternion CameraRotation; // ī�޶� ȸ�� ����
    //private new Transform transform; // ī�޶� ������Ʈ�� Transform ������Ʈ. 
    private bool isRotate; // ȸ�� ���� ����. 
    private RigBuilder rigBuilder;


   // float mouseX;
    //float mouseY;

    float flashMouseX = 0;
    float flashMouseY = 0;

    // Start is called before the first frame update
    void Start()
    {
        // ���콺 Ŀ�� �����.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

       // transform = GetComponent<Transform>(); //�θ��� ������Ʈ(ObjectCamera)�� Transform ������Ʈ�� �����´�. 
        rigBuilder = rigPlayer.GetComponent<RigBuilder>();

        // ���콺�� ���Ϸ� �ޱ� ����.
       // mouseX = transform.rotation.eulerAngles.y; // ���콺 ����(x)�� ������ (y)�� �߽�.
        //mouseY = -transform.rotation.eulerAngles.x; // ���콺 ����(y)�� ������(-x)�� �߽�.
    }

    // �� �����Ӹ��� ȣ��.( ���� ȿ���� ������� ���� ������Ʈ�� �������̳� �ܼ��� Ÿ�̸�, Ű �Է�.
    void Update()
    {
        // ���콺 ��ǥ�� �޴´�.
        float getAxisMouseX = Input.GetAxis("Mouse X");
        float getAxisMouseY = Input.GetAxis("Mouse Y");

        //���콺 ������ Ŭ�� �� (���콺 ���� : 0, ���콺 ������ : 1, ���콺 ��� : 2) ȸ����Ŵ
        // ���� : rigBuilder != null �̸鼭 �ķ����� �������� �� ��밡��.! 
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





    // ��� Update �Լ��� ȣ��� ��, ���������� ȣ��ȴ�. �ַ� ������Ʈ�� ���󰡰� ������ ī�޶� ���.
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
    /// �÷��̾ ���� ī�޶� �̵� �޼ҵ�.X,Z�ุ �̵���.
    /// </summary>
   /* private void FixedUpdate()
    {
        targetPos = new Vector3(target.transform.position.x + offsetX,
            offsetY,
             target.transform.position.z + offsetZ
            );

        // Lerp �Լ��� �̿��Ͽ� ���� ���� ������� ī�޶��� �̵��� �ε巴�� �Ѵ�.
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
    }*/


    private void Rotation(float getAxisMouseX, float getAxisMouseY)
    {



        // Mathf.Clamp �Լ� : �ּ�/ �ִ밪�� �����Ͽ� float���� ���� �̿��� ���� ���� ���ϰ� ��. 
        // -limitAngle ~ limitAngle ������ ������ ���.! 

        // ī�޶� ���� ���� ����.
       // mouseX = Mathf.Clamp(mouseX + getAxisMouseX * rotateSpeed, -x_limitAngle, x_limitAngle);
        //mouseY = Mathf.Clamp(mouseY + getAxisMouseY * rotateSpeed, -y_limitAngle, y_limitAngle);


        // �ķ��� ���� ���� ����. 

        // ���콺 �Է����κ��� Y�� ȸ�� �� ���

        flashMouseX -= getAxisMouseY * rotateSpeed;

        // ȸ�� ���� -160������ 160�� ���̷� ����
        flashMouseX = Mathf.Clamp(flashMouseX, -60f, 60f);

        // ���콺 �Է����κ��� X�� ȸ�� �� ���
        flashMouseY += getAxisMouseX * rotateSpeed;

        // ȸ�� ���� -160������ 160�� ���̷� ����
        flashMouseY = Mathf.Clamp(flashMouseY, -60f, 60f);

        aimTarget.transform.localEulerAngles = new Vector3(flashMouseX, flashMouseY, 0f);


        // ���ο� ȸ�� ���� �����Ͽ� ��� ī�޶� ȸ��
        //transform.rotation = Quaternion.Euler(transform.rotation.x - mouseY, transform.rotation.y + mouseX, 0.0f);


    }

    /// <summary>
    /// ���� ī�޶� �÷��̾� ȸ�� ��Ŀ�� ��� ���� �޼ҵ� 
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

        // Ÿ�� ���ϴ� ������ ���.
        //Vector3 targetDirection = target.transform.position - this.transform.position;
        Vector3 targetDirection = aimTarget.transform.position - target.transform.position;
        targetDirection.Normalize();

        // Ÿ�� ���ϴ� ȸ���� ���.
        Quaternion targetRotaion = Quaternion.LookRotation(targetDirection);

        while (CameraRotation != targetRotaion)
        {
            // ������ ȸ���� ���
            CameraRotation = Quaternion.Slerp(CameraRotation, targetRotaion, focusSmoothSpeed * Time.deltaTime);

            // �ε巯�� ȸ���� ����
            //transform.rotation = CameraRotation;
           
            // �÷��� ȸ���� ��Ŀ�� ��� ����.
            aimTarget.transform.rotation = CameraRotation;

            // ��Ŀ�� ��� ���� �� �÷��̾��� �� �������� ����.
            aimTarget.transform.transform.forward = target.transform.forward;


            yield return null; // ���� �����ӿ� �����. 
        }
    }







}