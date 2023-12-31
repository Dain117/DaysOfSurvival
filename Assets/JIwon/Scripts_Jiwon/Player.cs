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

    GameObject RightGrabbedObject;          // 잡고있는 물체
    GameObject LefftGrabbedObject;
    
    Vector3 prevPos;                        // 이전 위치
    Quaternion prevRot;                     // 이전 방향

    public LayerMask RightgrabbedLayer;     // 잡고있는 물체 종류
    public LayerMask LeftgrabbedLayer;      // 잡고있는 물체 종류
    public GameObject attackPoint;
    public GameObject PlayerBody;
    public GameObject DeathAnim;
    public Transform objPosition;

    #region 체력바 UI
    public Slider HP; // 체력 슬라이더
    public Slider HG; // 허기 슬라이더
    #endregion

    #region 아이템창 UI
    public Image itemImage; //체력 아이템 창
    public Image usedImage; //아이템 사용시 뜰 빈 이미지
    public Image HGImage; //허기 아이템 창
    #endregion

    public float speed = 5f;
    public float gravity = -20;
    public float jumpPower = 5;
    public float grabRange = 0.2f;          // 잡을 수 있는 거리
    public float remoteGrabDistance = 20f;  // 원거리 물체를 잡을 수 있는 거리
    public bool isRightRemoteGrab = true;   // 원거리 물체를 잡는 기능 활성화 여부
    public bool isLeftRemoteGrab = true;    // 원거리 물체를 잡는 기능 활성화 여부

    public float rotationSpeed = 100f;      // 회전 속도

    float throwPower = 5f;                  // 던질 힘

    protected float time = 0;
    bool isJumping = false;
    float yVelocity = 0;
    public bool isRightGrabbing = false;           // 오른손 물체를 잡고 있는지 여부
    public bool isLeftGrabbing = false;            // 오른손 물체를 잡고 있는지 여부
    bool isAttack = false;
    bool ishunger = false;

    #region 플레이어 상태(체력 등...)
    public static int hp = 0;                      // 체력
    public int Maxhunger = 100;             // 허기
    public int currentHunger;
    public int chill = 0;                   // 한기
    public static int damage = 0;                  // 공격력
    int weaponDamage = 150;                 // 무기에 받는 피해

    public bool isDead = false;
    #endregion

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
       // gameObject.transform.position = gameManager.spawnPoint.transform.position;
        characterController = GetComponent<CharacterController>();
        hp = 100;
        currentHunger = 100;
        damage = 10;
}

    void Update()
    {
        #region 체력바 연결
        HP.value = hp;
        HG.value = currentHunger;
        #endregion

        #region 아이템 사용
        if (Input.GetKeyDown(KeyCode.Q)) //지원님 이거 키 바꿔주세요  
        {
            itemImage = GameObject.Find("ItemImg").GetComponent<Healing>().GetComponent<Image>(); //체력아이템 UI 이미지를 컴포넌트
            if (itemImage.sprite.name == "equip_icon_potion_red_2") // 만약 그 이미지의 이름이 equip_icon_potion_red_2 라면...
            {
                hp += 50; //체력 50회복
                itemImage.sprite = usedImage.sprite; //아이템을 사용했으면 빈이미지로 바꿈

            }
        }

        if (Input.GetKeyDown(KeyCode.W)) //지원님 이것도 키 바꿔주세요

        {
            HGImage = GameObject.Find("HGImg").GetComponent<Meat>().GetComponent<Image>(); //고기아이템 UI 이미지를 컴포넌트
            usedImage = GameObject.Find("UsedImg").GetComponent<Meat>().GetComponent<Image>();  //빈아이템 UI 이미지 컴포넌트
            if (HGImage.sprite.name == "icon_food_meat") //만약 그 이미지의 이름이 icon_food_meat 라면..
            {
                currentHunger += 30;  //허기 30 회복
                {
                    HGImage.sprite = usedImage.sprite; //아이템을 사용했으면 빈이미지로 바꿈
                }
            }
        }
        #endregion

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

        #region 플레이어 왼손 오른손 Grab
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

    #region 플레이어 움직임
    public void Move()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);            // Oculus 오른쪽 Thumbstick 입력 받기
        transform.Rotate(Vector3.up, thumbstickInput.x * rotationSpeed * Time.deltaTime);       // Thumbstick의 x 값으로 캐릭터를 좌우로 회전

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

    #region 플레이어 오른손 Grab
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
            isRightGrabbing = false;                                             //잡지 않은 상태로 전환
            RightGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;    // 물리기능 활성화
            RightGrabbedObject.transform.parent = null;                          //손에서 떼어내기
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

    #region 플레이어 왼손 Grab
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
            isLeftGrabbing = false;                                             //잡지 않은 상태로 전환
            LefftGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;    // 물리기능 활성화
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

    #region 플레이어 공격과 데미지 입음을 처리
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
            print("데미지를 입음");

        }
        if (collision.gameObject.tag == "WeaponAttack")
        {
            Damaged(weaponDamage);
            print("데미지를 입음");
        }
    }
    #endregion

    #region 배고픔처리
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