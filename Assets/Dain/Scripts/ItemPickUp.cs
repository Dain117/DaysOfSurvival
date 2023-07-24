using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickUp : MonoBehaviour
{
    
    public Image itemImage;
    public Image usedImage;
    public Image HGImage;
    public Item item;
    public static int twigCount;
    private TextMeshProUGUI questUI;

    private void Start()
    {
        
        
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AttackPoint")
        {

            if (gameObject.GetComponent<ItemPickUp>().item.itemImage.name == "equip_icon_potion_red_2")
            {
                itemImage = GameObject.Find("ItemImg").GetComponent<Healing>().GetComponent<Image>();
                usedImage = GameObject.Find("UsedImg").GetComponent<Meat>().GetComponent<Image>();
                itemImage.sprite = gameObject.GetComponent<ItemPickUp>().item.itemImage;
                Destroy(gameObject);
            }
            else if (gameObject.GetComponent<ItemPickUp>().item.itemImage.name == "icon_food_meat")
            {
                HGImage = GameObject.Find("HGImg").GetComponent<Meat>().GetComponent<Image>();
                HGImage = GameObject.Find("HGImg").GetComponent<Meat>().GetComponent<Image>();
                HGImage.sprite = gameObject.GetComponent<ItemPickUp>().item.itemImage;
                Destroy(gameObject);
            }
            else if (gameObject.GetComponent<ItemPickUp>().item.itemName == "Twig")
            questUI = GameObject.Find("QuestText").GetComponent<TextMeshProUGUI>();
            Destroy(gameObject);
                twigCount++;
            questUI.text = $"Let's collect twig\n20 / {twigCount}";
        }
    }

}
