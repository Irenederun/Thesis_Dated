using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject UITextIn;
    public GameObject UITextOut;
    public bool isInCharacter;
    private Rigidbody rb;
    public float speed = 2;
    public GameObject gameManager;
    private Vector3 initialPosPlayer;
    public GameObject paddle;
    public bool isPaddling;
    public bool canPaddle;
    private float paddleTime;
    public float maxPaddleTime = 3;
    private bool shoreTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initialPosPlayer = transform.position;
        isPaddling = false;
        canPaddle = true;
        shoreTime = false;
        paddle.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInCharacter)
        {
            OutCharacterControls();
        }
        else
        {
            InCharacterControls();
        }

        //if (isPaddling)
        //{
        //    paddleTime += Time.deltaTime;
        //}

        if (paddleTime > maxPaddleTime)
        {
            if (!shoreTime)
            {
                shoreTime = true;
                print("get to the shore");
                gameManager.GetComponent<SeeingGM>().GetTheShore();
            }
        }
        
        print(paddleTime);
    }

    private void InCharacterControls()
    {
        if (canPaddle)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                paddle.GetComponent<Animator>().enabled = true;
                //paddle.GetComponent<Animator>().SetTrigger("paddling");
                
                isPaddling = true;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (isPaddling)
                {
                    paddleTime += Time.deltaTime;
                }
            }
        }
        else
        {
            paddle.GetComponent<Animator>().enabled = false;
            isPaddling = false;
        }
        
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            //paddle.GetComponent<Animator>().SetTrigger("idle");
            paddle.GetComponent<Animator>().enabled = false;
                
            isPaddling = false;
        }
    }

    public void ChangePaddleStatus(bool canornot)
    {
        canPaddle = canornot;
    }

    private void OutCharacterControls()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector3.right * speed);
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector3.left * speed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * speed);
        }
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(Vector3.forward * speed);
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(Vector3.back * speed);
        }
        
        rb.velocity *= 0.99f;
    }

    public void backToOriginalPos()
    {
        transform.position = initialPosPlayer;
    }

    public void ChangeStatus(bool status)
    {
        isInCharacter = status;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (isInCharacter)
        {
            UITextIn.SetActive(true);
            UITextOut.SetActive(false);
        }
        else
        {
            UITextOut.SetActive(true);
            UITextIn.SetActive(false);
        }

        gameManager.GetComponent<SeeingGM>().CanChangeStatus("yes");
    }

    private void OnTriggerExit(Collider other)
    {
        UITextIn.SetActive(false);
        UITextOut.SetActive(false);
        gameManager.GetComponent<SeeingGM>().CanChangeStatus("no");
    }
}
