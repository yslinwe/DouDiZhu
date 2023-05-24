using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
public class ShowTip : UIManager{
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            uiObjects.Add(transform.GetChild(i).gameObject);
        }
    }
    public void OpenTimerAndTip()
    {
        OpenUIObject("Timer");
        OpenUIObject("Tip");
    }
    public void OpenTimerAndCloseTip()
    {
        OpenUIObject("Timer");
        CloseUIObject("Tip");
    }
    public void CloseTimerAndOpenTip()
    {
        CloseUIObject("Timer");
        OpenUIObject("Tip");
    }
    public void CloseTimerAndTip()
    {
        CloseUIObject("Timer");
        CloseUIObject("Tip");
    }
    public void OpenTimerTipButton()
    {
        OpenUIObject("Timer");
        OpenUIObject("Tip");
        OpenUIObject("no_cards");
        OpenUIObject("playing_cards");
        Debug.Log(1);
    }
    public void CloseTimerTipButton()
    {
        CloseUIObject("Timer");
        CloseUIObject("Tip");
        CloseUIObject("no_cards");
        CloseUIObject("playing_cards");
    }
    public void OpenButton()
    {
        GetUIObj("no_cards").GetComponent<Button>().interactable = true;
        GetUIObj("playing_cards").GetComponent<Button>().interactable = true;
        OpenUIObject("no_cards");
        OpenUIObject("playing_cards");
    }
    public void setButtonDisable()
    {
        GetUIObj("no_cards").GetComponent<Button>().interactable = false;
        GetUIObj("playing_cards").GetComponent<Button>().interactable = false;
    }
     public void CloseButton()
    {
        CloseUIObject("no_cards");
        CloseUIObject("playing_cards");
    }
    public void OpenCallButton()
    {
        GetUIObj("NoCall").GetComponent<Button>().interactable = true;
        GetUIObj("Call").GetComponent<Button>().interactable = true;
        OpenUIObject("NoCall");
        OpenUIObject("Call");
    }
    public void setCallButtonDisable()
    {
        GetUIObj("NoCall").GetComponent<Button>().interactable = false;
        GetUIObj("Call").GetComponent<Button>().interactable = false;
    }
    public void OpenGrabButton()
    {
        GetUIObj("NoGrab").GetComponent<Button>().interactable = true;
        GetUIObj("Grab").GetComponent<Button>().interactable = true;
        OpenUIObject("NoGrab");
        OpenUIObject("Grab");
    }
    public void setGrabButtonDisable()
    {
        GetUIObj("NoGrab").GetComponent<Button>().interactable = false;
        GetUIObj("Grab").GetComponent<Button>().interactable = false;
    }
     public void CloseGrabButton()
    {
        CloseUIObject("NoGrab");
        CloseUIObject("Grab");
    }
    public void OpenTip()
    {
        CloseAllUIObjects();
        OpenUIObject("Tip");
    }
    public void OpenNormalTip()
    {
        OpenUIObject("Tip");
    }
    public void CloseNormalTip()
    {
        CloseUIObject("Tip");
    }
    public TextMeshProUGUI GetTip()
    {
        return GetUIObj("Tip").GetComponent<TextMeshProUGUI>();
    }
     public TextMeshProUGUI GetTimerTip()
    {
        return GetUIObj("Timer").GetComponentInChildren<TextMeshProUGUI>();
    }
}
