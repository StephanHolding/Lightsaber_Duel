using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    public struct Attack
    {
        public SaberGesture gesture;
        public int animationIndex;
    }

    public Attack[] attacks;
    public Lightsaber mySaber;

    private Animator anim;
    private bool readyForNewAttack;
    private bool animationDone;
    private Attack currentAttack;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        foreach(Attack a in attacks)
        {
            a.gesture.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        readyForNewAttack = true;

        mySaber.ToggleBlade(true, 0);
    }

    private void Update()
    {
        if (readyForNewAttack)
        {
            StartCoroutine(ExecuteAttack());
        }
    }

    public void TriggerSlowMotion(int trigger)
    {
        if (trigger == 0)
        {
            GameManager.instance.ToggleSlowMotion(false);
        }
        else if (trigger == 1)
        {
            GameManager.instance.ToggleSlowMotion(true);
        }
    }

    public void TriggerGesture(int trigger)
    {
        if (trigger == 0)
        {
            currentAttack.gesture.gameObject.SetActive(false);
        }
        else if (trigger == 1)
        {
            currentAttack.gesture.gameObject.SetActive(true);
        }
    }

    public void AnimationDone()
    {
        animationDone = true;
    }

    private IEnumerator ExecuteAttack()
    {
        readyForNewAttack = false;
        animationDone = false;

        currentAttack = GetAttack();

        currentAttack.gesture.Refresh();
        currentAttack.gesture.gameObject.SetActive(true);

        anim.SetInteger("Animation Index", currentAttack.animationIndex);

        anim.SetBool("New Attack", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("New Attack", false);

        yield return new WaitUntil(() => animationDone);

        //yield return new WaitForSeconds(2);

        readyForNewAttack = true;
    }

    private Attack GetAttack()
    {
        Attack toReturn = attacks[Random.Range(0, attacks.Length)];
        return toReturn;
    }
}
