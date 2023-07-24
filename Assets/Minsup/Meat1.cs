using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat1 : MonoBehaviour
{
    public int hungerValue = 10; // 고기를 먹었을 때 증가시킬 허기 값

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 "Player" 태그를 가진 경우
        if (other.CompareTag("Player"))
        {
            // 플레이어 스크립트를 가져옴
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // 플레이어의 허기를 증가시키고 고기 오브젝트를 제거
                //player.IncreaseHunger(hungerValue);
            }

            Destroy(gameObject);
        }
    }
}
