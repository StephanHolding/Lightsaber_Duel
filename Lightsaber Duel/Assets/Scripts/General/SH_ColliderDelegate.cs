using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SH_ColliderDelegate : MonoBehaviour {

    public enum InteractionType
    {
        Script,
        Collider,
        Trigger,
    }

    public enum ColliderCondition
    {
        None,
        Name,
        Tag,
    }
    #region How will events be called
    [Tooltip("How will the events be called?" + 
        "\n<b>Script:</b> Events will be called by external script." +
        "\nCollider: Events will be called when this objects collides." +
        "\nTrigger: Events will be called when something enters the trigger collider on this object.")]
    #endregion
    [Header("Interaction Type")]
    public InteractionType interactionType;

    [Tooltip("A tag for the collider on this object. Useful for extra check when calling events via external script.")]
    public string colliderTag;

    [Header("Collider Conditions")]
    [Tooltip("A condition that the other colliding object should meet before events are called." +
        "\nNone: No condition must be met." +
        "\nName: Other Collider must have specified name." +
        "\nTag: Other Collider must have specified tag.")]
    public ColliderCondition colliderCondition;
    public string condition;

    [Header("Other Proporties")]
    public float invokeDelay;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onEnter;
    [SerializeField]
    private UnityEvent onExit;

    public delegate void Delegate();
    public Delegate toCall;

    #region Script Callable

    public void GotCollision()
    {
        onEnter.Invoke();
    }

    public void GotCollision(Delegate toCall, bool callEvent = true)
    {
        toCall.Invoke();

        if (callEvent)
            onEnter.Invoke();
    }

    #endregion
    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (interactionType == InteractionType.Trigger)
        {
            if (ConditionCheck(other))
                StartCoroutine(InvokeDelay(onEnter, invokeDelay));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactionType == InteractionType.Trigger)
        {
            if (ConditionCheck(other))
                StartCoroutine(InvokeDelay(onExit, invokeDelay));
        }
    }

    #endregion
    #region Collisions

    private void OnCollisionEnter(Collision collision)
    {
        if (interactionType == InteractionType.Collider)
        {
            if (ConditionCheck(collision))
                StartCoroutine(InvokeDelay(onEnter, invokeDelay));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (interactionType == InteractionType.Collider)
        {
            if (ConditionCheck(collision))
                StartCoroutine(InvokeDelay(onExit, invokeDelay));
        }
    }

    #endregion

    private bool ConditionCheck(Collision collision)
    {
        bool toReturn = false;

        switch (colliderCondition)
        {
            case ColliderCondition.None:
                toReturn = true;
                break;
            case ColliderCondition.Name:
                if (collision.gameObject.name == condition)
                    toReturn = true;
                break;
            case ColliderCondition.Tag:
                if (collision.gameObject.tag == condition)
                    toReturn = true;
                break;
        }

        return toReturn;
    }
    private bool ConditionCheck(Collider collider)
    {
        bool toReturn = false;

        switch (colliderCondition)
        {
            case ColliderCondition.None:
                toReturn = true;
                break;
            case ColliderCondition.Name:
                if (collider.gameObject.name == condition)
                    toReturn = true;
                break;
            case ColliderCondition.Tag:
                if (collider.gameObject.tag == condition)
                    toReturn = true;
                break;
        }

        return toReturn;
    }

    private IEnumerator InvokeDelay(UnityEvent toCall, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        toCall.Invoke();
    }

    #region Context Menu
    [ContextMenu("Call OnEnter")]
    public void CallOnEnter()
    {
        onEnter.Invoke();
    }

    [ContextMenu("Call OnExit")]
    public void CallOnExit()
    {
        onExit.Invoke();
    }

    #endregion
}
