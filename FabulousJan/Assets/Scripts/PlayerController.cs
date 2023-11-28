using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{

    public GameManager gameManager;

    private SpriteRenderer spriteRenderer;
    //public Sprite[] runSprites;
    //public Sprite climbSprite;
    private int spriteIndex;

    private new Rigidbody2D rigidbody2D;
    private new Collider2D collider;

    private Collider2D[] overlaps = new Collider2D[4];
    private Vector2 direction;

    private bool grounded;
    private bool climbing;
    public bool jump;


    public float moveSpeed = 3f;
    public float jumpStrength = 4f;

    internal Animator animator;


    private bool rightPressed, leftPressed;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        //CancelInvoke();
    }

    private void Update()
    {
        CheckCollision();
        SetDirection();
        AnimatePlayer();
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        //the amount that two colliders can overlap
        //increase this value for steeper platforms

        float skinWidth = 0.1f;

        Vector2 size = collider.bounds.size;
        size.y += skinWidth;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, overlaps);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = overlaps[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {

                // Only set as grounded if the platform is below the player
                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);

                // Turn off collision on platforms the player is not grounded to
                Physics2D.IgnoreCollision(overlaps[i], collider, !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }

        }


    }


    private void SetDirection()
    {

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        } 
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        } 
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Prevent gravity from building up infinitely
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if (direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

    }

    private void FixedUpdate() => rigidbody2D.MovePosition(rigidbody2D.position + direction * Time.fixedDeltaTime);

    //private void AnimateSprite()
    //{
    //    if (climbing)
    //    {
    //        spriteRenderer.sprite = climbSprite;
    //    }
    //    else if (direction.x != 0f)
    //    {
    //        spriteIndex++;

    //        if (spriteIndex >= runSprites.Length) {
    //            spriteIndex = 0;
    //        }

    //        if (spriteIndex > 0 && spriteIndex <= runSprites.Length) {
    //            spriteRenderer.sprite = runSprites[spriteIndex];
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            //gameManager.LevelComplete();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;

            this.collider.enabled = false;
            
            animator.SetTrigger("DeathTrigger");

            gameManager.LevelFailed();
            
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            //grounded = false;
            //this.animator.SetInteger("AnimState", 1);
        }
    }


    private void AnimatePlayer()
    {

        if (direction.x != 0f && grounded)
        {
            animator.SetBool("Ground", true);
        }
        else
        {
            animator.SetBool("Ground", false);
        }

        if ((Input.GetKey(KeyCode.Space) && grounded))
        {
            animator.SetBool("Jump", true);
        }
        else if(grounded)
        {
            animator.SetBool("Jump", false);
        }

        if (climbing)
        {
            animator.SetBool("Climb", climbing);
            animator.SetBool("Jump", false);
        }
        else
        {
            animator.SetBool("Climb", climbing);
        }
    }



    public void RightButton()
    {
        rightPressed = true;

    }

    public void LeftButton()
    {
        leftPressed = true;
    }


}