using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber_HitDetection : MonoBehaviour {

    private Lightsaber saber;
    private bool colliding;

    private void Awake()
    {
        saber = GetComponentInParent<Lightsaber>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Enter");
        saber.OnHit(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        print("OK");
    }

}
