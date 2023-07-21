using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class dummyEnemy : MonoBehaviour
{
    public GameObject PlayerBody;
    public GameObject DeathAnim;
    Transform objPosition;

    bool isDead = false;

    #region 플레이어 상태(체력 등...)
    public int hp = 0;                      // 체력
    public int hunger = 0;                  // 허기
    public int chill = 0;                   // 한기
    public int damage = 0;                  // 공격력
    int weaponDamage = 150;
    #endregion

    void Start()
    {
        hp = 100;
        hunger = 100;
        chill = 100;
        damage = 10;
    }

    void Update()
    {
        objPosition = gameObject.transform;

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
    }
   
    void Damaged(int _damage)
    {
        hp -= _damage;
        Debug.Log("damage");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hp > 0)
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
    }
}