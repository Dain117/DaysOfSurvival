using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeHP : MonoBehaviour
{
    public int hp;
    public GameObject twig;
    MeshRenderer mine;
    bool check;
 
    // Start is called before the first frame update
    void Start()
    {
        check = true;
        hp = 30;
        mine = gameObject.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {

        if (hp <= 0 && check ==true)
        {
            StartCoroutine(TreeRemove());   
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AttackPoint")
        {
            hp -= (PlayerDain.damage);
        }
    }

    IEnumerator TreeRemove()
    {
        Vector3 pos = transform.position + Vector3.up*1f;
       
            mine.enabled = false;
            Instantiate(twig, pos, gameObject.transform.rotation).transform.parent = GameObject.Find("Item").GetComponent<ItemSpawn>().transform;
            check= false;
            yield return new WaitForSeconds(30f);
            hp = 30;
            mine.enabled = true;
            check = true;
    }

}
