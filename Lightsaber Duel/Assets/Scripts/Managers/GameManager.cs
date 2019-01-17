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
        SetStandardTimescale();
    }

    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    private void SetStandardTimescale()
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
