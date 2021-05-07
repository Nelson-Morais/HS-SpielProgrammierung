using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Private variables
    
    private Transform cameraTransform;
    private Vector3 pos;
    private float rotX, rotY, rotSpeed , fovSpeed;
    Quaternion rotationQuaternion;
    private bool buttonPressed;


    //Serialized variables (viewable on editor but still private access)
    [SerializeField]private float distance;
    [SerializeField] private float heigth;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float fovMin, fovMax;


    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector3(0,0,0);
        rotSpeed = 3.0f;
        fovSpeed = 10.0f;
        cameraTransform = transform;

    }

    // Update is called once per frame
    void Update()
    {


        
    }
    //Update is called once per frame AFTER update()
    private void LateUpdate()
    {
        //Camera Rotation
        //GetMouseButton(0) = Left
        //GetMouseButton(1) = right
        //GetMouseButton(2) = middle
        if (Input.GetMouseButton(0))
        {
            rotX += Input.GetAxis("Mouse X") * rotSpeed;
            rotY += Input.GetAxis("Mouse Y") * rotSpeed;
            //nochmal Quaternion anschauen !!!
            rotationQuaternion = Quaternion.Euler(rotY, rotX, 0);

            Vector3 position = rotationQuaternion * new Vector3(0, heigth, -distance) + playerTransform.position;

            cameraTransform.position = position;
            cameraTransform.rotation = rotationQuaternion;
           

        }
        else
        {
            rotX = 0;
            rotY = 0;
   
            pos.x = playerTransform.position.x;
            pos.y = playerTransform.position.y + heigth;
            pos.z = playerTransform.position.z - distance;

            cameraTransform.position = pos;
            
        }

        //Camera Zoom
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * fovSpeed;
        fov = Mathf.Clamp(fov, fovMin, fovMax);
        Camera.main.fieldOfView = fov;

        cameraTransform.LookAt(playerTransform);
    }
}
