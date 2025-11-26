using UnityEngine;

public class GameGuide : MonoBehaviour
{
    public static GameGuide Instance;

    public GameObject guidePanel;

    private void Awake()
    {
        Instance = this;
        guidePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SkipGuide()
    {
        guidePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
