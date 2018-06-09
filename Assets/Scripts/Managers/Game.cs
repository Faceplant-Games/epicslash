using UnityEngine.SceneManagement;

public class Game {
    public static bool Started = GetCurrentStage() != 0;
    public static long Level = 0;
    public static bool IsTransitioning = false;
    public static GameManager GameManager;

    public static void InitializeDefaultValues()
    {
        Started = false;
        Level = 0;
        IsTransitioning = false;
    }

    public static int GetCurrentStage()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
