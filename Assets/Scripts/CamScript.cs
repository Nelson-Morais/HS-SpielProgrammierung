using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamScript : MonoBehaviour
{
    // The target GameObject the camera follows and rotates Around
    public GameObject target;


    // Offset of the camera position above the offset
    [Range(0.0f, 10.0f)]
    public float height = 4.0f;

    // The Speed of the camera rotation controlled by mouse movement
    [Range(50.0f, 500.0f)]
    public float rotationSpeed = 300.0f;

    // Indicates Weather to flip the rotation around the y-axis
    public bool invertMouse = false;

    // Offset from the target transforms position to the point the Camera looks at
    public Vector3 offset = Vector3.up;

    public Vector2 scrollLimit = new Vector2(10e-6f, 50f);
    // How far back the camera is from the offset
    [Range(10e-6f, 50f)]
    public float distance = 1.0f;

    [Range(0.0f, 200.0f)]
    public float scrollSpeed = 10.0f;

    // Indicates weather the camera should currently be rotated when mouse movement occurs
    private bool rotateCamera = false;


    void LateUpdate()
    {
        UpdateInput();
        UpdateTransform();
    }

    public void UpdateTransform()
    {



        // Initialize camera position
        transform.position = target.transform.position + offset;


        // Extract xz component of the forward vector to go back in that direction by distance units
        Vector3 direction = transform.forward;
        direction.y = 0;
        if (direction.sqrMagnitude < 10e-10)
        {
            direction = -transform.up;
        }
        direction.Normalize();


        transform.position -= direction * distance;



        // Move up height units
        transform.position += Vector3.up * height;

        // Look at the target Position
        transform.LookAt(target.transform.position + offset);
    }

    private void UpdateInput()
    {
        // Update the state of rotateCamera
        if (Input.GetMouseButtonDown(0))
        {
            rotateCamera = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rotateCamera = false;
        }
        // Update Distance
        distance = Mathf.Clamp(distance + (Input.mouseScrollDelta.y * scrollSpeed * Time.deltaTime), scrollLimit.x, scrollLimit.y);

        // Rotate
        if (rotateCamera)
        {
            transform.Rotate(target.transform.up, Time.deltaTime * rotationSpeed * Input.GetAxis("Mouse X") * (invertMouse ? 1 : -1));
        }
    }
}
