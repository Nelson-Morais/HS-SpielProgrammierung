using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCharacterControl : MonoBehaviour
{
    [SerializeField] private float char_speed = 0;
    private Animator char_animator;
    private Rigidbody char_rigidbody;
    private Camera cam;
    private Vector3 input;
    private Vector3 sphereOffset = new Vector3(0, 1.1f, 0);

    // Start is called before the first frame update
    void Start()
    {
        char_speed = 5;
        char_rigidbody = GetComponent<Rigidbody>();
        char_animator = GetComponent<Animator>();
        cam = Camera.main;
        input = new Vector3(0, 0, 0);
    }

    //called in Sync with Physics engine
    private void FixedUpdate()
    {

        if (Physics.SphereCast(transform.position + sphereOffset, 0.5f, Vector3.down, out RaycastHit raycasthit, 2.0f))
        {

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
            input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (input != Vector3.zero) transform.rotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.z));

            

        }
        else
        {
            char_animator.SetBool("Grounded", false);
            input.y -= 0.1f;
        }

        char_rigidbody.MovePosition(transform.position + input.normalized * Time.deltaTime * char_speed);

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

    }

    private void resetChar()
    {

        char_animator.SetBool("Grounded", true);
        transform.position = new Vector3(0, 0, 0);
        input.y = 0;


    }


}
