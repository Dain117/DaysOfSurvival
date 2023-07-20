using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject go_SloatsParents;
    private Slot[] slots;
    int inventoryCount;
    // Start is called before the first frame update
    void Start()
    {
        slots = go_SloatsParents.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {if(Input.GetKeyUp(KeyCode.Tab))
        NextInventory();
    }

    void NextInventory()
    {
        //slots[inventoryCount + 1].itemImage.SetActive(true);
    }

    public void AcquireItem(Item _item)
    {
        for(int i =0; i < slots.Length; i++)
        {
            if (slots[i].item.itemName != null)
            {
                if (slots[i].item.itemName == _item.itemName)
                {
                    slots[i].AddItem(_item);
                    return;
                }
            }
        }
    }

}
