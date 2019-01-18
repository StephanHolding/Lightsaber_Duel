using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber_Singlebladed : Lightsaber {

    public const int bladeAmount = 1;

    private Vector3 previousFramePosition;

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
            float bladeSpeed = GetBladeSpeed(blades[0]);

            if (bladeSpeed > 0)
            {
                float volume = bladeSpeed / 6;
                EffectsManager.instance.AdjustVolume("Player Lightsaber Swing " + 0, volume);
            }
        }
    }

    private void FixedUpdate()
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

    private float GetBladeSpeed(Blade blade)
    {
        float movementPerFrame = Vector3.Distance(previousFramePosition, blade.bladeCollider.transform.position);
        float toReturn = movementPerFrame / Time.deltaTime;
        previousFramePosition = blades[0].bladeCollider.transform.position;

        return toReturn;
    }
}
