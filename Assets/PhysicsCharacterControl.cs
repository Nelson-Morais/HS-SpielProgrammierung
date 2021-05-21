using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCharacterControl : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Animator animator;

    public float acceleration = 1.0f;
    public float maxSpeed = 10.0f;
    public float jumpForce = 1.0f;
    public float jumpCooldown = 1.0f;
    public float knockCooldown = 1.0f;

    private float currJumpCooldown = 0.0f;
    public float currKnockCooldown = 0.0f;


    private int floorTouchCount;

    private int knockTouchCount;


    private bool isGrounded
    {
        get
        {
            return floorTouchCount > 0;
        }
    }

    private bool _hasControl;


    private bool hasControl
    {
        get => _hasControl;
        set
        {
            if (_hasControl != value)
            {
                _hasControl = value;
                if (value)
                {
                    rigidbody.rotation = initialRotation;
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }
                else
                {
                    rigidbody.constraints = RigidbodyConstraints.None;
                }
            }
        }
    }

    private Vector3 initialPosition;
    private Quaternion initialRotation;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        initialPosition = rigidbody.position;
        initialRotation = rigidbody.rotation;
        hasControl = true;
    }

    private void FixedUpdate()
    {
        UpdateInput();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.speed = Mathf.Max(1, rigidbody.velocity.magnitude * 0.1f);
        animator.SetFloat("Speed", rigidbody.velocity.magnitude);
        animator.SetBool("Grounded", isGrounded);
    }

    private void UpdateInput()
    {
        if (isGrounded && knockTouchCount <= 0)
        {
            currKnockCooldown -= Time.fixedDeltaTime;
        }
        hasControl = currKnockCooldown < 0.0f;

        if (hasControl)
        {
            Vector3 input = Camera.main.transform.forward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
            input.y = 0;
            input.Normalize();
            rigidbody.AddForce(input * acceleration, ForceMode.Acceleration);
            Vector3 lookDir = rigidbody.velocity;
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 1)
            {
                rigidbody.MoveRotation(Quaternion.LookRotation(lookDir, Vector3.up));
            }
            bool jump = Input.GetButton("Jump");
            currJumpCooldown -= Time.fixedDeltaTime;
            if (isGrounded && jump && currJumpCooldown <= 0.0f)
            {
                currJumpCooldown = jumpCooldown;
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        floorTouchCount++;
        if (collision.collider.CompareTag("Knock"))
        {
            Debug.Log("Knock");
            knockTouchCount++;
            currKnockCooldown = knockCooldown;
        }
        TipToePlatform tipToePlattform = collision.gameObject.GetComponent<TipToePlatform>();
        if (tipToePlattform != null)
        {
            tipToePlattform.CharacterTouches();
            if (!tipToePlattform.isPath)
            {
                floorTouchCount--;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        floorTouchCount--;
        if (collision.collider.CompareTag("Knock"))
        {
            Debug.Log("Knock");
            knockTouchCount--;
            currKnockCooldown = knockCooldown;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.CompareTag("ResetChar");
        ResetChar();
    }

    private void ResetChar()
    {
        rigidbody.position = initialPosition;
        rigidbody.rotation = initialRotation;
        rigidbody.velocity = Vector3.zero;
        currKnockCooldown = 0;
    }


}