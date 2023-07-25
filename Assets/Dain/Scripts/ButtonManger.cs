using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManger : MonoBehaviour
{
    public GameObject menu;
    public GameObject Control;
    public GameObject Option;
    public GameObject Login;
    public GameObject GameUI;

    private void Start()
    {
        GameUI.SetActive(false);
        menu.SetActive(false);
        Control.SetActive(false);
        Option.SetActive(false);
        Login.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void MenuOpen()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        GameUI.SetActive(false);
    }
    public void ControlOpen()
    {
        Control.SetActive(true);
        Time.timeScale = 0f;
        GameUI.SetActive(false);
    }
    public void OptionOpen()
    {
        Option.SetActive(true);
        Time.timeScale = 0f;
        GameUI.SetActive(false);
    }

    public void Close()
    {
        menu.SetActive(false);
        Control.SetActive(false);
        Option.SetActive(false);
        Login.SetActive(false);
        Time.timeScale = 1f;
        GameUI.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
        Time.timeScale = 1f;

    }    
}
