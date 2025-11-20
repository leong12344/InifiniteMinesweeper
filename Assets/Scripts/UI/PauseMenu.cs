using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject[] disabled;

    public bool isPaused = false;


    private void Awake()
    {
        Instance = this;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
                return;
            }

            if (BetweenLevelsUI.Instance != null && BetweenLevelsUI.Instance.panel.activeSelf)
                return;

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSettings()
    {
        foreach(var b in disabled)
        {
            b.SetActive(false);
        }
        settingsPanel.SetActive(true);
        
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        foreach (var b in disabled)
            b.SetActive(true);

    }


    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
