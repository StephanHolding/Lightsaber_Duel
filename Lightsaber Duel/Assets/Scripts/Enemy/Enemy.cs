using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LightsaberWielder {

    public AnimationClip[] animations;
    public Vector2 waitTime;
    public Lightsaber mySaber;

    private int maxAnimationIndex;
    private Animator anim;
    private bool playingAnimation;
    private bool attacking;
    private float timer;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        mySaber.wieldedBy = this;
    }

    private void Update()
    {
        if (!playingAnimation)
        {
            timer = 0;
            int index = Random.Range(0, animations.Length);
            StartCoroutine(PlayAnimation(index));
        }
    }

    private void Start()
    {
        ToggleSaber(true);
    }

    private void ToggleSaber(bool toggle)
    {
        mySaber.ToggleBlade(toggle, 0);
    }

    public override void ReceiveMessage()
    {
        if (attacking)
        {
            print("Rebound");

            //GameManager.instance.ChangeTimeScale(0);
            StartCoroutine(AttackRebound());
            //anim.SetTrigger("Rebound");
        }
    }

    public void TriggerSlowMotion(int toggle)
    {
        if (toggle == 1)
        {
            switch (GameManager.instance.difficulty)
            {
                case GameManager.Difficulty.Easy:
                    GameManager.instance.ChangeTimeScale(0.1f);
                    break;
                case GameManager.Difficulty.Medium:
                    GameManager.instance.ChangeTimeScale(0.3f);
                    break;
            }
        }
        else if (toggle == 0)
        {
            GameManager.instance.ChangeTimeScale(GameManager.instance.standardTimeScale);
        }

    }

    private IEnumerator PlayAnimation(int animationIndex)
    {
        playingAnimation = true;
        attacking = true;

        anim.SetInteger("Animation Index", animationIndex);
        anim.SetBool("Attack", true);

        yield return new WaitForSeconds(animations[animationIndex].length);

        anim.SetBool("Attack", false);
        attacking = false;

        float toWait = Random.Range(waitTime.x, waitTime.y);
        print(toWait);
        yield return new WaitForSeconds(toWait);


        playingAnimation = false;
    }

    private IEnumerator AttackRebound()
    {
        GameManager.instance.ChangeTimeScale(GameManager.instance.standardTimeScale);
        anim.SetFloat("Rewind", -1);

        yield return new WaitForSeconds(0.5f);

        anim.SetFloat("Rewind", 1);

        StopCoroutine("PlayAnimation");
        anim.SetTrigger("Rebound");

        playingAnimation = false;
    }

}
