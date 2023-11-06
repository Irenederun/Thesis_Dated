using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public GameObject camera2D;
    public bool blocked;
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
            //gameManager.GetComponent<SeeingGM>().CanMoveBody();
            collision.gameObject.GetComponent<Body>().StopMoving();

            blocked = true;
            canShakeScreen = Services.player.isInCharacter;
            if (canShakeScreen)
            {
                camera2D.GetComponent<CameraShakeScreen>().ShakeScreen();
                //Services.player.ChangePaddleStatus(false);
            }
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "body")
        {
            //Services.player.ChangePaddleStatus(true);
            blocked = false;
        }
    }

    public void TrackCharacterStatus(bool isInCharacter)
    {
        canShakeScreen = isInCharacter;
    }
}
