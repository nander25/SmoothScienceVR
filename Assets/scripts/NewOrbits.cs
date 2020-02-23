using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class NewOrbits : MonoBehaviour
{
    //public int sphereCount = 9;
    public int maxRadius = 10;
    public static GameObject[] spheres;
    public Material[] mats;
    public Material trailMat;
    public bool initVelocity = false;
    public float velocity_scale = 1;
    public Vector3 offset = new Vector3(0, 0, 0);

    void Awake()
    {
        //Spheres contains all of the objects created
        UpdateSpheres();
    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateSpheres(sphereCount, maxRadius);

        if (initVelocity)
            this.GetComponent<Rigidbody>().velocity = this.transform.forward * velocity_scale;
    }

    public void CreateSpheres(int count, int radius)
    {
        var sphs = new GameObject[count];

        //Sphere that is a template for gravity spheres
        var sphereToCopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = sphereToCopy.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 1000;

        sphereToCopy.AddComponent<Interactable>();
        sphereToCopy.AddComponent<Throwable>();

        for (int i = 0; i < count; i++)
        {
            //Create spheres using templates
            var sp = GameObject.Instantiate(sphereToCopy);
            sp.transform.position = this.transform.position +
                new Vector3(Random.Range(-maxRadius, maxRadius),
                Random.Range(-maxRadius, maxRadius),
                Random.Range(-maxRadius, maxRadius));
            sp.transform.localScale *= Random.Range(0.5f, 1);

            //Commented code creates a trail and adds a material to the sphere
            /*sp.GetComponent<Renderer>().material = mats[Random.Range(0, mats.Length)];

            TrailRenderer tr = sp.AddComponent<TrailRenderer>();
            tr.time = 1.0f;
            tr.startWidth = 0.1f;
            tr.endWidth = 0;
            tr.material = trailMat;
            tr.startColor = new Color(1, 1, 0, 0.1f);
            tr.endColor = new Color(0, 0, 0, 0);
            */
            spheres[i] = sp;


        }

        GameObject.Destroy(sphereToCopy);

        return;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject s in spheres)
        {
            if (s == null || s == gameObject) continue;

            // Calculations for gravity
            Vector3 difference = this.transform.position - s.transform.position;
            float dist = difference.magnitude / 100;

            float gravity = 100 * 6.7e-11f * (this.GetComponent<Rigidbody>().mass * s.GetComponent<Rigidbody>().mass) / (dist);
            Vector3 gravDir = difference.normalized;
            Vector3 gravVec = gravDir * gravity;

            CreateLabel(s, dist, gravity);

            s.transform.GetComponent<Rigidbody>().AddForce(gravVec, ForceMode.Force);
        }
    }

    public static void UpdateSpheres()
    {
        spheres = GameObject.FindGameObjectsWithTag("gravity");
    }

    private void CreateLabel(GameObject sphere, float distance, float gravity)
    {
        if (sphere.transform.childCount == 0) return;

        sphere.transform.GetChild(0).eulerAngles = Camera.main.transform.eulerAngles; // set our angle to be the same as the one the camera is facing
        sphere.transform.GetChild(0).transform.localPosition = offset;
        var textMesh = sphere.transform.GetChild(0).GetComponent<TextMesh>();

        if ((distance * 100) > 100)
        {
            textMesh.text = "";
        }
        else
        {
            int m = (int) Mathf.Round(sphere.GetComponent<Rigidbody>().mass);
            int r = (int) Mathf.Round(distance * 100);
            int g = (int) Mathf.Round(gravity);

            textMesh.text = "m: " + m + "\nr: " + r + "\nF: " + g;
            /*
            textMesh.text = "m: " + (int) Mathf.Round(sphere.GetComponent<Rigidbody>().mass) +
                "\nr: " + (int) Mathf.Round(distance * 100) +
                "\nF: " + (int) Mathf.Round(gravity);*/
        }
    }
    /*
    private Vector3 GetCameraFoward()

    {

        Vector3 forward = Camera.main.transform.forward * 3;

        forward.y += 2f;

        return forward;

    }*/
}




