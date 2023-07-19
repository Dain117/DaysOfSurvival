using Oculus.Interaction.PoseDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    CharacterController cc; //ĳ���� ��Ʈ�ѷ� ������Ʈ
    public float gravity = -20; //�߷� ���ӵ��� ũ��
    float yVelocity = 0; //���� �ӵ�
    public float jumpPower = 5f; //���� ũ��
    bool isJumping = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //����� �Է¿� ���� �յ��¿� �̵�����
        //1. ������� �Է��� �޴´�.
        float h = ARAVRInput.GetAxis("Horizontal"); //�¿�
        float v = ARAVRInput.GetAxis("Vertical"); //���Ʒ�(�յ�)
        Vector3 dir = new Vector3(h, 0, v); //2. ������ �����.
        dir = Camera.main.transform.TransformDirection(dir);
        //�߷��� ������ �������� �߰� 
        yVelocity += gravity * Time.deltaTime; 
        //�ٴڿ� ���� ���, ���� �׷��� ó���ϱ� ���� �ӵ��� 0���� �Ѵ�.
        if(cc.isGrounded)
        {
            yVelocity = 0;
            if(isJumping) isJumping = false;
        }
        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
        {
            if (!isJumping)
            {
                yVelocity = jumpPower;
                isJumping = true;
            }
        }
        dir.y = yVelocity;

        cc.Move(dir * speed * Time.deltaTime); //3. �̵��Ѵ�.
    }
}
