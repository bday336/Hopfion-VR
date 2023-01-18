using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleScript : MonoBehaviour
{
    // Information for test particle prefab
    Rigidbody rigidBody;
    public float surviveDistance;                   // Distance from origin at which the test particle is destoryed
    public static bool flowToggle = false;          // Check to determine if the particle is flowing with vector field

    // Placeholder variables to simplify the field expressions
    public float px;
    public float py;
    public float pz;
    public float vecx;
    public float vecy;
    public float vecz;

    private void Start()
    {
        // Make sure it has rigid body component for physics
        rigidBody = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        // Check if beyond survive range
        if (transform.position.magnitude > surviveDistance)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        // Particle only flows when the toggle is flagged to flow
        if (flowToggle)
        {
            // Physical position
            Vector3 pos = rigidBody.transform.position;

            // Move to fit mathematical field
            pos = pos + Vector3.down*1.1f;

            // Transform to right handed coordinate frame (Unity -> real world)
            float px = pos.x;
            float py = pos.z;
            float pz = pos.y;

            // Set the position through simple forward Euler time stepping
            px = (px + Time.deltaTime * (4f * (2f * px * pz - py * (px * px + py * py + pz * pz - 1f)) / ((1f + px * px + py * py + pz * pz) * (1f + px * px + py * py + pz * pz))));
            py = (py + Time.deltaTime * (4f * (2f * py * pz + px * (px * px + py * py + pz * pz - 1f)) / ((1f + px * px + py * py + pz * pz) * (1f + px * px + py * py + pz * pz))));
            pz = (pz + Time.deltaTime * (1f - 8f * (px * px + py * py) / ((1f + px * px + py * py + pz * pz) * (1f + px * px + py * py + pz * pz))));

            // Transform to left handed coordinate frame (real world -> Unity)
            pos.Set(px,pz,py);

            // Move back to physical position
            pos = pos + Vector3.up * 1.1f;

            // Set physical position
            rigidBody.transform.position = pos;
        }

    }

    // Method to toggle flow
    public static void ToggleFlow()
    {
        flowToggle = !flowToggle;
    }

}
