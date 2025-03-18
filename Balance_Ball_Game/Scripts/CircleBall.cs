using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CircleBall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _circleCollider;
    public bool isHandled = false;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }
    
   public void SetAttachedState()
    {
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
        }
        
        _circleCollider.enabled = false;
    }
    public void SetFallingState()
    {
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }
        _rigidbody.isKinematic = false;
        _rigidbody.gravityScale = 1f;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        _circleCollider.enabled = true; 
    }
    public void SetStaticState()
    {
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
        }
        
         isHandled = true; 
        _circleCollider.enabled = false; 
        }
    }
