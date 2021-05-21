using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterControl : MonoBehaviour
{
    Rigidbody rigidbody;
    
    [Range(0,10)]
    public float speed = 5f;

    [Range(0, 10)]
    public float fallSpeed = 5f;

    private Animator animator;

    private Vector3 startPos;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 m_Input = Camera.main.transform.forward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
        m_Input.y = 0;
        m_Input.Normalize();
        rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * speed);
        if (m_Input.sqrMagnitude > 10e-10)
        {
            rigidbody.MoveRotation(Quaternion.LookRotation(m_Input));
        }

        animator.SetFloat("Speed", rigidbody.velocity.magnitude / speed);
        RaycastHit hit;
        if (!rigidbody.SweepTest(Vector3.down, out hit))
        {
            
            animator.SetBool("Grounded", false);
            rigidbody.MovePosition(transform.position + Vector3.down * Time.deltaTime * fallSpeed);
        }
        else
        {
            TipToePlatform tipToePlatform = hit.collider.GetComponent<TipToePlatform>();
            if (tipToePlatform != null)
            {
                tipToePlatform.CharacterTouches();
            }
            animator.SetBool("Grounded", true);
        }

        if(transform.position.y < -10)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = startPos;
    }
}
