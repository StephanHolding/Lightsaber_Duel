using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber_Bladehit : MonoBehaviour {

    internal Lightsaber parent;

    private bool clashlock;

    private void OnCollisionEnter(Collision collision)
    {
        EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Clash"), "Player Lightsaber Clash", false, 1, 1, collision.transform.position);

        parent.OnHit(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!clashlock)
        {
            EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Clashlock"), "Player Lightsaber Clashlock", true, 1, 1, collision.transform.position);
            clashlock = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        EffectsManager.instance.StopAudio("Player Lightsaber Clashlock");
        clashlock = false;
    }
}
