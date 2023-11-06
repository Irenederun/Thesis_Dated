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
    public float accel = 10f;
    public float maxSpeed = 3f;
    private Vector3 initialPosPlayer;
    public bool isInsideTrigger;

    // paddle related
    [Header("Paddle Related")]
    public GameObject paddle;
    public GameObject boat;
    public bool isPaddling;
    public float paddleTime;

    // talking related
    [Header("Talking Related")]
    public GameObject talkTarget;
    // action flags
    // **ideally only be modified by Stages and GameManagers
    // **avoid including these in other logics; instead use separate booleans
    [Header("Action Flags")]
    public bool canMoveIn2D;
    public bool canMoveIn3D;
    public bool canTalk;
    public bool canEnterBoat;
    public bool canExitBoat;
    public bool canPaddle;
    
    // private fields
    private Flowchart convo;

    // events
    public delegate void Notify();
    public event Notify onEnterBoat;
    public event Notify onExitBoat;
    public event Notify onStartPaddle;
    public event Notify onStopPaddle;
    public event Notify onMove2D;
    public event Notify onMove3D;

    public delegate void TargetedNotify(GameObject target);
    public event TargetedNotify onTalk;

    private void Awake()
    {
        Services.player = this;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initialPosPlayer = transform.position;
        isPaddling = false;
        canPaddle = false;
        paddle.GetComponent<Animator>().enabled = false;
        convo = gameObject.GetComponent<Flowchart>();
    }

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

    private void FixedUpdate()
    {
        if (!isInCharacter)
        {
            OutCharacterControlsPhysics();
        }
        else
        {
            InCharacterControlsPhysics();
        }
    }

    public void EnterCharacter()
    {
        isInCharacter = true;

        inCharacter2D.SetActive(true);
        outCharacter3D.SetActive(false);
        backToOriginalPos();

        Services.seeingGM.currentStage.Resume();
        Services.uiManager.ShowInCharacterUI();
    }

    public void ExitCharacter()
    {
        isInCharacter = false;

        inCharacter2D.SetActive(false);
        outCharacter3D.SetActive(true);

        Services.seeingGM.currentStage.Pause();
        Services.uiManager.ShowOutCharacterUI();
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
        // paddling
        if (canPaddle && !boat.GetComponent<BoatMovement>().blocked)
        {
            if (Input.GetKeyDown(KeyCode.R) && transform.position.x > -5.5f && transform.position.x < -4.8f)
            {
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


        // talking on boat
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                print("nfdsajkf");
                onTalk?.Invoke(talkTarget);
            }
        }
    }

    private void InCharacterControlsPhysics()
    {
        // moving on boat
        if (canMoveIn2D && convo.GetVariable<BooleanVariable>("isTalking").Value == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.left * accel);
                onMove2D?.Invoke();
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector3.right * accel);
                onMove2D?.Invoke();
            }
        }

    }

    public void ChangePaddleStatus(bool canornot)
    {
        canPaddle = canornot;
    }

    private void OutCharacterControls()
    {

    }

    private void OutCharacterControlsPhysics()
    {
        // moving when out of character
        if (canMoveIn3D)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector3.right * accel);
                onMove3D?.Invoke();
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(Vector3.left * accel);
                onMove3D?.Invoke();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * accel);
                onMove3D?.Invoke();
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(Vector3.forward * accel);
                onMove3D?.Invoke();
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(Vector3.back * accel);
                onMove3D?.Invoke();
            }
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void backToOriginalPos()
    {
        transform.position = initialPosPlayer;
    }

    public void ChangeStatus(bool status)
    {
        isInCharacter = status;
    }

    private void OnTriggerEnter(Collider other)
    {
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

    private void OnTriggerStay(Collider other)
    {
        /*
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
        */
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
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "UI")
        {
            print("exited trigger");
            //UITextIn.SetActive(false);
            //UITextOut.SetActive(false);
            isInsideTrigger = false;
        }

        if (other.tag == "Girl")
        {
            if (talkTarget == other.gameObject) talkTarget = null;
        }
    }
}
