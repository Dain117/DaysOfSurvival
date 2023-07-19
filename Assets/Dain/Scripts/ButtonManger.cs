using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManger : MonoBehaviour
{
    public GameObject menu;
    public GameObject Control;
    public GameObject Option;


    private void Start()
    {
        menu.SetActive(false);
        Control.SetActive(false);
        Option.SetActive(false);
    }

    public void MenuOpen()
    {
        menu.SetActive(true);
    }
    public void ControlOpen()
    {
        Control.SetActive(true);
    }
    public void OptionOpen()
    {
        Option.SetActive(true);
    }

    public void Close()
    {
        menu.SetActive(false);
        Control.SetActive(false);
        Option.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            MenuOpen();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }    
}
