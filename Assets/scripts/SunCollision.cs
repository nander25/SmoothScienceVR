using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var colliderObj = collision.collider.gameObject;
        if (colliderObj.tag != "gravity") return;
        Destroy(colliderObj);
        NewOrbits.UpdateSpheres();
    }
}
