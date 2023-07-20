using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public int hp;
    public Slider hpSlider;
    Animator anim;
    public GameObject effect;
    public GameObject chest;
    public GameObject[] Item;
    Vector3 position;
    Vector3 hpScale;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        hp = 30;
        hpScale =hpSlider.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = hp;
        hpSlider.transform.rotation = Camera.main.transform.rotation;
        if (hp >= 30)
            hpSlider.transform.localScale = Vector3.zero;
        else
            hpSlider.transform.localScale = hpScale;

        if (hp<=0)
        {
            anim.SetTrigger("Open");
            StartCoroutine(ChestRemove());

        }
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AttackPoint")
        {
            hp -= (PlayerDain.damage);  
        }
    }

    IEnumerator ChestRemove()
    {
        Instantiate(effect, gameObject.transform);
        yield return new WaitForSeconds(3f);
        Instantiate(Item[Random.RandomRange(0, Item.Length)], gameObject.transform.position, gameObject.transform.rotation);
        Destroy(chest);

    }
}
