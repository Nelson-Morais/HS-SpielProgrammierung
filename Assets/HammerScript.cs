using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerScript : MonoBehaviour
{

    public float speed = 1.0f;
    // Start is called before the first frame update

    public GameObject rotor;
    private Rigidbody rotorRigidbody;
    void Start()
    {
        rotorRigidbody = rotor.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rotorRigidbody.MoveRotation(rotorRigidbody.rotation * Quaternion.Euler(360 * speed * Time.deltaTime,0,0));
    }

}
