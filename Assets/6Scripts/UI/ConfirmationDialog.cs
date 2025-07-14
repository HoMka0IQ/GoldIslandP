using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class ConfirmationDialog : MonoBehaviour
{
    public TMP_Text text;
    public event Action OnAccept;
    public Animation anim;

    public static ConfirmationDialog instance;
    private void Awake()
    {
        instance = this;
    }
    public void SetDialog(Action action, string text)
    {
        anim.Play("CDialogShowAnim");
        this.text.text = text;
        OnAccept += action;
    }

    public void AcceptBtn()
    {
        anim.Play("CDialogHideAnim");
        OnAccept?.Invoke();
        OnAccept = null;
    }
    public void DecilBtn()
    {
        anim.Play("CDialogHideAnim");


        OnAccept = null;
    }
}
