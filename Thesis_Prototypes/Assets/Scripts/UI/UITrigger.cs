using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrigger : MonoBehaviour
{
    public GameObject UI2D;
    public GameObject UI3D;

    private bool active;

    private void Start()
    {
        if (UI2D != null) UI2D.SetActive(false);
        if (UI3D != null) UI3D.SetActive(false);
    }
    
    private void Update()
    {
        if (!Services.uiManager.triggerUIEnabled) return;
        if (!active)
        {
            if (UI2D != null) UI2D.SetActive(false);
            if (UI3D != null) UI3D.SetActive(false);
        }
        else
        {
            if (Services.player.isInCharacter)
            {
                if (UI2D != null) UI2D.SetActive(true);
                if (UI3D != null) UI3D.SetActive(false);
            }
            else
            {
                if (UI2D != null) UI2D.SetActive(false);
                if (UI3D != null) UI3D.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            if (Services.uiManager.triggerUIEnabled) active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            if (Services.uiManager.triggerUIEnabled) active = false;
        }
    }
}
