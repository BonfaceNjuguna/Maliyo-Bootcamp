using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 15;

    private bool isMoving;
    private Vector3 travelDirection;

    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 500;

    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private Color solveColor;

    private AudioSource playerAudio;
    public AudioClip swipeSound;


    private void Start()
    {
        solveColor = Random.ColorHSV(.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
        playerAudio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = speed * travelDirection;
        }

        //paint the ground
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), .05f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GroundPieceController ground = hitColliders[i].transform.GetComponent<GroundPieceController>();

            if (ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }

            i++;
        }

        //check if reached destination
        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1 )
            {
                isMoving = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isMoving)
            return;

        //swipe section
        if (Input.GetMouseButton(0))
        {
            //get the current mouse postion
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //calculate swipe
            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    playerAudio.Stop();
                    return;
                }

                currentSwipe.Normalize(); //get direction

                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                {
                    //go up or down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5)
                {
                    //go left or right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }
                playerAudio.PlayOneShot(swipeSound, 0.5f);
            }

            swipePosLastFrame = swipePosCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
            playerAudio.Stop();
        }
    }
    

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point; //store nextcollision on hit point
        }

        isMoving = true;
    }

}
