using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
    public int hungerValue = 10; // ��⸦ �Ծ��� �� ������ų ��� ��

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� "Player" �±׸� ���� ���
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� ��⸦ ������Ű�� ��� ������Ʈ�� ����
            //PlayerController playerController = other.GetComponent<PlayerController>();
            //if (playerController != null)
            {
                //playerController.IncreaseHunger(hungerValue);
            }

            Destroy(gameObject);
        }
    }
}
