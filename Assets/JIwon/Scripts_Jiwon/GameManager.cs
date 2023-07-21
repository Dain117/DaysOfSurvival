using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text WaitForGameStart;
    public GameObject InviWall;

    int numOfPeople = 1;
    int maxCapacity = 6;
    int startTimer = 5;
    float time = 0f;

    public bool isGameStart = false;

    void Start()
    {

    }

    void Update()
    {
        WaitForGameStartText();

        if (startTimer <= 0)
        {
            isGameStart = true;
            InviWall.SetActive(false);
            WaitForGameStart.text = "";
        }

        WaitForGameStart.transform.rotation = Camera.main.transform.rotation;
    }

    void  WaitForGameStartText()
    {
        time += Time.deltaTime;

        if (numOfPeople < maxCapacity)
        {
            if (time > 3f)
            {
                numOfPeople++;
                WaitForGameStart.text = $"{numOfPeople.ToString()}/{maxCapacity.ToString()}";
                time = 0f;
            }
        }

        if (numOfPeople == maxCapacity)
        {
            StartCoroutine(CountForGameStart());
            numOfPeople++;
        }
    }

    IEnumerator CountForGameStart()
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