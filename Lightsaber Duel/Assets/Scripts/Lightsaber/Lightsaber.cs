using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : Weapon {

    public float saberExtendSpeed;

    [System.Serializable]
    public class Blade {

        public float bladeLength;
        public LineRenderer blade;
        public Light bladeLight;
        public bool isEnabled;
        public Lightsaber_Color_ScriptableObject bladeColor;
        public Collider bladeCollider;

        internal float currentLength;
        internal RaycastHit hit;
    }

    public Blade[] blades;
    public LayerMask lightsaberMask;

    private bool isColliding;

    public void ToggleBlade(bool toggle, int bladeIndex)
    {
        blades[bladeIndex].currentLength = blades[bladeIndex].blade.GetPosition(1).y;

        Color baseColor = blades[bladeIndex].bladeColor.bladeColor;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(13);
        blades[bladeIndex].blade.material.SetColor("_EmissionColor", finalColor);

        blades[bladeIndex].bladeLight.color = blades[bladeIndex].bladeColor.lightColor;

        StartCoroutine(ExtendBlade(toggle, bladeIndex));
    }

    public void OnHit(Transform gotHit)
    {
        if (gotHit.gameObject.layer == 9)
        {
            if (!isColliding)
            {
                EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Clash"), "Lightsaber Clash Player");
                EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Clashlock"), "Lightsaber Clashlock Player");
                isColliding = true;
            }
        }
        print("Hit");
    }

    public void OnHitExit()
    {
        if (isColliding)
        {
            EffectsManager.instance.StopAudio("Lightsaber Clashlock Player");
            isColliding = false;
        }
    }

    public RaycastHit CheckBladeHit(Transform raycastOrigin, float raycastRange, LayerMask mask)
    {
        RaycastHit toReturn;

        Debug.DrawRay(raycastOrigin.position, raycastOrigin.up * raycastRange, Color.black);
        Physics.Raycast(raycastOrigin.position, raycastOrigin.up, out toReturn, raycastRange, mask);

        return toReturn;
    }

    private IEnumerator ExtendBlade(bool toggle, int bladeIndex)
    {
        if (toggle)
        {
            EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Extend"), "Player Lightsaber Extend");
            EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Hum"), "Player Lightsaber Hum");

            while (blades[bladeIndex].currentLength < blades[bladeIndex].bladeLength)
            {
                blades[bladeIndex].currentLength += saberExtendSpeed;
                blades[bladeIndex].blade.SetPosition(1, new Vector3(0, blades[bladeIndex].currentLength, 0));

                yield return null;
            }
        }
        else
        {
            EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Retract"), "Player Lightsaber Retract");
            EffectsManager.instance.StopAudio("Player Lightsaber Hum");

            while (blades[bladeIndex].currentLength > 0)
            {
                blades[bladeIndex].currentLength -= saberExtendSpeed;
                blades[bladeIndex].blade.SetPosition(1, new Vector3(0, blades[bladeIndex].currentLength, 0));

                yield return null;
            }
        }

        blades[bladeIndex].bladeLight.enabled = toggle;
        blades[bladeIndex].isEnabled = toggle;
        blades[bladeIndex].bladeCollider.enabled = toggle;
    }



}
