using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {

    public Lightsaber_Singlebladed lightsaber;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (lightsaber.blades[0].isEnabled)
            {
                lightsaber.ToggleBlade(false, 0);
            }
            else
            {
                lightsaber.ToggleBlade(true, 0);
            }
        }
    }
}
