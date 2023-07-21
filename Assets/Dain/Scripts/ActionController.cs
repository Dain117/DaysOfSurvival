using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //ΩªµÏ ∞°¥…«— √÷¥Î ∞≈∏Æ.
    private bool pickupActivated = false;

    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    public TextMeshProUGUI itemText;

    // Update is called once per frame
    void Update()
    {
        TryAction();   
    }

    void TryAction()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Destroy(hitInfo.transform.gameObject);
                InfoDisAppear();
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "»πµÊ");
            }
        }
    }
    void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "»πµÊ");
            ItemInfoAppear();
            
        }
        else
            InfoDisAppear();
    }

    void ItemInfoAppear()
    {
        pickupActivated = true;
        itemText.gameObject.SetActive(true);
        itemText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "»πµÊ" + "<color=yellow>" + "(X)" + "</color>";
    }
    
    void InfoDisAppear()
    {
        pickupActivated = false;
        itemText.gameObject.SetActive(false);
    }
}
