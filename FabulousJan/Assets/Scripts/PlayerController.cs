using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static UnityEngine.AudioSettings;

public class PlayerController : MonoBehaviour
{
    public enum InputController { NONE, PC, Mobile };

    public InputController inputType;

    //public static bool isMobile = false;

    public AudioSource _audioWalkSource;

    public GameObject pokeballPrefab;
    //public Vector2 deathKick = new Vector2(250f, 250f);

    //public GameManager gameManager;


    //public Sprite[] runSprites;
    //public Sprite climbSprite;
    //private int spriteIndex;


    private SpriteRenderer _spriteRenderer;


    private Rigidbody2D _rb;
    private new Collider2D collider;

    private Collider2D[] overlaps = new Collider2D[4];
    private Vector2 direction;

    public bool isGround = true;
    private bool isClimb;
    public bool isJump;
    public bool isDeath = false;

    public bool isSpawn;


    public float moveSpeed = 3f;
    public float jumpStrength = 4f;

    internal Animator animator;

    //get and set horizontalValue if pressed button
    public float HorizontalValue { get { return horizontalValue; } set { horizontalValue = value; } }
    //get and set verticalValue if pressed button
    public float VerticalValue { get { return verticalValue; } set { verticalValue = value; } }

    float horizontalValue; 
    float verticalValue;

    public AudioClip JumpAudio, WalkAudio, DeathAudio;

    public float waitAfterDeath;
    public float waitImmortalityTime;


    private Vector3 startPos;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {     
        startPos = transform.position;
        Debug.Log("Spawn Player");
        isSpawn = true;
    }

    private void Update()
    {

        if (isDeath)
        {
            return;
        }
        else 
        {
            if (inputType == InputController.PC)
            {
                PCHandle();
            }
            else if (inputType == InputController.Mobile)
            {
                MobileHandle();
            }

            SetDirection();
            CheckCollision();
            AnimatePlayer();
        }


        //if (Mathf.Abs(horizontalValue) > 0f)
        //{
        //    _audioWalkSource.Play();   
        //}

        if(isSpawn) 
        {
            StartCoroutine(CoroutineSpawn(waitImmortalityTime));
        }

    }

    private IEnumerator CoroutineSpawn(float wait)
    {
        //Debug.Log($"Wait {wait} sec");

        //int counter = 0;
        _spriteRenderer.color = Color.green;
        //while(counter <= wait) 
        //{
        //    Debug.Log(counter);
        //    _spriteRenderer.color = Color.green;
        //    counter++;
        //}
        //_spriteRenderer.color = new Color(1, 1, 1);

        yield return new WaitForSeconds(wait); //delay after spawn

        _spriteRenderer.color = new Color(1, 1, 1);

        //Debug.Log("Coroutine end");
        //Debug.Log(isSpawn);
        isSpawn = false;
    }

    private void FixedUpdate() => Move(horizontalValue);

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

    }

    private void PCHandle()
    {
        //isMobile = false;

        horizontalValue = Input.GetAxis("Horizontal");
        verticalValue = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    private void MobileHandle()
    {
        //isMobile = true;

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
            // physics on player
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
            SoundManager.Instance.PlaySoundFX(JumpAudio, 0.5f);
            isJump = false;
        }
    }
    #endregion

    void Move(float horizontalVal)
    {


        direction.x = horizontalVal * moveSpeed;
        
        _rb.MovePosition(_rb.position + direction * Time.fixedDeltaTime);

        float xVal = horizontalVal * moveSpeed * 100 * Time.fixedDeltaTime;

        Vector2 targetVelocity = new Vector2(xVal, _rb.velocity.y);

        _rb.velocity = targetVelocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
        }
        else if(isSpawn && collision.gameObject.CompareTag("Obstacle"))
        {

            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());

        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            animator.SetBool("Death", true);

            float randomKickX = Random.Range(-7, 7);
            float randomKickY = Random.Range(5, 8);

            Debug.Log($"randomKickX: {randomKickX}");
            Debug.Log($"randomKickY: {randomKickY}");

            Vector2 deathKick = new Vector2(randomKickX, randomKickY);


            SoundManager.Instance.PlaySoundFX(DeathAudio, 1f);

            enabled = false;

            this.collider.enabled = false;

            _rb.velocity = deathKick;

            isDeath = true;

            StartCoroutine(CoroutineDeath(waitAfterDeath));

        }
    }

    private IEnumerator CoroutineDeath(float wait)
    {
        Debug.Log($"Wait {wait} sec");
        yield return new WaitForSeconds(wait);
        Spawn();
    }

    void Spawn() 
    {
        //animator.SetBool("Death", false);
        transform.position = startPos;
        this.collider.enabled = true;
        isDeath = false;
        enabled = true;
        isSpawn = true;
    }


    private void AnimatePlayer()
    {


        if (isSpawn)
        {
            animator.SetBool("Spawn", true);
        }
        else
        {
            animator.SetBool("Spawn", false);
        }

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

        if(isDeath) 
        {
            animator.SetBool("Death", true);
        }
        else 
        {
            animator.SetBool("Death", false);
            animator.SetFloat("directionX", Mathf.Abs(_rb.velocity.x));
        }

    }
}