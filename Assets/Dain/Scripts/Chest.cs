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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hp = 30;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = hp;
        hpSlider.transform.rotation = Camera.main.transform.rotation;

        if (Input.GetKey(KeyCode.Q))
        {
            anim.SetTrigger("Open");
            Instantiate(Item[Random.RandomRange(0,Item.Length)], gameObject.transform.position,gameObject.transform.rotation);
            StartCoroutine(ChestRemove());

        }
    }

    IEnumerator ChestRemove()
    {
        Instantiate(effect, gameObject.transform);
        yield return new WaitForSeconds(3f);
        Destroy(chest);
    }
}
