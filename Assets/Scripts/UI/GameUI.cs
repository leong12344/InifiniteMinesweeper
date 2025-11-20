using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public TextMeshProUGUI GoldNum;
    public TextMeshProUGUI ScoreNum;
    public TextMeshProUGUI LevelNum;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateGold(int value)
    {
        GoldNum.text = value.ToString();
    }

    public void UpdateScore(int value)
    {
        ScoreNum.text = value.ToString();
    }
    public void UpdateLevel(int value)
    {
        LevelNum.text = value.ToString();
    }
}
