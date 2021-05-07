using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterControl : MonoBehaviour
{
    [SerializeField]private Transform checkGround;
    [SerializeField] private float objHeigth;
    private Animator animator;
    private Camera cam;
    private Rigidbody player_rigidBody;
    private float speed = 1.0f;
    private Vector3 forward, down;
    private float sphereRadius;
    private Vector3 vecPos = new Vector3(0,1.1f,0);
    private bool falling = false;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        player_rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        forward = new Vector3(0, 0, 0);
        speed = 5.0f;
        sphereRadius = 0.5f;
        down = new Vector3();
        cam = Camera.main; 
        
    }

    // Update is called once per frame
    void Update()
    {

            
        if(forward!= Vector3.zero)
        {
        transform.rotation = Quaternion.LookRotation(new Vector3(forward.x,0,forward.z));
        }
        if (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w") || Input.GetKey("s"))
        {
            animator.SetFloat("Speed", 1);
        }else
        {
            animator.SetFloat("Speed", 0);
        }

       

        if (Physics.SphereCast(transform.position+vecPos,sphereRadius, Vector3.down, out RaycastHit raycasthit, objHeigth))
        {
            
           
        
        
            TipToePlatform platform = raycasthit.collider.gameObject.GetComponent<TipToePlatform>();
            if(platform != null)
            {
                platform.CharacterTouches();
            }
            GoalPlatform goalPlatform = raycasthit.collider.gameObject.GetComponent<GoalPlatform>();
            if (goalPlatform != null)
            {
                resetChar();
            }
            //pos.x = Input.GetAxis("Horizontal");
            //pos.z = Input.GetAxis("Vertical");
            //pos += cam.transform.forward;
            //pos += cam.transform.right;

            forward = cam.transform.forward * Input.GetAxis("Vertical");
            forward += cam.transform.right * Input.GetAxis("Horizontal");
            forward.y = 0;
            forward.Normalize();
            animator.SetBool("Grounded", true);

        }
        else
        {
            animator.SetBool("Grounded", false);
            forward.y -= 0.1f;
        }

        
        transform.position += forward.normalized * Time.deltaTime * speed;

       
        if(transform.position.y < -5)
        {
            resetChar();
           
        }
    }



    private void OnDrawGizmosSelected() { 
        Gizmos.DrawWireSphere(transform.position+vecPos, sphereRadius); 
    }


    private void resetChar()
    {
        
        animator.SetBool("Grounded", true);
        transform.position = new Vector3(0, 0, 0);
        forward.y = 0;
       ;
        
    }
}
