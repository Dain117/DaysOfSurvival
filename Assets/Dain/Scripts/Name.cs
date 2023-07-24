using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Name : MonoBehaviour
{
    public TMP_InputField name;
    public TextMeshProUGUI nameText;
    private string playerName;

    // Start is called before the first frame update
    void Start()
    {
        playerName = name.GetComponent<TMP_InputField>().text;
    }

    // Update is called once per frame
    void Update()
    {
      nameText.text = name.text;
    }
}
