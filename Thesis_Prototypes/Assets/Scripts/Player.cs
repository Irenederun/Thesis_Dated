using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // ui? TODO: probably gonna separate this out
    [Header("UI")]
    public GameObject UITextIn;
    public GameObject UITextOut;

    // in/out character
    [Header("Character State")]
    public bool isInCharacter;
    public GameObject inCharacter2D;
    public GameObject outCharacter3D;

    // character properties
    [Header("Character Properties")]
    private Rigidbody rb;
    public float speed = 2;
    private Vector3 initialPosPlayer;

    // paddle related
    [Header("Paddle Related")]
    public GameObject paddle;
    public GameObject boat;
    public bool isPaddling;
    public float paddleTime;

    // action flags
    [Header("Action Flags")]
    public bool canMoveIn2D;
    public bool canTalk;
    public bool canEnterBoat;
    public bool canExitBoat;
    public bool canPaddle;
    
    // private fields
    private Flowchart convo;
    private bool isInsideTrigger;
    private GameObject talkTarget;

    // events
    public delegate void Notify();
    public event Notify onEnterBoat;
    public event Notify onExitBoat;
    public event Notify onStartPaddle;
    public event Notify onStopPaddle;

    public delegate void TargetedNotify(GameObject target);
    public event TargetedNotify onTalk;

    private void Awake()
    {
        Services.player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initialPosPlayer = transform.position;
        isPaddling = false;
        canPaddle = false;
        paddle.GetComponent<Animator>().enabled = false;
        convo = gameObject.GetComponent<Flowchart>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isInsideTrigger) SwitchInOutControls();

        if (!isInCharacter)
        {
            OutCharacterControls();
        }
        else
        {
            InCharacterControls();
        }

    }

    public void EnterCharacter()
    {
        isInCharacter = true;

        inCharacter2D.SetActive(true);
        outCharacter3D.SetActive(false);
        backToOriginalPos();

        Services.seeingGM.currentStage.Resume();
    }

    public void ExitCharacter()
    {
        isInCharacter = false;

        inCharacter2D.SetActive(false);
        outCharacter3D.SetActive(true);

        Services.seeingGM.currentStage.Pause();
    }

    private void SwitchInOutControls()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInCharacter && canExitBoat)
            {
                ExitCharacter();
                onExitBoat?.Invoke();
            }
            else if (!isInCharacter && canEnterBoat)
            {
                EnterCharacter();
                onEnterBoat?.Invoke();
            }
        }
    }

    private void InCharacterControls()
    {
        if (canPaddle && !boat.GetComponent<BoatMovement>().blocked)
        {
            if (Input.GetKeyDown(KeyCode.R) && transform.position.x > -5.5f && transform.position.x < -4.8f)
            {
                //paddle.GetComponent<Animator>().SetTrigger("paddling");
                paddle.GetComponent<Animator>().enabled = true;

                isPaddling = true;
                onStartPaddle?.Invoke();
            }

            if (isPaddling)
            {
                paddleTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                //paddle.GetComponent<Animator>().SetTrigger("idle");
                paddle.GetComponent<Animator>().enabled = false;

                isPaddling = false;
                onStopPaddle?.Invoke();
            }
        }
        else // when canPaddle gets set to false while paddling
        {
            if (isPaddling)
            {
                paddle.GetComponent<Animator>().enabled = false;
                isPaddling = false;
                onStopPaddle?.Invoke();
            }
        }


        if (canMoveIn2D && convo.GetVariable<BooleanVariable>("isTalking").Value == false)
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
                onTalk?.Invoke(talkTarget);
                /*
                convo.SendFungusMessage("StartConvo");
                if (!hasTalked)
                {
                    hasTalked = true;
                }
                */
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
        print(isInCharacter);
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

        //Services.seeingGM.CanChangeStatus("yes");

        
        if (other.tag == "UI")
        {
            isInsideTrigger = true;
            //TODO: tutorial UI stuff, should be decoupled from player
            /*
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
            */
        }

        if (other.tag == "Girl")
        {
            /*
            if (!hasTalked)
            {
                UITextIn.GetComponent<TextMeshPro>().text = "[T]alk To";
            }
            */

            //canTalk = true;
            if (talkTarget != other.gameObject) talkTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "UI")
        {
            print("exited trigger");
            UITextIn.SetActive(false);
            UITextOut.SetActive(false);
            //Services.seeingGM.CanChangeStatus("no");
            isInsideTrigger = false;
        }

        if (other.tag == "Girl")
        {
            //canTalk = false;
            if (talkTarget == other.gameObject) talkTarget = null;
        }
    }
}
