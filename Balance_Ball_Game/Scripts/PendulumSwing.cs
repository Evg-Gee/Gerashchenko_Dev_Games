using System;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class PendulumSwing : MonoBehaviour
{
    [SerializeField] private HingeJoint2D hingJoint;
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float swingSpeed = 100f;
    [SerializeField] private float maxAngle = 45f; // Максимальный угол отклонения
    private GameObject attachedCircle;
    private JointMotor2D motor;
    private bool isSwingingRight = true;
    private Rigidbody2D rigidbd;
    public event Action<GameObject> OnCircleReleased; // Событие сброса круга

    private void Awake()
    {
        rigidbd = GetComponent<Rigidbody2D>();
        motor = hingJoint.motor;
        motor.motorSpeed = swingSpeed;
        hingJoint.motor = motor;
    }

    private void FixedUpdate()
    {
        float currentAngle = Mathf.Abs(rigidbd.rotation);
        if (currentAngle >= maxAngle)
        {
            ReverseDirection();
        }
    }

    private void ReverseDirection()
    {
        isSwingingRight = !isSwingingRight;
        motor.motorSpeed = isSwingingRight ? swingSpeed : -swingSpeed;
        hingJoint.motor = motor;
    }

    public void ReleaseCircle()
    {
        if (attachedCircle == null) return;

        CircleBall circleBehavior = attachedCircle.GetComponent<CircleBall>();
        circleBehavior.SetFallingState();
        attachedCircle.transform.SetParent(null);
        OnCircleReleased?.Invoke(attachedCircle);
        attachedCircle = null;
    }
   
    public void AttachCircle(GameObject circle)
    {
        CircleBall circleBehavior = circle.GetComponent<CircleBall>();
        if (circleBehavior == null)
        {
            circleBehavior = circle.AddComponent<CircleBall>();
        }
        attachedCircle = circle;
        circleBehavior.SetAttachedState(); 
        attachedCircle.transform.position = ballTransform.position;
        attachedCircle.transform.SetParent(transform);
    }
    public bool HasCircle() => attachedCircle != null;
    
}
