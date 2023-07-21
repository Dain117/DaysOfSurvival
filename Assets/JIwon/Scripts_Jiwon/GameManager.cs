using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text WaitForGameStart;       // 게임 시작 전 모닥불 위에 표시되는 인원수 및 카운트다운
    public GameObject InviWall;         // 게임 시작 전 플레이어의 이동을 제어해줄 투명벽입니다.
    public GameObject spawnPoint;       // 플레이어 스폰 포인트의 위치값을 받아오기 위함입니다.

    int numOfPeople = 1;                // 현재 인원수
    int maxCapacity = 6;                // 총 인원수
    int startTimer = 5;                 // 게임 시작 전 5초 카운트다운
    float time = 0f;                    // 임시 timer입니다.

    public bool isGameStart = false;    // 게임 시작 여부를 체크합니다.

    void Start()
    {
        
    }

    void Update()
    {
        WaitForGameStartText();                                                 // 게임을 Start하기 전에 모닥불 위에 플레이어 인원수와 카운트 다운

        if (startTimer <= 0)                                                    // 게임 시작 카운트다운이 0 이하라면
        {
            isGameStart = true;                                                 // 게임시작을 true로 변경합니다. (Player 스크립트와 Enemy 스크립트에서 상호작용합니다.)
            InviWall.SetActive(false);                                          // 투명벽을 제거합니다.
            WaitForGameStart.text = "";                                         // 모닥불 위 텍스트를 지웁니다.
        }

        WaitForGameStart.transform.rotation = Camera.main.transform.rotation;   // 카메라가 보는 방향으로 텍스트를 회전시킵니다.
    }

    void  WaitForGameStartText()                                                
    {
        time += Time.deltaTime;     

        if (numOfPeople < maxCapacity)                                          // 현재 인원수가 총 인원수보다 적을 때
        {
            if (time > 3f)                                                      // (임시) 3초마다 인원수가 1증가
            {
                numOfPeople++;
                WaitForGameStart.text = $"{numOfPeople.ToString()}/{maxCapacity.ToString()}";
                time = 0f;
            }
        }

        if (numOfPeople == maxCapacity)                                         // 현재 인원수가 총 인원수와 같다면
        {
            StartCoroutine(CountForGameStart());                                // 5초 카운트다운 실행
            numOfPeople++;                                                      // Update함수의 무한 루프를 막아주기 위한 플레이어 수 +1
        }
    }

    IEnumerator CountForGameStart()                                             // 1초마다 카운트 다운을 위한 코루틴
    {
        yield return new WaitForSeconds(3f);

        while (startTimer > 0)
        {
            WaitForGameStart.text = $"{startTimer.ToString()}";
            yield return new WaitForSeconds(1f);
            startTimer--;
        }
    }
}