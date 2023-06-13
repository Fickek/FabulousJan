using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Player.Mechanics
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 1.0f;
        [SerializeField] private float _jumpStrength = 1.0f;

        [SerializeField] private Sprite[] _runSprites;
        [SerializeField] private Sprite[] _spriteClimb;
        private SpriteRenderer _spriteRenderer;
        private int _spriteRunIndex;
        private int _spriteClimbIndex;

        //private GameManager _gameManager;

        private Collider2D _collider;
        private Collider2D[] _results = new Collider2D[4];
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private bool _grounded;
        private bool _climbing;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            //_gameManager = GetComponent<GameManager>();
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(AnimateSprite), 1f / 12f, 1f / 12f);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void Update()
        {
            CheckCollision();
            SetDirection();
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + _direction * Time.fixedDeltaTime);
        }

        private void AnimateSprite()
        {
            if (_climbing && _direction.y != 0f)
            {
                _spriteClimbIndex++;
                if (_spriteClimbIndex >= _spriteClimb.Length)
                {
                    _spriteClimbIndex = 0;
                }
                _spriteRenderer.sprite = _spriteClimb[_spriteClimbIndex];
            }
            else if (_grounded && _direction.x != 0f)
            {
                _spriteRunIndex++;
                if (_spriteRunIndex >= _runSprites.Length)
                {
                    _spriteRunIndex = 0;
                }
                _spriteRenderer.sprite = _runSprites[_spriteRunIndex];
            }

        }

        #region CheckCollision
        private void CheckCollision()
        {
            _climbing = false;
            _grounded = false;

            Vector2 size = _collider.bounds.size;
            size.y += 0.1f;
            size.x /= 2f;

            int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, _results);

            for (int i = 0; i < amount; i++)
            {
                GameObject hit = _results[i].gameObject;

                if (hit.layer == LayerMask.NameToLayer("Ground"))
                {
                    _grounded = hit.transform.position.y < (transform.position.y - 0.5f);
                    Physics2D.IgnoreCollision(_results[i], _collider, !_grounded);
                }
                else if (hit.layer == LayerMask.NameToLayer("Ladder"))
                {
                    _climbing = true;
                }
            }
        }
        #endregion

        #region SetDirection
        private void SetDirection()
        {
            _direction.x = Input.GetAxis("Horizontal") * _moveSpeed;

            if (_grounded)
            {
                _direction.y = Mathf.Max(_direction.y, -1f);
            }

            if (_direction.x > 0f)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else if (_direction.x < 0f)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }


            if (_climbing)
            {
                _direction.y = Input.GetAxis("Vertical") * _moveSpeed;
            }
            else if (_grounded && Input.GetButtonDown("Jump"))
            {
                _direction = Vector2.up * _jumpStrength;
            }
            else
            {
                _direction += Physics2D.gravity * Time.deltaTime;
            }
        } 
        #endregion

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Objective"))
            {
                enabled = false;
                FindObjectOfType<GameManager>().LevelComplete();
            }

            if (collision.gameObject.CompareTag("Obstacle"))
            {
                enabled = false;
                FindObjectOfType<GameManager>().LevelFailed();
            }
        }
    }
}
