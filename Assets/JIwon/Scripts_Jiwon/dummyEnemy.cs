using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dummyEnemy : MonoBehaviour
{
    #region 플레이어 상태(체력 등...)
    public int hp = 0;                      // 체력
    public int hunger = 0;                  // 허기
    public int chill = 0;                   // 한기
    public int damage = 0;                  // 공격력
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
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
   
    void Damaged()
    {
        hp -= damage;
        Debug.Log("damage");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AttackPoint")
        {
            Damaged();
            print("데미지를 입음");
        }
    }
    /*void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag== "AttackPoint")
        {
            Damaged();
            print("데미지를 입음");
        }
    }*/

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AttackPoint")
        {
            Damaged();
        }
    }*/
}