using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        gameClearPanel = GameObject.Find("GameClearPanel");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (gameClearPanel != null)
            gameClearPanel.SetActive(false);
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map_1");
    }
    public void ExitGame()
    {
        Debug.Log("게임 종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogWarning("GameOverPanel을 찾을 수 없습니다.");
    }

    public void GameClear()
    {
        Time.timeScale = 0f;
        if (gameClearPanel != null)
            gameClearPanel.SetActive(true);
        else
            Debug.LogWarning("GameClearPanel을 찾을 수 없습니다.");
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScreen");
    }
}
