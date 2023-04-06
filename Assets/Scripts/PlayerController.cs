using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Player movement speed
    [SerializeField] int speed;

    //Player jump height
    [SerializeField] int jumpHeight;

    //Player dash speed
    [SerializeField] int dashSpeed = 20;

    //Score tracker
    [SerializeField] float score;

    //Player spring height
    [SerializeField] int springHeight = 30;

    //Audio Player for background and other audio sources
    [SerializeField] AudioSource audioplayer;

    //Checks if player is connected with the ground
    private bool isGrounded = false;

    //Player rigidbody
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //Move Right
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //Move Left
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            //Jump
            //dojump = true;
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.right * Time.deltaTime * dashSpeed);           
        }


    }


    private void FixedUpdate()
    {
        //Edited jumping code.
        //Needs reviewing. It breaks the jump every time.
        //Unsure how to fix currently.
        
        //if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        //}

    }


    //If the player collides with the ground after jumping, they are allowed to jump again
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    //If the player is in the air, the player will not be able to jump again until they collide with the ground
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }



    private void OnTriggerEnter(Collider collision)
    {
        // If the player collides or falls into water
        //The water droplet sound will player
        //And the player dies and the game resets
        if (collision.tag == "Water")
        {
            audioplayer.Play();
            //need to add a delay to game over so you can hear sound effect before reset
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Game Over");
        }

        //If the player collides/collects a coin
        //their score will be increased
        //and the coin game Object will disappear from the scene
        if (collision.gameObject.CompareTag("Coin"))
        {
            score++;
            collision.gameObject.SetActive(false);
            Debug.Log($"Score: {score}");
            Instantiate(collision.gameObject);
        }

        //Collision with spring forces player into the air
        if (collision.gameObject.CompareTag("Spring"))
        {
            rb.AddForce(Vector3.up * springHeight, ForceMode.Impulse);
        }
    }
}


