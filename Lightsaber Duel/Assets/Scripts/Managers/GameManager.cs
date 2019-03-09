using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
    }

    public Difficulty difficulty;

    public float standardTimeScale { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetToStandardTimescale();
    }

    public void ToggleSlowMotion(bool toggle)
    {
        if (toggle)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    ChangeTimeScale(0.2f);
                    break;
                case Difficulty.Medium:
                    ChangeTimeScale(0.75f);
                    break;
            }
        }
        else
        {
            SetToStandardTimescale();
        }
    }

    private void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    private void SetToStandardTimescale()
    {
        if (difficulty == Difficulty.Easy)
        {
            standardTimeScale = 0.8f;
        }
        else
        {
            standardTimeScale = 1;
        }

        ChangeTimeScale(standardTimeScale);
    }
}
