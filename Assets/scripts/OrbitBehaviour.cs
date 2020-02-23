using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class OrbitBehaviour : MonoBehaviour
{
    public int sphereCount = 10;
    public int maxRadius = 10;
    public GameObject[] spheres;
    public Material[] mats;
    public Material trailMat;

    void Awake()
    {

        //Spheres contains all of the objects created
        spheres = new GameObject[sphereCount];
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateSpheres(sphereCount, maxRadius);
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
            //Calculations for gravity
            Vector3 difference = this.transform.position - s.transform.position;

            float dist = difference.magnitude;
            dist /= 100f;
            
            Vector3 gravDir = difference.normalized;
            float gravity = 6.7e-11f * (this.GetComponent<Rigidbody>().mass * s.GetComponent<Rigidbody>().mass) / (dist * dist);

            // print(gravity);

            Vector3 gravVec = gravDir * gravity;

            s.transform.GetComponent<Rigidbody>().AddForce(s.transform.forward * 100, ForceMode.Force);

            s.transform.GetComponent<Rigidbody>().AddForce(gravVec, ForceMode.Force);
        }

    }
}
