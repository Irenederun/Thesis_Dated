using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StageTut : Stage
{
    // sequence flags
    [Header("sequence flags")]
    public bool hasPaddled;
    public bool hasFirstTalked;
    public bool hasHitByBody;

    // body related
    [Header("bodyyyy")]
    public GameObject firstBody;

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
                Services.player.canMoveIn2D = true;
                Services.player.canTalk = true;
            }
        }

    }

    public override void Entrance()
    {
        convo = Services.player.GetComponent<Flowchart>();

        // subscribe to events
        Services.player.onStartPaddle += startCountPaddleTime;
        Services.player.onStopPaddle += stopCountPaddleTime;
        Services.player.onTalk += startConvo;
        Services.player.onEnterBoat += checkFirstHitConvo;

        // initial setting
        Services.player.canPaddle = true;
        Services.player.canMoveIn2D = false;
        Services.player.canTalk = false;

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
        if (hitBy == firstBody)
        {
            hasHitByBody = true;

            Services.player.canEnterBoat = true;
            Services.player.canExitBoat = true;
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

    private void startConvo(GameObject target)
    {
        if (target.tag == "Girl")
        {
            if (!hasFirstTalked)
            {
                convo.SendFungusMessage("StartConvo");
                hasFirstTalked = true;

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
        if (hasHitByBody)
        {
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
}
