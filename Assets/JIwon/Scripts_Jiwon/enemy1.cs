using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy1 : Player
{
    public GameObject weaponAttack;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameObject.transform.position = gameManager.spawnPoint.transform.position;
        characterController = FindObjectOfType<CharacterController>();
        hp = 150;
        currentHunger = 100;
        damage = 10;
    }

    void Update()
    {
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
                dead.transform.position = objPosition.transform.position;
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

    public override void Attack()
    {
        if (gameManager.isGameStart)
        {
            if (ARAVRInput.Get(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
                attackPoint.SetActive(true);
            else if (ARAVRInput.GetUp(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
                attackPoint.SetActive(false);

            if (ARAVRInput.Get(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch))
                weaponAttack.SetActive(true);
            else if (ARAVRInput.GetUp(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch))
                weaponAttack.SetActive(false);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AttackPoint")
        {
            Damaged(damage);
            print("데미지를 입음");
        }
    }
}
