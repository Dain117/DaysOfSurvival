using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text WaitForGameStart;       // ���� ���� �� ��ں� ���� ǥ�õǴ� �ο��� �� ī��Ʈ�ٿ�
    public GameObject InviWall;         // ���� ���� �� �÷��̾��� �̵��� �������� �����Դϴ�.
    public GameObject spawnPoint;       // �÷��̾� ���� ����Ʈ�� ��ġ���� �޾ƿ��� �����Դϴ�.

    int numOfPeople = 1;                // ���� �ο���
    int maxCapacity = 6;                // �� �ο���
    int startTimer = 5;                 // ���� ���� �� 5�� ī��Ʈ�ٿ�
    float time = 0f;                    // �ӽ� timer�Դϴ�.

    public bool isGameStart = false;    // ���� ���� ���θ� üũ�մϴ�.

    void Start()
    {
        
    }

    void Update()
    {
        WaitForGameStartText();                                                 // ������ Start�ϱ� ���� ��ں� ���� �÷��̾� �ο����� ī��Ʈ �ٿ�

        if (startTimer <= 0)                                                    // ���� ���� ī��Ʈ�ٿ��� 0 ���϶��
        {
            isGameStart = true;                                                 // ���ӽ����� true�� �����մϴ�. (Player ��ũ��Ʈ�� Enemy ��ũ��Ʈ���� ��ȣ�ۿ��մϴ�.)
            InviWall.SetActive(false);                                          // ������ �����մϴ�.
            WaitForGameStart.text = "";                                         // ��ں� �� �ؽ�Ʈ�� ����ϴ�.
        }

        WaitForGameStart.transform.rotation = Camera.main.transform.rotation;   // ī�޶� ���� �������� �ؽ�Ʈ�� ȸ����ŵ�ϴ�.
    }

    void  WaitForGameStartText()                                                
    {
        time += Time.deltaTime;     

        if (numOfPeople < maxCapacity)                                          // ���� �ο����� �� �ο������� ���� ��
        {
            if (time > 3f)                                                      // (�ӽ�) 3�ʸ��� �ο����� 1����
            {
                numOfPeople++;
                WaitForGameStart.text = $"{numOfPeople.ToString()}/{maxCapacity.ToString()}";
                time = 0f;
            }
        }

        if (numOfPeople == maxCapacity)                                         // ���� �ο����� �� �ο����� ���ٸ�
        {
            StartCoroutine(CountForGameStart());                                // 5�� ī��Ʈ�ٿ� ����
            numOfPeople++;                                                      // Update�Լ��� ���� ������ �����ֱ� ���� �÷��̾� �� +1
        }
    }

    IEnumerator CountForGameStart()                                             // 1�ʸ��� ī��Ʈ �ٿ��� ���� �ڷ�ƾ
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