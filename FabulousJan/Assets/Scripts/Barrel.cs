using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Rigidbody2D _rigidbody;
    private float _undergeound = -8.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckUnderground();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _rigidbody.AddForce(collision.transform.right * _speed, ForceMode2D.Impulse);
        }
    }

    private void CheckUnderground()
    {
        if(gameObject.transform.position.y < _undergeound)
        {
            Destroy(gameObject);
        }
    }
}
