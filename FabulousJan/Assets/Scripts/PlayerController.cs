using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{


    public enum InputController { NONE, PC, Mobile };

    public InputController inputType;

    public GameManager gameManager;

    //private SpriteRenderer spriteRenderer;
    //public Sprite[] runSprites;
    //public Sprite climbSprite;
    //private int spriteIndex;

    private Rigidbody2D _rb;
    private new Collider2D collider;

    private Collider2D[] overlaps = new Collider2D[4];
    private Vector2 direction;

    public bool isGround = true;
    private bool isClimb;
    public bool isJump;


    public float moveSpeed = 3f;
    public float jumpStrength = 4f;

    internal Animator animator;

    //get and set horizontalValue if pressed button
    public float HorizontalValue { get { return horizontalValue; } set { horizontalValue = value; } }
    //get and set verticalValue if pressed button
    public float VerticalValue { get { return verticalValue; } set { verticalValue = value; } }

    float horizontalValue; 
    float verticalValue;

    public AudioClip JumpAudio, WalkAudio;


    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
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

        //Debug.Log(horizontalValue);

        if (inputType == InputController.PC) 
        {
            PCHandle();
        }
        else if (inputType == InputController.Mobile)
        {
            //SetDirection();
            MobileHandle();
            
        }

        SetDirection();
        CheckCollision();
        AnimatePlayer();
    }

    private void FixedUpdate() => Move(horizontalValue);

    //private void FixedUpdate() => rigidbody2D.MovePosition(rigidbody2D.position + direction * Time.fixedDeltaTime);

    private void CheckCollision()
    {
        
        isGround = false;
        isClimb = false;

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
                isGround = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);

                // Turn off collision on platforms the player is not grounded to
                Physics2D.IgnoreCollision(overlaps[i], collider, !isGround);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                isClimb = true;
            }
        }

        //animator.SetBool("Jump", !grounded);
    }

    private void PCHandle()
    {
        horizontalValue = Input.GetAxis("Horizontal");
        verticalValue = Input.GetAxis("Vertical");
        //SoundManager.instance.PlaySoundFX(JumpAudio, 0.5f);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    private void MobileHandle()
    {
        //Debug.Log(horizontalValue);
    }

    

    private void SetDirection()
    {

        if (isClimb && verticalValue > 0)
        {
            direction.y = verticalValue * moveSpeed;
        } 
        else
        {
            // physics player to down
            direction += Physics2D.gravity * Time.deltaTime;
        }

        // Prevent gravity from building up infinitely
        if (isGround)
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


    #region Jump
    public void Jump()
    {
        isJump = true;

        if (isGround && isJump)
        {
            direction = Vector2.up * jumpStrength;
            SoundManager.instance.PlaySoundFX(JumpAudio, 0.5f);
            isJump = false;
        }
    }
    #endregion

    void Move(float horizontalVal)
    {
        //SoundManager.instance.PlaySoundFX(WalkAudio, 0.5f);

        direction.x = horizontalVal * moveSpeed;
        
        _rb.MovePosition(_rb.position + direction * Time.fixedDeltaTime);

        float xVal = horizontalVal * moveSpeed * 100 * Time.fixedDeltaTime;

        Vector2 targetVelocity = new Vector2(xVal, _rb.velocity.y);
        _rb.velocity = targetVelocity;


        animator.SetFloat("directionX", Mathf.Abs(_rb.velocity.x));

    }

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

    private void AnimatePlayer()
    {

        if (!isGround && !isClimb)
        {
            animator.SetBool("Jump", true);
        }
        else 
        {
            animator.SetBool("Climb", true);
            animator.SetBool("Jump", false);
        }

        if (verticalValue > 0 && isClimb)
        {
            animator.SetBool("Climb", true);
        }
        else
        {
            animator.SetBool("Climb", false);
        }
    }



}