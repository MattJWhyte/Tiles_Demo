using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 30.0f;
    public float jumpForce = 10.0f;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector3 movement = new Vector3(moveHorizontal,0, moveVertical);
        Vector3 move = cam.transform.forward;

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        //rb.AddForce(movement * speed);
        rb.AddForce(move * moveVertical*speed);
        Debug.Log(moveVertical);
        if ((Input.GetKeyDown(KeyCode.JoystickButton15)))
        {
            rb.AddForce(new Vector3(0, 1, 0) * jumpForce);
        }
        }
}
