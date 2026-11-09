using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title,
    Playing,
    Paused,
    GameOver,
    Won
}

// Created automatically when the game starts. Handles pausing, game over and restarting,
// which previously only existed as Debug.Log messages in the editor.
public class GameManager : MonoBehaviour
{
    const string HighScoreKey = "HighScore";

    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }

    GameObject overlay;
    // when restarting after game over / win, skip the title screen
    bool skipTitle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Bootstrap()
    {
        if (Instance != null)
        {
            return;
        }
        GameObject go = new GameObject("GameManager");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<GameManager>();
        Instance.OnSceneStart();
        SceneManager.sceneLoaded += (scene, mode) => Instance.OnSceneStart();
    }

    void OnSceneStart()
    {
        // overlay belongs to the previous scene and was destroyed with it
        overlay = null;
        if (skipTitle)
        {
            skipTitle = false;
            SetPaused(false);
            State = GameState.Playing;
        }
        else
        {
            ShowTitle();
        }
    }

    void ShowTitle()
    {
        State = GameState.Title;
        SetPaused(true);
        string body = "HIGH SCORE " + PlayerPrefs.GetInt(HighScoreKey, 0).ToString("D6") +
                      "\n\nPress ENTER or SPACE to start" +
                      "\n\n<size=30>Arrow keys / WASD - move     Esc / P - pause</size>";
        ShowOverlay("PAC-MAN", Color.yellow, body, 0.92f);
    }

    public void StartGame()
    {
        State = GameState.Playing;
        SetPaused(false);
        HideOverlay();
    }

    void Update()
    {
        switch (State)
        {
            case GameState.Title:
                if (StartPressed())
                {
                    StartGame();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
                {
                    Quit();
                }
                break;

            case GameState.Playing:
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                {
                    Pause();
                }
                break;

            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                {
                    Resume();
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    Restart();
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    Quit();
                }
                break;

            case GameState.GameOver:
            case GameState.Won:
                if (StartPressed() || Input.GetKeyDown(KeyCode.R))
                {
                    Restart();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
                {
                    Quit();
                }
                break;
        }
    }

    public void Pause()
    {
        State = GameState.Paused;
        SetPaused(true);
        ShowOverlay("PAUSED", Color.yellow, "Esc / P - resume\nR - restart" + QuitHint());
    }

    public void Resume()
    {
        State = GameState.Playing;
        SetPaused(false);
        HideOverlay();
    }

    public void GameOver()
    {
        if (State == GameState.GameOver)
        {
            return;
        }
        State = GameState.GameOver;
        SetPaused(true);
        ShowOverlay("GAME OVER", Color.red, ScoreSummary() + "\n\nEnter / Space - play again" + QuitHint());
    }

    public void Win()
    {
        if (State != GameState.Playing)
        {
            return;
        }
        State = GameState.Won;
        SetPaused(true);
        ShowOverlay("YOU WIN!", Color.yellow,
            "All pellets collected!\n\n" + ScoreSummary() + "\n\nEnter / Space - play again" + QuitHint());
    }

    public void Restart()
    {
        skipTitle = true;
        SetPaused(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif !UNITY_WEBGL
        Application.Quit();
#endif
    }

    // Returns "SCORE x   HIGH SCORE y" and stores a new high score if needed.
    string ScoreSummary()
    {
        int score = CurrentScore();
        int best = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt(HighScoreKey, best);
            PlayerPrefs.Save();
        }
        return "SCORE " + score.ToString("D6") + "     HIGH SCORE " + best.ToString("D6");
    }

    public static int CurrentScore()
    {
        PlayerScore playerScore = FindObjectOfType<PlayerScore>();
        if (playerScore == null || playerScore.score == null)
        {
            return 0;
        }
        string[] lines = playerScore.score.text.Split('\n');
        int value;
        return int.TryParse(lines[lines.Length - 1].Trim(), out value) ? value : 0;
    }

    static bool StartPressed()
    {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
               Input.GetKeyDown(KeyCode.Space);
    }

    static string QuitHint()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return "";
#else
        return "\nQ - quit";
#endif
    }

    void ShowOverlay(string title, Color color, string body, float backgroundAlpha = 0.8f)
    {
        HideOverlay();
        overlay = Overlay.Show(title, color, body, backgroundAlpha);
    }

    void HideOverlay()
    {
        if (overlay != null)
        {
            Destroy(overlay);
            overlay = null;
        }
    }

    static void SetPaused(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
        AudioListener.pause = paused;
    }
}
