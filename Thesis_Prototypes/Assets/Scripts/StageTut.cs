using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StageTut : Stage
{
    private Flowchart convo;

    private bool hasTalked;

    private void Update()
    {
        
    }

    public override void Entrance()
    {
        Services.player.EnterCharacter();

        convo = Services.player.GetComponent<Flowchart>();
        Services.player.onTalk += checkStartConvo;
        Services.player.onEnterBoat += checkFirstHitConvo;
        /*
        Services.seeingGM.isInCharacter = true;
        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.ChangeStatus(isInCharacter);
        EnterCharacter();
        isInsideTrigger = true;
        boat.GetComponent<BoatMovement>().TrackCharacterStatus(true);
        */
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    private void checkStartConvo()
    {
        if (!hasTalked)
        {
            convo.SendFungusMessage("StartConvo");
            hasTalked = true;
        }
    }

    private void checkFirstHitConvo()
    {
        if (Services.player.currentState != Player.GameState.tutorial)
        {
            Services.seeingGM.canMoveBodies = true;
            Services.player.currentState = Player.GameState.interval;
            StartCoroutine(FirstHitConvo());
        }
    }

    private IEnumerator FirstHitConvo()
    {
        yield return new WaitForSeconds(1);
        //TODO: deal with bodyIsOnStage
        /*
        if (bodyIsOnStage)
        {
            convo.SendFungusMessage("OnStage");
        }
        else
        {
            convo.SendFungusMessage("OffStage");
        }
        */
    }
}
