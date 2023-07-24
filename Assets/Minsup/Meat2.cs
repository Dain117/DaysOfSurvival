using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat2 : MonoBehaviour
{
    public int hungerValue = 10; // ��⸦ �Ծ��� �� ������ų ��� ��
    public int healthValue = 20; // ��⸦ �Ծ��� �� ������ų ü�� ��

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� "Player" �±׸� ���� ���
        if (other.CompareTag("Player"))
        {
            // �÷��̾� ��ũ��Ʈ�� ������
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // �÷��̾��� ���� ü���� ������Ű�� ��� ������Ʈ�� ����
                player.IncreaseHunger(hungerValue);
                //player.IncreaseHealth(healthValue);
            }

            Destroy(gameObject);
        }
    }
}
