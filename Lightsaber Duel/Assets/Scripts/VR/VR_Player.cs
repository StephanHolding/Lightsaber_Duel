﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Player : MonoBehaviour, IDamageable {

    public float health;
    public Lightsaber rightLightsaber;
    public Lightsaber leftLightsaber;

    private bool left;
    private bool right;

    private void Awake()
    {
        if (rightLightsaber != null)
        {
            rightLightsaber.wieldedBy = this;
            right = true;
        }
        if (leftLightsaber != null)
        {
            leftLightsaber.wieldedBy = this;
            left = true;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void CheckForDeath()
    {
        if (health <= 0)
        {
            print("Dead");
        }
    }

    private void Update()
    {
        if (right)
        {
            if (SteamVR_Input.Lightsaber_Duel.inActions.LightsaberToggle.GetStateDown(SteamVR_Input_Sources.RightHand))
            {

                if (rightLightsaber.blades[0].isEnabled)
                {
                    rightLightsaber.ToggleBlade(false, 0);
                }
                else
                {
                    rightLightsaber.ToggleBlade(true, 0);
                }
            }
        }

        if (left)
        {
            if (SteamVR_Input.Lightsaber_Duel.inActions.LightsaberToggle.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {

                if (leftLightsaber.blades[0].isEnabled)
                {
                    leftLightsaber.ToggleBlade(false, 0);
                }
                else
                {
                    leftLightsaber.ToggleBlade(true, 0);
                }
            }
        }
    }

}
