using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class LeftControllerScript : MonoBehaviour
{
    // Input mappings to the SteamVR action for left controller
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean toggle;

    // Held object information
    private GameObject collidingObject;
    public static GameObject objectInHand;

    // Teleport Information
    public GameObject teleportPointer;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean teleportActivation;
    public bool hasPosition = false;
    public bool isTeleporting = false;
    public bool isTeleportActive = false;
    public float fadeTime = 0.5f;

    void Update()
    {

        // Side grip button input to reset scene (destroy all objects in scene)
        if (toggle.GetLastStateDown(handType))
        {
            Reset();
        }

        // Trigger pull input to grab object in the scene
        if (grabAction.GetLastStateDown(handType) && collidingObject)
        {
            if(collidingObject != RightControllerScript.model)
            {
                if (collidingObject.tag == "Test Particle")
                {
                    GrabObject();
                }
            }
            
        }

        // Trigger release input to release object in the scene
        if (grabAction.GetLastStateUp(handType) && objectInHand)
        {
            ReleaseObject(); 
        }

        // Menu button input to activate and deactivate teleport function
        if (teleportActivation.GetLastStateUp(handType))
        {
            isTeleportActive = !isTeleportActive;
        }

        // Teleporter stuff (is teleport function is activated)
        if (isTeleportActive)
        {
            // Pointer
            hasPosition = UpdatePointer();
            teleportPointer.SetActive(hasPosition);

            // Teleport
            if (teleportAction.GetLastStateUp(handType))
            {
                TryTeleport();
            }
        }

    }


    // Method to set colliding object
    private void SetCollidingObject(Collider col)
    {
        // Make sure the object has a rigid body component
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
        // Highlight outline of colliding object
        if(col.tag == "Test Particle" && !objectInHand)
        {
            collidingObject.GetComponent<Outline>().enabled = true;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }


    public void OnTriggerExit(Collider other)
    {
        // Cancel outline highlighting
        if (other.tag == "Test Particle")
        {
            other.GetComponent<Outline>().enabled = false;
        }
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    // Method to grab object in the scene (currently only can grab test particles)
    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;
        if (objectInHand.tag == "Test Particle")
        {
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = Mathf.Infinity;
        fx.breakTorque = Mathf.Infinity;
        return fx;
    }

    // Method to reset the scene by destroying of the objects in scene
    private void Reset()
    {
        GameObject[] charge = GameObject.FindGameObjectsWithTag("Test Particle");
        GameObject[] charge2 = GameObject.FindGameObjectsWithTag("Nothing");
        for (int i = 0; i < charge.Length; i++) {
            if (!charge[i].Equals(RightControllerScript.model))
            {
                RightControllerScript.ExplosionEffect(charge[i]);
                Destroy(charge[i]);
            }
        }
        for (int i = 0; i < charge2.Length; i++)
        {
            if (!charge2[i].Equals(RightControllerScript.model))
            {
                Destroy(charge2[i]);
            }
        }

    }

    // Method to release held object (inherits dynamics of the controller at the moment of release)
    private void ReleaseObject()
    {
        
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }

    // Method to attempt teleporting the user in the scene
    void TryTeleport()
    {
       // Check for valid position, and if already teleporting
       if(!hasPosition || isTeleporting)
        {
            return;
        }

        // Get camera rig, and head position
        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 headPosition = SteamVR_Render.Top().head.position;

        // Figure out translation
        Vector3 groundPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
        Vector3 translateVector = teleportPointer.transform.position - groundPosition;

        // Move
        StartCoroutine(MoveRig(cameraRig, translateVector));
    }

    // Method to move the headset camera when teleporting
    public IEnumerator MoveRig(Transform camerRig, Vector3 translation)
    {
        // Flag
        isTeleporting = true;

        // Fade to black
        SteamVR_Fade.Start(Color.black, fadeTime, true);

        // Apply translation
        yield return new WaitForSeconds(fadeTime);
        camerRig.position += translation;

        // Fade to clear
        SteamVR_Fade.Start(Color.clear, fadeTime, true);

        // De-flag
        isTeleporting = false;
    }

    // Method to update the location of the teleport pointer in the scene
    public bool UpdatePointer()
    {
        // Ray from the controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // If it's a hit
        if(Physics.Raycast(ray, out hit))
        {
            teleportPointer.transform.position = hit.point;
            return true;
        }

        // If not a hit
        return false;
    }


}
