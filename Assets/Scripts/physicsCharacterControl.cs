using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCharacterControl : MonoBehaviour
{
    [SerializeField] private float char_speed;
    [SerializeField] private float jumpForce;
    private Animator char_animator;
    private Rigidbody char_rigidbody;
    private Camera cam;
    private Vector3 input;
    private Vector3 sphereOffset = new Vector3(0, 1.1f, 0);
    private bool isGrounded;
    private bool isJump;
    private bool isCollidet;
    float time = 3;


    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        char_speed = 5.0f;
        char_rigidbody = GetComponent<Rigidbody>();
        char_animator = GetComponent<Animator>();
        cam = Camera.main;
        input = new Vector3(0, 0, 0);
        jumpForce = 4.0f;
        isCollidet = false;
    }

    //called in Sync with Physics engine
    private void FixedUpdate()
    {

        if (Physics.SphereCast(transform.position + sphereOffset, 0.5f, Vector3.down, out RaycastHit raycasthit, 2.0f))
        {
            isGrounded = true;
            char_animator.SetBool("Grounded", true);
            TipToePlatform platform = raycasthit.collider.gameObject.GetComponent<TipToePlatform>();
            if (platform != null)
            {
                platform.CharacterTouches();
            }
            GoalPlatform goalPlatform = raycasthit.collider.gameObject.GetComponent<GoalPlatform>();
            if (goalPlatform != null)
            {
                resetChar();
            }
            input = cam.transform.forward * Input.GetAxis("Vertical");
            input += cam.transform.right * Input.GetAxis("Horizontal");
            input.y = 0;
            if (input != Vector3.zero) transform.rotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.z));

        }
        else
        {
            char_animator.SetBool("Grounded", false);
            isGrounded = false;
        }

        if (isJump && isGrounded)
        {
            char_animator.SetBool("Grounded", false);
            char_rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJump = false;
        }

        //char_rigidbody.MovePosition(transform.position + input.normalized * Time.deltaTime * char_speed);
        input.Normalize();
        char_rigidbody.velocity = new Vector3(input.x * char_speed, char_rigidbody.velocity.y, input.z* char_speed);
        if (transform.position.y < -5)
        {
            resetChar();

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w") || Input.GetKey("s"))
        {
            char_animator.SetFloat("Speed", 1);
        }
        else
        {
            char_animator.SetFloat("Speed", 0);
        }

        if (Input.GetKeyDown("space")) isJump = true;


        if (isCollidet) {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                time = 3;
                isCollidet = false;
                resetCharCollider();
            }       
        }
        
  
        

    }

    private void resetChar()
    {
        char_animator.SetBool("Grounded", true);
        transform.position = new Vector3(0, 0, 0);
        input = Vector3.zero;

    }

    private void resetCharCollider()
    {
       
        char_animator.SetBool("Grounded", true);
        transform.rotation = Quaternion.identity;
        char_rigidbody.freezeRotation = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("dwads");
            char_rigidbody.freezeRotation = false;
            isCollidet = true;
        }
    }


}
