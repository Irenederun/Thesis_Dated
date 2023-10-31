using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public GameObject camera2D;
    public GameObject player;
    private bool canShakeScreen;

    // Start is called before the first frame update
    void Start()
    {
        canShakeScreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "body")
        {
            if (canShakeScreen)
            {
                camera2D.GetComponent<CameraShakeScreen>().ShakeScreen();
                player.GetComponent<PlayerController>().ChangePaddleStatus(false);
            }
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "body")
        {
            player.GetComponent<PlayerController>().ChangePaddleStatus(true);
        }
    }

    public void TrackCharacterStatus(bool isInCharacter)
    {
        canShakeScreen = isInCharacter;
    }
}
