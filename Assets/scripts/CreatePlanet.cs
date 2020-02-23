using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class CreatePlanet : MonoBehaviour
{
    public SteamVR_Action_Boolean boolAction;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject gravsphere;

    public Material[] mats;

    private bool isFirstPress = true;
    GameObject currentSphere;

    private void Awake()
    {
        boolAction = SteamVR_Actions.default_GrabGrip;
    }

    private void Start()
    {
        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");
    }

    // Update is called once per frame
    void Update()
    {
        //Left hand
        if (boolAction[SteamVR_Input_Sources.LeftHand].stateDown && isFirstPress)
        {
            isFirstPress = false;
            CreateSphere(leftHand);
        }
        else if (boolAction[SteamVR_Input_Sources.LeftHand].lastState && !isFirstPress)
        {
            print("Left hand");

            //Increase mass
            currentSphere.transform.position = leftHand.transform.position;
            currentSphere.transform.rotation = leftHand.transform.rotation;

            currentSphere.GetComponent<Rigidbody>().mass += 5f;

            currentSphere.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            currentSphere.transform.GetChild(0).localScale = new Vector3(.5f, .5f, .5f);
        }

        //Right hand
        if (boolAction[SteamVR_Input_Sources.RightHand].stateDown && isFirstPress)
        {
            isFirstPress = false;
            CreateSphere(rightHand);
        }
        else if (boolAction[SteamVR_Input_Sources.RightHand].lastState && !isFirstPress)
        {
            print("Right hand");

            //Increase mass
            currentSphere.transform.position = rightHand.transform.position;
            currentSphere.transform.rotation = rightHand.transform.rotation;

            currentSphere.GetComponent<Rigidbody>().mass += 5f;

            currentSphere.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            currentSphere.transform.GetChild(0).localScale = new Vector3(.5f, .5f, .5f);
        }

        if (boolAction[SteamVR_Input_Sources.Any].stateUp)
        {
            isFirstPress = true;
        }
    }

    
    private void CreateGravSphere()
    {
        // gravsphere template
        gravsphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gravsphere.tag = "gravsphere";
        // rigidbody properties
        Rigidbody rb = gravsphere.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 400;
        // make interactable
        gravsphere.AddComponent<Interactable>();
        gravsphere.AddComponent<Throwable>();
    }

    private void DestroyGravSphere()
    {
        Destroy(gravsphere);
    }

    private void CreateSphere(GameObject hand)
    {
        CreateGravSphere();

        // create instance of gravsphere
        var sphere = Instantiate(gravsphere);
        sphere.tag = "gravity";
        // set location
        sphere.transform.position = hand.transform.position;
        sphere.transform.rotation = hand.transform.rotation;
        // set scale
        sphere.transform.localScale *= Random.Range(0.25f, 0.5f);
        // update list of spheres
        NewOrbits.UpdateSpheres();

        sphere.GetComponent<Rigidbody>().freezeRotation = true;

        var text3d = new GameObject();
        var textMesh = text3d.AddComponent<TextMesh>();
        var textRenderer = text3d.AddComponent<MeshRenderer>();
        textMesh.text = "Please work";



        textMesh.transform.SetParent(sphere.transform);
        textMesh.transform.localScale *= .5f;

        sphere.GetComponent<Renderer>().material = mats[Random.Range(0, mats.Length)];

        currentSphere = sphere;

        DestroyGravSphere();
    }
}
