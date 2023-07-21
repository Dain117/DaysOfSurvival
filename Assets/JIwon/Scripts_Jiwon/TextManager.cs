using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //public Text NumberOfPeople;
    private Transform mainCam;

    void Start()
    {
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        //NumberOfPeople.transform.rotation = Camera.main.transform.rotation;
        transform.LookAt(mainCam);
    }
}
