using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    public static bool isStart;
    public Light light;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI nightText;
    float dayTime;
    float nightTime;
    public GameObject sunGroup;
    public GameObject sky;
    public GameObject magnetic;
    public float radius = 0.1f;
    float time;
    public bool night;
    public static int twigCount;
    public TextMeshProUGUI Quest;


    // Start is called before the first frame update
    void Start()
    {
        night = false;
        isStart = true;
        dayTime = 30;
        nightTime = 30f;
        //twigCount = GameObject.FindWithTag("Item").GetComponent<ItemPickUp>().twigCount;
    }

    // Update is called once per frame
    void Update()
    {

        if (dayTime > 0)
        {
            night = false;
            sky.SetActive(true);
            sunGroup.SetActive(true);
            light.intensity -=Time.deltaTime/dayTime;
            nightText.text = "";
            dayTime -= Time.deltaTime;
            dayText.text = dayTime.ToString("F0");
        }
        else if (dayTime <= 0 && nightTime > 0)
        {
            night = true;
            StartCoroutine(AddDamage());
            sky.SetActive(false);
            sunGroup.SetActive(false);
            light.intensity += Time.deltaTime / dayTime;
            dayText.text = "";
            nightTime -= Time.deltaTime;
            nightText.text = nightTime.ToString("F0");
        }
        else if (dayTime <= 0 && nightTime <= 0)
        {
            twigCount = 0;
            dayTime = 30f;
            nightTime = 30f;
        }

        //Quest.text = ItemPickUp.twigCount.ToString();

    }

    IEnumerator AddDamage()
    {
        
        time += Time.deltaTime;
        while (time>1.0f)
        {   
            GameObject.Find("Player").GetComponent<PlayerDain>().hp -= 5;
            time = 0;
            yield return null;
        }
    }

    
}
