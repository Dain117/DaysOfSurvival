using Oculus.Interaction.PoseDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    CharacterController cc; //캐릭터 컨트롤러 컴포넌트
    public float gravity = -20; //중력 가속도의 크기
    float yVelocity = 0; //수직 속도
    public float jumpPower = 5f; //점프 크기
    bool isJumping = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //사용자 입력에 따른 앞뒤좌우 이동구현
        //1. 사용자의 입력을 받는다.
        float h = ARAVRInput.GetAxis("Horizontal"); //좌우
        float v = ARAVRInput.GetAxis("Vertical"); //위아래(앞뒤)
        Vector3 dir = new Vector3(h, 0, v); //2. 방향을 만든다.
        dir = Camera.main.transform.TransformDirection(dir);
        //중력을 적용한 수직방향 추가 
        yVelocity += gravity * Time.deltaTime; 
        //바닥에 있을 경우, 수직 항력을 처리하기 위해 속도를 0으로 한다.
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

        cc.Move(dir * speed * Time.deltaTime); //3. 이동한다.
    }
}
