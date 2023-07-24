using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManger : MonoBehaviour
{
    public GameObject menu;
    public GameObject Control;
    public GameObject Option;
    public GameObject Login;

    private void Start()
    {
        menu.SetActive(false);
        Control.SetActive(false);
        Option.SetActive(false);
        Login.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MenuOpen()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ControlOpen()
    {
        Control.SetActive(true);
        Time.timeScale = 0f;
    }
    public void OptionOpen()
    {
        Option.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        menu.SetActive(false);
        Control.SetActive(false);
        Option.SetActive(false);
        Login.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
        Time.timeScale = 1f;

    }    
}
