using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {
    public static int currentStage = 0;
    public static bool started = false;
    public static long level = 0;
    public static bool isTransitioning = false;
    public static GameManager gameManager;

    public static void InitializeDefaultValues()
    {
        currentStage = 0;
        started = false;
        level = 0;
        isTransitioning = false;
    }
}
