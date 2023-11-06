using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject UITextIn;
    public GameObject UITextOut;

    public bool triggerUIEnabled;

    private void Awake()
    {
        Services.uiManager = this;
    }

    private void Update()
    {

    }

    public void SetInCharacterUI(string text)
    {
        UITextIn.GetComponent<TMP_Text>().text = text;
    }

    public void SetOutCharacterUI(string text)
    {
        UITextOut.GetComponent<TMP_Text>().text = text;
    }

    public void ShowInCharacterUI()
    {
        UITextIn.SetActive(true);
        UITextOut.SetActive(false);
    }

    public void ShowOutCharacterUI()
    {
        UITextIn.SetActive(false);
        UITextOut.SetActive(true);
    }
}
