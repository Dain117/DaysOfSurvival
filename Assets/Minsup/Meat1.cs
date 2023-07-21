using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat1 : MonoBehaviour
{
    public int hungerValue = 10; // ��⸦ �Ծ��� �� ������ų ��� ��

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� "Player" �±׸� ���� ���
        if (other.CompareTag("Player"))
        {
            // �÷��̾� ��ũ��Ʈ�� ������
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // �÷��̾��� ��⸦ ������Ű�� ��� ������Ʈ�� ����
                //player.IncreaseHunger(hungerValue);
            }

            Destroy(gameObject);
        }
    }
}
