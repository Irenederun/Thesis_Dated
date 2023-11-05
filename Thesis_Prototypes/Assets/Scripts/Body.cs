using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Vector2 speedRange;
    public float lowerBound;
    public Stage stage;

    private float speed;
    private bool moving;

    private void Start()
    {
        speed = Random.Range(speedRange.x, speedRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (transform.position.y < lowerBound)
        {
            stage.HandleOutOfBoundBody(this.gameObject);
        }
    }

    public void StartMoving()
    {
        moving = true;
    }

    public void StopMoving()
    {
        moving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Boat")
        {
            // hit boat
            stage.HandleHit(this.gameObject);
        }
    }
}
