using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Player : MonoBehaviour {

    public Lightsaber rightLightsaber;
    public Lightsaber leftLightsaber;

    private void Update()
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
