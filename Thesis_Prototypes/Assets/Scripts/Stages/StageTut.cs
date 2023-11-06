using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StageTut : Stage
{
    public enum TutStates
    {
        Inactive, Started, Paddled, Moved, Talked, HitbyBody, ExitedBoat, PushedBodyAway, ReEnteredBoat
    }
    [SerializeField]
    public TutStates currentState;

    // sequence flags
    [Header("sequence flags")]
    public bool hasPaddled;
    public bool hasMoved;
    public bool hasFirstTalked;
    public bool hasHitByBody;

    // body related
    [Header("bodyyyy")]
    public GameObject firstBody;

    [Space(10f)]
    public GameObject TalkToUI;

    // private fields
    private Flowchart convo;
    private bool countingPaddleTime;
    private float paddleTime;
    private bool isOutOfBound;

    private void Update()
    {
        if (countingPaddleTime)
        {
            paddleTime += Time.deltaTime;
            if (paddleTime > 1f && !hasPaddled)
            {
                hasPaddled = true;
                currentState = TutStates.Paddled;
                Services.uiManager.SetInCharacterUI("[A][D] to Move");
                Services.player.canMoveIn2D = true;
                Services.player.canTalk = true;
            }
        }

        // temporary UI before triggerUI gets activated globally
        if (currentState == TutStates.Moved)
        {
            if (Services.player.talkTarget != null && Services.player.talkTarget.tag == "Girl")
            {
                //Services.uiManager.SetInCharacterUI("[T]alk To");
                TalkToUI.SetActive(true);
            }
            else
            {
                //Services.uiManager.SetInCharacterUI("");
                TalkToUI.SetActive(false);
            }
        }
        else if (currentState == TutStates.HitbyBody)
        {
            if (Services.player.isInsideTrigger)
            {
                Services.uiManager.SetInCharacterUI("[E]xit Character");
            }
            else
            {
                Services.uiManager.SetInCharacterUI("");
            }
        }
        
        
        if (currentState == TutStates.ExitedBoat)
        {
            if (!firstBody.GetComponent<Body>().willHitBoat())
            {
                currentState = TutStates.PushedBodyAway;

                Services.uiManager.SetOutCharacterUI("Go Back to Boat");
                Services.uiManager.triggerUIEnabled = true;
                Services.player.canEnterBoat = true;
            }
        }
    }

    public override void Entrance()
    {
        convo = Services.player.GetComponent<Flowchart>();

        // subscribe to events
        Services.player.onStartPaddle += startCountPaddleTime;
        Services.player.onStopPaddle += stopCountPaddleTime;
        Services.player.onMove2D += checkFirstMove;
        Services.player.onTalk += startConvo;
        Services.player.onEnterBoat += checkFirstHitConvo;
        Services.player.onExitBoat += checkFirstExitBoat;

        // initial setting
        Services.player.canPaddle = true;
        Services.player.canMoveIn2D = false;
        Services.player.canTalk = false;
        Services.player.canMoveIn3D = false;
        currentState = TutStates.Started;

        // show paddle UI
        Services.uiManager.SetInCharacterUI("[R] to Paddle");
        Services.uiManager.SetOutCharacterUI("[E]nter Character");

        // starts in character
        Services.player.EnterCharacter();
    }

    public override void Exit()
    {
        Services.player.onStartPaddle -= startCountPaddleTime;
        Services.player.onStopPaddle -= stopCountPaddleTime;
        Services.player.onTalk -= startConvo;
        Services.player.onEnterBoat -= checkFirstHitConvo;
    }

    public override void Pause()
    {
        if (firstBody != null) firstBody.GetComponent<Body>().StopMoving();
    }

    public override void Resume()
    {
        if (firstBody != null) firstBody.GetComponent<Body>().StartMoving();
    }

    public override void HandleOutOfBoundBody(GameObject outOfBoundBody)
    {
        if (outOfBoundBody == firstBody)
        {
            isOutOfBound = true;
            Destroy(outOfBoundBody);
            firstBody = null;
        }
    }

    public override void HandleHit(GameObject hitBy)
    {
        if (currentState == TutStates.Talked)
        {
            hasHitByBody = true;
            currentState = TutStates.HitbyBody;

            //Services.player.canEnterBoat = true;
            Services.player.canExitBoat = true;
            Services.player.canMoveIn3D = true;
        }
    }

    private void startCountPaddleTime()
    {
        countingPaddleTime = true;
    }

    private void stopCountPaddleTime()
    {
        countingPaddleTime = false;
    }

    private void checkFirstMove()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            currentState = TutStates.Moved;
            Services.uiManager.SetInCharacterUI("");
        }
    }

    private void startConvo(GameObject target)
    {
        if (target == null) return;

        if (target.tag == "Girl")
        {
            if (!hasFirstTalked)
            {
                //Services.uiManager.SetInCharacterUI("");
                TalkToUI.SetActive(false);
                convo.SendFungusMessage("StartConvo");
                hasFirstTalked = true;
                currentState = TutStates.Talked;

                sendFirstBody();
            }
        }
    }

    private void sendFirstBody()
    {
        firstBody.SetActive(true);
        firstBody.GetComponent<Body>().StartMoving();
        firstBody.GetComponent<Body>().stage = this;
    }

    private void checkFirstHitConvo()
    {
        //if (hasHitByBody)
        if (currentState == TutStates.PushedBodyAway)
        {
            currentState = TutStates.ReEnteredBoat;
            Services.uiManager.SetOutCharacterUI("");
            StartCoroutine(FirstHitConvo());
        }
    }

    private IEnumerator FirstHitConvo()
    {
        yield return new WaitForSeconds(1);

        if (!isOutOfBound)
        {
            convo.SendFungusMessage("OnStage");
        }
        else
        {
            convo.SendFungusMessage("OffStage");
        }
    }    

    private void checkFirstExitBoat()
    {
        if (currentState == TutStates.HitbyBody)
        {
            currentState = TutStates.ExitedBoat;
            Services.uiManager.SetInCharacterUI("");
            Services.uiManager.SetOutCharacterUI("Move the Body");
        }
    }
}
