using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("������ �ε���");
            Destroy(gameObject);
        }
    }
        public void RunItem()
    {
        print("�����ۻ��");
        Destroy(gameObject);
    }
}
