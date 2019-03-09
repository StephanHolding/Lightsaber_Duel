using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : Weapon {

    [System.Serializable]
    public class Blade
    {
        public float bladeLength;
        public LineRenderer blade;
        public Light bladeLight;
        public bool isEnabled;
        public Lightsaber_Color_ScriptableObject bladeColor;
        public Collider bladeCollider;

        internal int humSound;
        internal int swingSound;

        internal float currentLength;
        internal RaycastHit hit;
    }

    [Header("Blades")]
    public Blade[] blades;

    [Header("Saber Properties")]
    public float saberExtendSpeed;
    public float damage;
    public IDamageable wieldedBy;

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
        if (gotHit != null)
        {
            switch (gotHit.transform.tag)
            {
                case "Damageable":
                    gotHit.GetComponent<IDamageable>().TakeDamage(damage);
                    break;
                case "Lightsaber":
                    EffectsManager.instance.PlayAudio("Lightsaber Clash");
                    break;
            }
        }
    }

    public void OnHitExit()
    {
        if (isColliding)
        {
            isColliding = false;
        }
    }

    private IEnumerator ExtendBlade(bool toggle, int bladeIndex)
    {
        if (toggle) //when the blade should be extended
        {
            EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Extend"));
            blades[bladeIndex].humSound = EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Hum"), loop: true);
            blades[bladeIndex].swingSound = EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Swing"), loop: true, volume: 0);

            while (blades[bladeIndex].currentLength < blades[bladeIndex].bladeLength)
            {
                blades[bladeIndex].currentLength += saberExtendSpeed;
                blades[bladeIndex].blade.SetPosition(1, new Vector3(0, blades[bladeIndex].currentLength, 0));

                yield return null;
            }
        }
        else        //When the blade should be retracted.
        {
            EffectsManager.instance.PlayAudio(EffectsManager.instance.FindAudioClip("Lightsaber Retract"));
            EffectsManager.instance.StopAudio(blades[bladeIndex].humSound);
            EffectsManager.instance.StopAudio(blades[bladeIndex].swingSound);

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
