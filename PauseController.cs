using UnityEngine;

public static class PauseController
{
    // Public flag your NPC.cs checks
    public static bool isGamePaused { get; private set; }

    // Keeps track of how many systems requested pause (dialogue, menu, inventory, etc.)
    private static int pauseRequests = 0;

    /// Pauses/unpauses the game

    public static void SetPause(bool pause)
    {
        if (pause)
        {
            pauseRequests++;
        }
        else
        {
            pauseRequests = Mathf.Max(0, pauseRequests - 1);
        }

        ApplyPauseState(pauseRequests > 0);
    }

    /// Hard override: immediately unpauses and clears all pause requests
    /// Use this only when you MUST force resume 
      public static void ForceResume()
    {
        pauseRequests = 0;
        ApplyPauseState(false);
    }

    private static void ApplyPauseState(bool paused)
    {
        isGamePaused = paused;

        // Freeze gameplay (physics, movement, etc.)
        Time.timeScale = paused ? 0f : 1f;

        // Optional: stop AudioListener globally during pause
        // (comment this out if you want UI sounds during pause)
        AudioListener.pause = paused;
    }
}
