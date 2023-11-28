using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    private Rigidbody2D rb;
    float moveSpeed = 5f;

    bool leftButton = false;
    bool rightButton = false;

    float horizontalMove = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Run();
        leftButton = false;
        rightButton = false;
    }

    void Run()
    {

        //float controlFlow = Input.GetAxis("Horizontal");

        //Vector2 playerVelocity = new Vector2(controlFlow, rb.velocity.y);

        if (leftButton) 
        {
            horizontalMove = moveSpeed;
        }
        else if(rightButton) 
        {
            horizontalMove = -moveSpeed;
        }
        else 
        {
            horizontalMove = 0;
        }

        // = new Vector2(horizontalMove, rb.velocity.y);
        //rb.velocity = playerVelocity * moveSpeed;
        
    }

    private void FixedUpdate() => rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

    public void leftButtonPress() => leftButton = true;
    public void rightButtonPress() => rightButton = true;


}
