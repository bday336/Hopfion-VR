using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RightControllerScript : MonoBehaviour
{
    // Input mappings to the SteamVR action for right controller
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean toggle;
    public SteamVR_Action_Boolean toggleFlow;

    // Object currently in hand to be placed in scene
    private GameObject objectInHand;

    // Options of objects to be placed in the scene
    public static GameObject model;                         // Model of placeable object
    GameObject[] placables;                                 // Container for placeable objects
    public GameObject nothing;                              // Stand in for an empty hand
    public GameObject testParticle;                         // Stand in for test particle
    int index = 0;                                          // Index to choose placeable objects
    Vector3 placementPosition;                              // Controller placement for object

    // Explosion effect for when the particles are destroyed
    public GameObject effect;
    public static GameObject explosion;


    private void Start()
    {
        // Initialize the explosion effect
        explosion = effect;   

        // Initialize the container for placeable objects and
        // instantiate the object in controller hand
        placables = new GameObject[] { testParticle, nothing };
        objectInHand = placables[index];
        model = Instantiate(objectInHand, transform.position + 0.2f * transform.forward, Quaternion.identity, gameObject.transform);
        model.GetComponent<Collider>().enabled = false;

    }
    void Update()
    {
        // Update the held object position with controller position
        placementPosition = transform.position + 0.2f * transform.forward;
        model.transform.position = placementPosition;

        // Trigger input to spawn currently selected placeable object
        if (grabAction.GetLastStateDown(handType))
        {
            SpawnModel();
        }

        // Menu button input to activate and deactivate the particle flow
        if (toggleFlow.GetLastStateDown(handType))
        {
            TestParticleScript.ToggleFlow();
        }

        // Side grip button input to change between placeable objects
        if (toggle.GetLastStateDown(handType))
        {
            index++;
            if (index == placables.Length)
            {
                index = 0;
            }
            SetModel(index, placables);

        }
       
    }

    // Method to set/change the model of object held by controller
    void SetModel(int index_, GameObject[] placables_)
    {
        objectInHand = placables_[index_];
        Destroy(model);
        model = Instantiate(objectInHand, placementPosition, gameObject.transform.rotation, gameObject.transform);
        model.transform.localRotation = objectInHand.transform.rotation;
        model.GetComponent<Collider>().enabled = false;
    }

    // Method to spawn the held object in the scene
    void SpawnModel()
    {
        GameObject temp = Instantiate(model, placementPosition, model.transform.rotation);
        temp.GetComponent<Collider>().enabled = false;

    }

    // Method to handle the explosion effect when a particle is destroyed
    public static void ExplosionEffect(GameObject spawnedObject)
    {
        ParticleSystem.MainModule ps = explosion.GetComponent<ParticleSystem>().main;

        ps.startColor = spawnedObject.GetComponent<MeshRenderer>().sharedMaterial.color;

        GameObject instance = Instantiate(explosion, spawnedObject.transform.position, Quaternion.identity);

        Destroy(instance, ps.startLifetimeMultiplier);

    }
}
