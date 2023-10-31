using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeeingGM : MonoBehaviour
{
    public GameObject inCharacter2D;
    public GameObject outCharacter3D;
    public bool isInCharacter;
    public GameObject player;
    private PlayerController playerCtrl;
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

    // Start is called before the first frame update
    void Start()
    {
        isInCharacter = true;
        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.ChangeStatus(isInCharacter);
        EnterCharacter();
        isInsideTrigger = true;
        boat.GetComponent<BoatMovement>().TrackCharacterStatus(true);
    }

    // Update is called once per frame
    void Update()
    {
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

        if (isInCharacter && listOfBodies.Count > 0)
        {
            BodiesMovement();
        }

        if (!isInCharacter && listOfBodies.Count > 0)
        {
            RemoveBodies();
        }

        if (noMoreSpawning)
        {
            ShoreMovement();
        }
    }

    public void BodiesMovement()
    {
        for (int i = listOfBodies.Count - 1; i >= 0; i--)
        {
            GameObject body = listOfBodies[i];
            bodySpeed = Random.Range(bodySpeedRange.x, bodySpeedRange.y);
            body.GetComponent<Rigidbody>().AddForce(Vector3.left * bodySpeed);
            body.GetComponent<Rigidbody>().velocity *= 0.996f;

            if (body.transform.position.y < lowerBound)
            {
                listOfBodies.Remove(body);
                Destroy(body);
                if (!noMoreSpawning)
                {
                    CreateNewBody();
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
                listOfBodies.Remove(body);
                Destroy(body);
                if (!noMoreSpawning)
                {
                    CreateNewBody();
                }
                playerCtrl.ChangePaddleStatus(true);
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

    public void EnterCharacter()
    {
        inCharacter2D.SetActive(true);
        playerCtrl.backToOriginalPos();
        outCharacter3D.SetActive(false);
    }

    public void ExitCharacter()
    {
        outCharacter3D.SetActive(true);
        inCharacter2D.SetActive(false);
    }

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

    public void GetTheShore()
    {
        shore.SetActive(true);
        noMoreSpawning = true;
    }

    public void ShoreMovement()
    {
        if (playerCtrl.isPaddling)
        {
            //shore.GetComponent<Rigidbody>().AddForce(Vector3.left * shoreSpeed);
            shore.transform.position += Vector3.left * Time.deltaTime;
        }
    }
}
