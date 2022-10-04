using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public float bounceForce = 6;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        audioManager.Play("bounce");
        playerRb.velocity = new Vector3(playerRb.velocity.x, bounceForce, playerRb.velocity.z);
        string materialName = collision.transform.GetComponent<MeshRenderer>().material.name;

        //if the ball hits safe area
        if (materialName == "Safe (Instance)")
        {
        
        }
        //if the ball hits unsafe area game over
        else if (materialName == "Unsafe (Instance)")
        {
            GameManager.gameOver = true;
            audioManager.Play("game over");
        }
        //if the ball hits last ring
        else if (materialName == "Last Ring (Instance)" &&!GameManager.levelCompleted)
        {
            GameManager.levelCompleted = true;
            audioManager.Play("win");
        }
    }
}
