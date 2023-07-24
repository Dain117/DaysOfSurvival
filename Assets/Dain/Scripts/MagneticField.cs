using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    public GameObject field;
    public float radius;
    float time;
    float num;
    float Scale;

    // Update is called once per frame

    void Update()
    {
        if (GameObject.Find("Game").GetComponent<TimeText>().night == false)
        { radius = 0.1f; num = 100; Scale = 180;
            field.transform.localScale = new Vector3(0, 0, 0);
;        }

        else if (GameObject.Find("Game").GetComponent<TimeText>().night == true)
        {
            field.transform.localScale = new Vector3(Scale, Scale, Scale);
            if(Scale>7)
            {
                Scale -= Time.deltaTime * 10;
            }
            radius = num;
            if (num > 4)
            {
                num-=Time.deltaTime*10;
            }
            
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        {
            
            Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, radius, 1 << 7);

            
            foreach (Collider collider in cols)
            {

                if (collider.name == "Player")
                    StartCoroutine(HealingHP());

            }
            
        }
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,1,0.5f);
        Gizmos.DrawSphere(gameObject.transform.position, radius);
    }

    IEnumerator HealingHP()
    {
        time += Time.deltaTime;
        while (time > 1.0f)
        {
            Player.hp += 5;
            time = 0;
            yield return null;
        }
    }

    
}
