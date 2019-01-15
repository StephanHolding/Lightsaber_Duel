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
        public CapsuleCollider bladeCollider;
        internal float currentLength;
    }

    public Blade[] blades;

    public void ToggleBlade(bool toggle, int bladeIndex)
    {
        blades[bladeIndex].currentLength = blades[bladeIndex].blade.GetPosition(1).y;

        Color baseColor = blades[bladeIndex].bladeColor.bladeColor;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(13);
        blades[bladeIndex].blade.material.SetColor("_EmissionColor", finalColor);

        blades[bladeIndex].bladeLight.color = blades[bladeIndex].bladeColor.lightColor;

        StartCoroutine(ExtendBlade(toggle, bladeIndex));
    }

    public void OnHit(Collision gotHit)
    {
        print("Hit");
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
