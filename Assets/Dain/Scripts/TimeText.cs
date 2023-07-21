using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    public static bool isStart;
    public Light light;
    Color lightColor;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI nightText;
    float dayTime;
    float nightTime;
    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        dayTime = 10f;
        nightTime = 12f;
        lightColor = light.color;
    }

    // Update is called once per frame
    void Update()
    {

        if (dayTime > 0)
        {
            light.intensity -=Time.deltaTime/10;
            nightText.text = "";
            dayTime -= Time.deltaTime;
            dayText.text = dayTime.ToString("F0");
        }
        else if (dayTime <= 0 && nightTime > 0)
        {
            light.intensity += Time.deltaTime / 10;
            dayText.text = "";
            nightTime -= Time.deltaTime;
            nightText.text = nightTime.ToString("F0");
        }
        else if (dayTime <= 0 && nightTime <= 0)
        {
            dayTime = 10f;
            nightTime = 12f;
        }



    }
}
