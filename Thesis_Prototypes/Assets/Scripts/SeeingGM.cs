using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeeingGM : MonoBehaviour
{
    //public GameObject inCharacter2D;
    //public GameObject outCharacter3D;
    //public bool isInCharacter;
    //public GameObject player;
    //private PlayerController playerCtrl;
    /*
    private bool isInsideTrigger;
    public List<GameObject> listOfBodies;
    public float bodySpeed;
    public float lowerBound;
    public Vector2 bodySpawnRangeX;
    public Vector2 bodySpawnRangeZ;
    public Vector2 bodySpeedRange;
    public GameObject bodyPrefab;
    public GameObject boat;
    public bool noMoreSpawning;
    public GameObject shore;
    public float shoreSpeed;
    */

    /*
    public bool canMoveBodies = false;
    public bool bodyIsOnStage = false;
    */
    [Space(10f)]
    public Flowchart convo;

    [Space(10f)]
    public Stage currentStage;
    public List<Stage> stages;

    private void Awake()
    {
        if (Services.seeingGM == null) Services.seeingGM = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStage.Entrance();

        /*
        isInCharacter = true;
        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.ChangeStatus(isInCharacter);
        EnterCharacter();
        isInsideTrigger = true;
        boat.GetComponent<BoatMovement>().TrackCharacterStatus(true);
        convo = player.GetComponent<Flowchart>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isInsideTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isInCharacter)
                {
                    ExitCharacter();
                    boat.GetComponent<BoatMovement>().TrackCharacterStatus(false);
                }
                else
                {
                    EnterCharacter();
                    boat.GetComponent<BoatMovement>().TrackCharacterStatus(true);
                }
                isInCharacter = !isInCharacter;
                playerCtrl.ChangeStatus(isInCharacter);
            }
        }
        */

        //if (isInCharacter && listOfBodies.Count > 0)
        //{
        //    BodiesMovement();
        //}

        /*
        if (canMoveBodies)
        {
            BodiesMovement();
        }
        */

        /*
        if (!isInCharacter && listOfBodies.Count > 0)
        {
            RemoveBodies();
        }
        */
        /*
        if (noMoreSpawning)
        {
            ShoreMovement();
        }
        */
    }

    /*
    public void BodiesMovement()
    {
        for (int i = listOfBodies.Count - 1; i >= 0; i--)
        {
            GameObject body = listOfBodies[i];
            bodySpeed = Random.Range(bodySpeedRange.x, bodySpeedRange.y);
            //body.GetComponent<Rigidbody>().AddForce(Vector3.left * bodySpeed);
            //body.GetComponent<Rigidbody>().velocity *= 0.996f;

            body.transform.position += Vector3.left * bodySpeed * Time.deltaTime;
            
            if (body.transform.position.y < lowerBound)
            {
                listOfBodies.Remove(body);
                bodyIsOnStage = false;
                Destroy(body);
                if (!noMoreSpawning)
                {
                    //CreateNewBody();
                    Debug.Log("Time For New");
                }
            }
        }
    }

    public void RemoveBodies()
    {
        for (int i = listOfBodies.Count - 1; i >= 0; i--)
        {
            GameObject body = listOfBodies[i];
            if (body.transform.position.y < lowerBound)
            {
                bodyIsOnStage = false;
                listOfBodies.Remove(body);
                Destroy(body);
                if (!noMoreSpawning)
                {
                    //CreateNewBody();
                    print("time for new");
                }
                Services.player.ChangePaddleStatus(true);
            }
        }
    }

    private void CreateNewBody()
    {
        float bodyXPos = Random.Range(bodySpawnRangeX.x, bodySpawnRangeX.y);
        float bodyZPos = Random.Range(bodySpawnRangeZ.x, bodySpawnRangeZ.y);
        GameObject newBody = Instantiate(bodyPrefab, new Vector3(bodyXPos, -3f, bodyZPos), 
            quaternion.Euler(0,0,90));
        listOfBodies.Add(newBody);
    }
    */

    /*
    public void EnterCharacter()
    {
        inCharacter2D.SetActive(true);
        Services.player.backToOriginalPos();
        outCharacter3D.SetActive(false);

        if (Services.player.currentState != Player.GameState.tutorial)
        {
            canMoveBodies = true;
            Services.player.currentState = Player.GameState.interval;
            StartCoroutine(FirstHitConvo());
        }
    }
    */

    /*
    private IEnumerator FirstHitConvo()
    {
        yield return new WaitForSeconds(1);
        if (bodyIsOnStage)
        {
            convo.SendFungusMessage("OnStage");
        }
        else
        {
            convo.SendFungusMessage("OffStage");
        }
    }
    */
    
    /*
    public void ExitCharacter()
    {
        outCharacter3D.SetActive(true);
        inCharacter2D.SetActive(false);
    }
    */

        /*
    public void CanChangeStatus(string yn)
    {
        switch (yn)
        {
            case "yes":
                if (!isInsideTrigger)
                {
                    isInsideTrigger = true;
                }
                break;
            case "no":
                isInsideTrigger = false;
                break;
        }
    }
    */

        /*
    public void GetTheShore()
    {
        shore.SetActive(true);
        noMoreSpawning = true;
    }

    public void ShoreMovement()
    {
        if (Services.player.isPaddling)
        {
            //shore.GetComponent<Rigidbody>().AddForce(Vector3.left * shoreSpeed);
            shore.transform.position += Vector3.left * Time.deltaTime;
        }
    }
    */

    /*
    public void FirstWaveOfBody()
    {
        foreach (GameObject body in listOfBodies)
        {
            if (!body.activeSelf)
            {
                body.SetActive(true);
                bodyIsOnStage = true;
                canMoveBodies = true;
            }
        }
    }
    */

    /*
    public void CanMoveBody()
    {
        canMoveBodies = false;
    }
    */
}
