using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public CharacterController characterController;

    GameObject RightGrabbedObject;          // ����ִ� ��ü
    GameObject LefftGrabbedObject;
    
    Vector3 prevPos;                        // ���� ��ġ
    Quaternion prevRot;                     // ���� ����

    public LayerMask RightgrabbedLayer;     // ����ִ� ��ü ����
    public LayerMask LeftgrabbedLayer;      // ����ִ� ��ü ����
    public GameObject attackPoint;
    public GameObject PlayerBody;
    public GameObject DeathAnim;
    public Transform objPosition;

    public Slider HP;
    public Slider HG;

    public Image itemImage;
    public Image usedImage;
    public Image HGImage;

    public float speed = 5f;
    public float gravity = -20;
    public float jumpPower = 5;
    public float grabRange = 0.2f;          // ���� �� �ִ� �Ÿ�
    public float remoteGrabDistance = 20f;  // ���Ÿ� ��ü�� ���� �� �ִ� �Ÿ�
    public bool isRightRemoteGrab = true;   // ���Ÿ� ��ü�� ��� ��� Ȱ��ȭ ����
    public bool isLeftRemoteGrab = true;    // ���Ÿ� ��ü�� ��� ��� Ȱ��ȭ ����

    public float rotationSpeed = 100f;      // ȸ�� �ӵ�

    float throwPower = 5f;                  // ���� ��

    protected float time = 0;
    bool isJumping = false;
    float yVelocity = 0;
    public bool isRightGrabbing = false;           // ������ ��ü�� ��� �ִ��� ����
    public bool isLeftGrabbing = false;            // ������ ��ü�� ��� �ִ��� ����
    bool isAttack = false;
    bool ishunger = false;

    #region �÷��̾� ����(ü�� ��...)
    public static int hp = 0;                      // ü��
    public int Maxhunger = 100;             // ���
    public int currentHunger;
    public int chill = 0;                   // �ѱ�
    public static int damage = 0;                  // ���ݷ�
    int weaponDamage = 150;                 // ���⿡ �޴� ����

    public bool isDead = false;
    #endregion

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameObject.transform.position = gameManager.spawnPoint.transform.position;
        characterController = GetComponent<CharacterController>();
        hp = 100;
        currentHunger = 100;
        damage = 10;
}

    void Update()
    {
        HP.value = hp;
        HG.value = currentHunger;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            itemImage = GameObject.Find("ItemImg").GetComponent<Healing>().GetComponent<Image>();
            if (itemImage.sprite.name == "equip_icon_potion_red_2")
            {
                hp += 50;
                itemImage.sprite = usedImage.sprite;

            }
        }

        if (Input.GetKeyDown(KeyCode.W))

        {
            HGImage = GameObject.Find("HGImg").GetComponent<Meat>().GetComponent<Image>();
            usedImage = GameObject.Find("UsedImg").GetComponent<Meat>().GetComponent<Image>();
            if (HGImage.sprite.name == "icon_food_meat")
            {
                currentHunger += 50;
                HGImage.sprite = usedImage.sprite;
            }
        }

        time += Time.deltaTime;

        Move();
        Attack();
        Hungry();

        if (hp <= 0)
        {
            PlayerBody.SetActive(false);

            if (isDead == false)
            {
                GameObject dead = Instantiate(DeathAnim);
                dead.transform.position = objPosition.position;
                isDead = true;
            }
        }

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
    }

    #region �÷��̾� ������
    public void Move()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);            // Oculus ������ Thumbstick �Է� �ޱ�
        transform.Rotate(Vector3.up, thumbstickInput.x * rotationSpeed * Time.deltaTime);       // Thumbstick�� x ������ ĳ���͸� �¿�� ȸ��

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
    public void TryRightGrab()
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
    public void TryRightUngrab()
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

    public IEnumerator RightGrabbingAnimation()
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
    public void TryLeftGrab()
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
    public void TryLeftUngrab()
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

    public IEnumerator LeftGrabbingAnimation()
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
    public virtual void Attack()
    {
        if (gameManager.isGameStart)
        {
            if (ARAVRInput.Get(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
            {
                attackPoint.SetActive(true);
            }
            else if (ARAVRInput.GetUp(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
                attackPoint.SetActive(false);
        }
    }

    public void Damaged(int _Damage)
    {
        hp -= _Damage;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AttackPoint")
        {
            Damaged(damage);
            print("�������� ����");

        }
        if (collision.gameObject.tag == "WeaponAttack")
        {
            Damaged(weaponDamage);
            print("�������� ����");
        }
    }
    #endregion

    #region �����ó��
    public void Hungry()
    {
        if (!ishunger)
        {
            if (time > 2f)
            {
                currentHunger -= 2;
                time = 0;

                Debug.Log(currentHunger);

                if(currentHunger <= 0)
                {
                    ishunger = true;
                }
            }
        }

        else if (ishunger)
        {
            if (time > 2f)
            {
                Damaged(10);
                time = 0;
            }
        }
    }

    public void IncreaseHunger(int amount)
    {
        currentHunger += amount;

        currentHunger = Mathf.Clamp(currentHunger, 0, Maxhunger);
    }
    #endregion
}