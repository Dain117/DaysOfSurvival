using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDain : MonoBehaviour
{
    CharacterController characterController;
    GameObject RightGrabbedObject;          // ����ִ� ��ü
    GameObject LefftGrabbedObject;
    Vector3 prevPos;                        // ���� ��ġ
    Quaternion prevRot;                     // ���� ����

    public Slider HP;
    public Slider HG;

    public LayerMask RightgrabbedLayer;     // ����ִ� ��ü ����
    public LayerMask LeftgrabbedLayer;      // ����ִ� ��ü ����
    public GameObject attackPoint;

    public float speed = 5f;
    public float gravity = -20;
    public float jumpPower = 5;
    public float grabRange = 0.2f;          // ���� �� �ִ� �Ÿ�
    public float remoteGrabDistance = 20f;  // ���Ÿ� ��ü�� ���� �� �ִ� �Ÿ�
    public bool isRightRemoteGrab = true;   // ���Ÿ� ��ü�� ��� ��� Ȱ��ȭ ����
    public bool isLeftRemoteGrab = true;    // ���Ÿ� ��ü�� ��� ��� Ȱ��ȭ ����

    public float rotationSpeed = 100f;      // ȸ�� �ӵ�

    float throwPower = 5f;                  // ���� ��

    float time = 0;
    bool isJumping = false;
    float yVelocity = 0;
    bool isRightGrabbing = false;           // ������ ��ü�� ��� �ִ��� ����
    bool isLeftGrabbing = false;            // ������ ��ü�� ��� �ִ��� ����
    bool isAttack = false;
    bool ishunger = false;

    public Image itemImage;
    public Image usedImage;
    public Image HGImage;

    #region �÷��̾� ����(ü�� ��...)
    public int hp = 0;                      // ü��
    public int hunger = 0;                  // ���
    public int chill = 0;                   // �ѱ�
    public static int damage = 0;                  // ���ݷ�
    #endregion

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        hp = 100;
        hunger = 100;
        damage = 10;

        itemImage = GameObject.Find("ItemImg").GetComponent<Healing>().GetComponent<Image>();
        usedImage = GameObject.Find("UsedImg").GetComponent<Meat>().GetComponent<Image>();
        HGImage = GameObject.Find("HGImg").GetComponent<Meat>().GetComponent<Image>();

    }

    void Update()
    {
        time += Time.deltaTime;

        Move();
        Attack();
        Hungry();

        if (hp <= 0)
            Destroy(gameObject);

        #region �÷��̾� �޼� ������ Grab
        if (isRightGrabbing == false)
            TryRightGrab();
        else
            TryRightUngrab();

        if (isLeftGrabbing == false)
            TryLeftGrab();
        else
            TryLeftUngrab();
        #endregion

        HP.value = hp;
        HG.value = hunger;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (itemImage.sprite.name == "equip_icon_potion_red_2")
            {
                hp += 50;
                itemImage.sprite = usedImage.sprite;

            }
        }

        if (Input.GetKeyDown(KeyCode.W))
            
        {
            if (HGImage.sprite.name == "icon_food_meat")
            {
                hunger += 50;
                HGImage.sprite = usedImage.sprite;
            }
        }
        
    }

    #region �÷��̾� ������
    void Move()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        //dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

       // Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);            // Oculus ������ Thumbstick �Է� �ޱ�
      //  transform.Rotate(Vector3.up, thumbstickInput.x * rotationSpeed * Time.deltaTime);       // Thumbstick�� x ������ ĳ���͸� �¿�� ȸ��

        if (characterController.isGrounded)
        {
            yVelocity = 0;

            if (isJumping)
                isJumping = false;
        }

        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch))
        {
            if (!isJumping)
            {
                yVelocity = jumpPower;
                isJumping = true;
            }
        }

        dir.y = yVelocity;

        characterController.Move(dir * speed * Time.deltaTime);
    }
    #endregion

    #region �÷��̾� ������ Grab
    void TryRightGrab()
    {
        if (ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            if (isRightRemoteGrab)
            {
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitInfo;

                if (Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance, RightgrabbedLayer))
                {
                    isRightGrabbing = true;
                    RightGrabbedObject = hitInfo.transform.gameObject;
                    StartCoroutine(RightGrabbingAnimation());

                }

                return;
            }

            int closest = 0;
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, RightgrabbedLayer);

            for (int i = 1; i < hitObjects.Length; i++)
            {
                Vector3 colosetPos = hitObjects[closest].transform.position;
                Vector3 nextPos = hitObjects[i].transform.position;

                float colsestDistance = Vector3.Distance(colosetPos, ARAVRInput.RHandPosition);
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.RHandPosition);

                if (nextDistance < colsestDistance)
                {
                    closest = i;
                }
            }

            if (hitObjects.Length > 0)
            {
                isRightGrabbing = true;
                RightGrabbedObject = hitObjects[closest].gameObject;
                RightGrabbedObject.transform.parent = ARAVRInput.RHand;
                RightGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                prevPos = ARAVRInput.RHandPosition;
                prevRot = ARAVRInput.RHand.rotation;
            }
        }
    }
    void TryRightUngrab()
    {
        Vector3 throwDirection = ARAVRInput.RHandDirection;
        prevPos = ARAVRInput.RHandPosition;

        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);

        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            isRightGrabbing = false;                                             //���� ���� ���·� ��ȯ
            RightGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;    // ������� Ȱ��ȭ
            RightGrabbedObject.transform.parent = null;                          //�տ��� �����
            RightGrabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

            float angle;
            Vector3 axis;

            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;

            RightGrabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
            RightGrabbedObject = null;
        }
    }

    IEnumerator RightGrabbingAnimation()
    {
        RightGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        prevPos = ARAVRInput.RHandPosition;
        prevRot = ARAVRInput.RHand.rotation;
        Vector3 startLocation = RightGrabbedObject.transform.position;
        Vector3 targetLocation = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;
        float elapsedRate = currentTime / finishTime;

        while (elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            RightGrabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);
            yield return null;
        }

        RightGrabbedObject.transform.position = targetLocation;
        RightGrabbedObject.transform.parent = ARAVRInput.RHand;
    }
    #endregion

    #region �÷��̾� �޼� Grab
    void TryLeftGrab()
    {
        if (ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            if (isLeftRemoteGrab)
            {
                Ray _ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
                RaycastHit _hitInfo;

                if (Physics.SphereCast(_ray, 0.5f, out _hitInfo, remoteGrabDistance, LeftgrabbedLayer))
                {
                    isLeftGrabbing = true;
                    LefftGrabbedObject = _hitInfo.transform.gameObject;
                    StartCoroutine(LeftGrabbingAnimation());
                }

                return;
            }

            int closest = 0;
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.LHandPosition, grabRange, LeftgrabbedLayer);

            for (int i = 1; i < hitObjects.Length; i++)
            {
                Vector3 _colosetPos = hitObjects[closest].transform.position;
                Vector3 _nextPos = hitObjects[i].transform.position;

                float _colsestDistance = Vector3.Distance(_colosetPos, ARAVRInput.LHandPosition);
                float _nextDistance = Vector3.Distance(_nextPos, ARAVRInput.LHandPosition);

                if (_nextDistance < _colsestDistance)
                {
                    closest = i;
                }
            }

            if (hitObjects.Length > 0)
            {
                isLeftGrabbing = true;
                LefftGrabbedObject = hitObjects[closest].gameObject;
                LefftGrabbedObject.transform.parent = ARAVRInput.RHand;
                LefftGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                prevPos = ARAVRInput.LHandPosition;
                prevRot = ARAVRInput.LHand.rotation;
            }
        }
    }
    void TryLeftUngrab()
    {
        Vector3 _throwDirection = ARAVRInput.LHandDirection;
        prevPos = ARAVRInput.LHandPosition;

        Quaternion _deltaRotation = ARAVRInput.LHand.rotation * Quaternion.Inverse(prevRot);

        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            isLeftGrabbing = false;                                             //���� ���� ���·� ��ȯ
            LefftGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;    // ������� Ȱ��ȭ
            LefftGrabbedObject.transform.parent = null;
            LefftGrabbedObject.GetComponent<Rigidbody>().velocity = _throwDirection * throwPower;

            float _angle;
            Vector3 _axis;

            _deltaRotation.ToAngleAxis(out _angle, out _axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * _angle * _axis;

            LefftGrabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
            LefftGrabbedObject = null;
        }
    }

    IEnumerator LeftGrabbingAnimation()
    {
        LefftGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        prevPos = ARAVRInput.LHandPosition;
        prevRot = ARAVRInput.LHand.rotation;
        Vector3 _startLocation = LefftGrabbedObject.transform.position;
        Vector3 _targetLocation = ARAVRInput.LHandPosition + ARAVRInput.LHandDirection * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;
        float elapsedRate = currentTime / finishTime;

        while (elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            LefftGrabbedObject.transform.position = Vector3.Lerp(_startLocation, _targetLocation, elapsedRate);
            yield return null;
        }

        LefftGrabbedObject.transform.position = _targetLocation;
        LefftGrabbedObject.transform.parent = ARAVRInput.LHand;
    }
    #endregion

    #region �÷��̾� ���ݰ� ������ ������ ó��
    void Attack()
    {
        if (ARAVRInput.Get(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
        {
            attackPoint.SetActive(true);
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
            attackPoint.SetActive(false);
    }

    public void Damaged(int _Damage)
    {
        hp -= _Damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AttackPoint")
        {
            Damaged(damage);
            print("�������� ����");
        }
    }
    #endregion

    #region �����ó��
    void Hungry()
    {
        if (!ishunger)
        {
            if (time > 2f)
            {
                hunger--;
                time = 0;

                

                if (hunger <= 0)
                {
                    ishunger = true;
                }
            }
        }

        else if (ishunger)
        {
            if (time > 5f)
            {
                Damaged(10);
                time = 0;
            }
        }
    }
    #endregion
}

