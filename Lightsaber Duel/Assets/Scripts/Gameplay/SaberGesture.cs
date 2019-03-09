using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberGesture : MonoBehaviour {

    public int totalTriggers;
    public bool completed;
    private int successTriggers;

    private void CheckGestureCompletion()
    {
        if (successTriggers >= totalTriggers)
        {
            completed = true;
            print("Completed");
        }
        else
        {
            completed = false;
        }
    }

    public void SaberEntered()
    {
        successTriggers += 1;
        CheckGestureCompletion();
    }

    public void SaberExit()
    {
        successTriggers -= 1;
        CheckGestureCompletion();
    }

    public void Refresh()
    {
        completed = false;
        successTriggers = 0;
    }
}
