using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game {
    public static bool started = GetCurrentStage() != 0;
    public static long level = 0;
    public static bool isTransitioning = false;
    public static GameManager gameManager;

    public static void InitializeDefaultValues()
    {
        started = false;
        level = 0;
        isTransitioning = false;
    }

    public static int GetCurrentStage()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
