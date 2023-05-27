using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _jumpStrength = 1.0f;
    private Collider2D _collider;
    private Collider2D[] _results;
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _grounded;
    private bool _climbing;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _results = new Collider2D[4];
    }

    public void CheckCollision()
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

                Physics2D.IgnoreCollision(_collider, _results[i], !_grounded);
            } 
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                _climbing = true;
            }
        }

    } 

    public void Update()
    {
        CheckCollision();

        _direction.x = Input.GetAxis("Horizontal") * _moveSpeed;


        if(_grounded)
        {
            _direction.y = Mathf.Max(_direction.y, -1f);
        }

        if(_direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        } 
        else if(_direction.x < 0f) 
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }


        if(_climbing)
        {
            _direction.y = Input.GetAxis("Vertical") * _moveSpeed;
        }
        else if(_grounded && Input.GetButtonDown("Jump"))
        {
            _direction = Vector2.up * _jumpStrength;
        } 
        else
        {
            _direction += Physics2D.gravity * Time.deltaTime;
        }

    }

    public void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _direction * Time.fixedDeltaTime);
    }

}
