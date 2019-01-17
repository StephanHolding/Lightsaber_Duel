using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber_Singlebladed : Lightsaber {

    public const int bladeAmount = 1;

    private void OnValidate()
    {
        if (blades.Length != bladeAmount)
        {
            Debug.LogWarning("This is a one bladed lightsaber!");
        }
    }

    private void Update()
    {
        if (blades[0].isEnabled)
        {
            RaycastHit hit = CheckBladeHit(blades[0].bladeCollider.transform, blades[0].bladeLength, lightsaberMask);
            if (hit.transform != null)
            {
                OnHit(hit.transform);
            }
            else
            {
                OnHitExit();
            }
        }
    }
}
