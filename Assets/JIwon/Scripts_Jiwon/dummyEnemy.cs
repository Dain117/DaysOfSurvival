using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dummyEnemy : MonoBehaviour
{
    #region �÷��̾� ����(ü�� ��...)
    public int hp = 0;                      // ü��
    public int hunger = 0;                  // ���
    public int chill = 0;                   // �ѱ�
    public int damage = 0;                  // ���ݷ�
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
            print("�������� ����");
        }
    }
    /*void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag== "AttackPoint")
        {
            Damaged();
            print("�������� ����");
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