using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static UnityEngine.AudioSettings;

public class PlayerController : MonoBehaviour
{
    public enum InputController { NONE, PC, Mobile };

    [SerializeField] private InputController _inputType;

    [SerializeField] private AudioSource _audioWalkSource;
    [SerializeField] private GameObject pokeballPrefab;

    private SpriteRenderer _spriteRenderer;


    private Rigidbody2D _rb;
    private new Collider2D collider;

    private Collider2D[] overlaps = new Collider2D[4];
    
    private List<Collider2D> overlapsList = new List<Collider2D>();
    
    private Vector2 direction;

    [SerializeField] private bool _isGround = true;
    private bool _isClimb;
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isDeath = false;
    [SerializeField] private bool _isSpawn;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpStrength = 4f;

    internal Animator animator;

    //get and set horizontalValue if pressed button
    public float HorizontalValue { get { return _horizontalValue; } set { _horizontalValue = value; } }
    //get and set verticalValue if pressed button
    public float VerticalValue { get { return _verticalValue; } set { _verticalValue = value; } }

    private float _horizontalValue; 
    private float _verticalValue;

    [SerializeField] private AudioClip _JumpAudio, _WalkAudio, _DeathAudio;

    [SerializeField] private float _waitAfterDeath;
    [SerializeField] private float _waitImmortalityTime;

    private Vector3 startPos;
    private Quaternion startRot;


    [SerializeField] private GameObject _blink;

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
        startRot = transform.rotation;
        Debug.Log("Spawn Player");
        Spawn();
    }

    private void Update()
    {

        if (_isDeath)
        {
            return;
        }
        else 
        {
            if (_inputType == InputController.PC)
            {
                PCHandle();
            }
            else if (_inputType == InputController.Mobile)
            {
                MobileHandle();
            }

            SetDirection();
            CheckCollision();
            AnimatePlayer();
        }
    }

    private void FixedUpdate() => Move(_horizontalValue);

    private void CheckCollision()
    {
        
        _isGround = false;
        _isClimb = false;

        //the amount that two colliders can overlap
        //increase this value for steeper platforms
        float skinWidth = 0.1f;

        Vector2 size = collider.bounds.size;
        size.y += skinWidth;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, overlaps);
        
        print($"Warning: {amount}");
        
        //int amount1 = Physics2D.OverlapBox(transform.position, size, amount, new ContactFilter2D(), overlapsList);
        

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = overlaps[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {

                // Only set as grounded if the platform is below the player
                _isGround = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);

                // Turn off collision on platforms the player is not grounded to
                Physics2D.IgnoreCollision(overlaps[i], collider, !_isGround);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                _isClimb = true;
            }
        }

    }

    private void PCHandle()
    {
        //isMobile = false;

        _horizontalValue = Input.GetAxis("Horizontal");
        _verticalValue = Input.GetAxis("Vertical");

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

        if (_isClimb && _verticalValue > 0)
        {
            direction.y = _verticalValue * _moveSpeed;
        } 
        else
        {
            // physics on player
            direction += Physics2D.gravity * Time.deltaTime;
        }

        // Prevent gravity from building up infinitely
        if (_isGround)
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
        _isJump = true;

        if (_isGround && _isJump)
        {
            direction = Vector2.up * _jumpStrength;
            SoundManager.Instance.PlaySoundFX(_JumpAudio, 0.5f);
            _isJump = false;
        }
    }
    #endregion

    void Move(float horizontalVal)
    {

        direction.x = horizontalVal * _moveSpeed;
        
        _rb.MovePosition(_rb.position + direction * Time.fixedDeltaTime);

        float xVal = horizontalVal * _moveSpeed * 100 * Time.fixedDeltaTime;

        Vector2 targetVelocity = new Vector2(xVal, _rb.linearVelocity.y);

        _rb.linearVelocity = targetVelocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
        }
        else if(_isSpawn && collision.gameObject.CompareTag("Obstacle"))
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


            SoundManager.Instance.PlaySoundFX(_DeathAudio, 1f);

            enabled = false;

            this.collider.enabled = false;

            _rb.linearVelocity = deathKick;

            _isDeath = true;

            StartCoroutine(CoroutineDeath(_waitAfterDeath));

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
        transform.position = startPos;
        transform.rotation = startRot;
        this.collider.enabled = true;
        _isDeath = false;
        enabled = true;
        _isSpawn = true;
        StartCoroutine(CoroutineColorFliker());
        StartCoroutine(CoroutineSpawn(_waitImmortalityTime));
    }

    private IEnumerator CoroutineSpawn(float wait)
    {
        //Debug.Log($"Wait {wait} sec");
        yield return new WaitForSeconds(wait); //delay after spawn
        _isSpawn = false;
    }

    private IEnumerator CoroutineColorFliker()
    {
        while (_isSpawn)
        {
            Debug.Log("Start CoroutineColorFliker");
            _spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(.2f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.2f);
        }
    }

    private void AnimatePlayer()
    {


        if (_isSpawn)
        {
            animator.SetBool("Spawn", true);
        }
        else
        {
            animator.SetBool("Spawn", false);
        }

        if (!_isGround && !_isClimb)
        {
            animator.SetBool("Jump", true);
        }
        else 
        {
            animator.SetBool("Climb", true);
            animator.SetBool("Jump", false);
        }

        if (_verticalValue > 0 && _isClimb)
        {
            animator.SetBool("Climb", true);
        }
        else
        {
            animator.SetBool("Climb", false);
        }

        if(_isDeath) 
        {
            animator.SetBool("Death", true);
        }
        else 
        {
            animator.SetBool("Death", false);
            animator.SetFloat("directionX", Mathf.Abs(_rb.linearVelocity.x));
        }

    }
}