using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public TextMeshProUGUI scoreText;
    public static GameOverManager Instance;

    private void Awake()
    {
        GameOverPanel.SetActive(false);
        Instance = this;
    }

    public void ShowGameOver(int finalScore)
    {
        scoreText.text = "Score:" + finalScore;
        GameOverPanel.SetActive (true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerHealth.Instance.ResetHealth();
        GameStats.Instance.ResetStats();
        ItemManager.Instance.ResetItems();

        if (HeartUI.Instance != null) { 
            HeartUI.Instance.SetHearts(PlayerHealth.Instance.currentHealth);
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
