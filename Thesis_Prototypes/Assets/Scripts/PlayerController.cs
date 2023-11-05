using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
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
    private bool canTalk;
    
    private bool hasPaddled = false;
    private bool hasTalked = false;
    
    public Flowchart convo;
    public bool hasCollided = false;
        
    public enum GameState
    {
        tutorial,
        interval,
        first,
        second,
        third,
        gameEnd
    }

    public GameState currentState;

    /*

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initialPosPlayer = transform.position;
        isPaddling = false;
        canPaddle = true;
        shoreTime = false;
        paddle.GetComponent<Animator>().enabled = false;
        convo = gameObject.GetComponent<Flowchart>();
        currentState = GameState.tutorial;
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

        if (hasTalked && hasPaddled && currentState == GameState.tutorial)
        {
            gameManager.GetComponent<SeeingGM>().FirstWaveOfBody();
            currentState = GameState.first;
        }
    }

    private void InCharacterControls()
    {
        if (canPaddle)
        {
            if (Input.GetKeyDown(KeyCode.R) && transform.position.x > -5.5f && transform.position.x < -4.8f)
            {
                paddle.GetComponent<Animator>().enabled = true;
                //paddle.GetComponent<Animator>().SetTrigger("paddling");

                isPaddling = true;
            }

            if (Input.GetKey(KeyCode.R) && transform.position.x > -5.5f && transform.position.x < -4.8f)
            {
                if (isPaddling)
                {
                    paddleTime += Time.deltaTime;
                }
                
                if (paddleTime > 1f && !hasPaddled)
                {
                    hasPaddled = true;
                }
            }
        }
        else
        {
            paddle.GetComponent<Animator>().enabled = false;
            isPaddling = false;
        }
        
        if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            //paddle.GetComponent<Animator>().SetTrigger("idle");
            paddle.GetComponent<Animator>().enabled = false;
                
            isPaddling = false;
        }

        if (hasPaddled && convo.GetVariable<BooleanVariable>("isTalking").Value == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.left * speed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector3.right * speed);
            }
        }
        
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                print("nfdsajkf");
                convo.SendFungusMessage("StartConvo");
                if (!hasTalked)
                {
                    hasTalked = true;
                }
            }
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

        if (other.tag == "UI")
        {
            if (currentState == GameState.tutorial && !hasPaddled)
            {
                UITextIn.GetComponent<TextMeshPro>().text = "[R] to Paddle";
            }
            else if (currentState == GameState.tutorial && hasPaddled)
            {
                UITextIn.GetComponent<TextMeshPro>().text = "[A][D] to Move";
            }
            else if (!hasCollided)
            {
                UITextIn.GetComponent<TextMeshPro>().text = "[R] to Paddle";
            }
            else if (currentState == GameState.first)
            {
                UITextIn.GetComponent<TextMeshPro>().text = "[E]xit Character";
            }
            else if (currentState != GameState.tutorial)
            {
                UITextIn.GetComponent<TextMeshPro>().text = "[R] or [E]";
            }
            
        }

        if (other.tag == "Girl")
        {
            if (!hasTalked)
            {
                UITextIn.GetComponent<TextMeshPro>().text = "[T]alk To";
            }
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "UI")
        {
            UITextIn.SetActive(false);
            UITextOut.SetActive(false);
            gameManager.GetComponent<SeeingGM>().CanChangeStatus("no");
        }

        if (other.tag == "Girl")
        {
            canTalk = false;
        }
    }

    */
}
