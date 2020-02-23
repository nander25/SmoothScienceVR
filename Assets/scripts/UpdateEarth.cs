using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEarth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 10;
        print(transform.forward);
        NewOrbits.UpdateSpheres();
    }
}
