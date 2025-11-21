using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int gold;
    public int score;
    public int currentLevel = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        GameUI.Instance.UpdateGold(gold);
    }

    public void AddScore(int amount)
    {
        score += amount;
        GameUI.Instance.UpdateScore(score);
    }

    public void AddLevelScore()
    {
        AddScore(500);
    }

    public void LevelUp()
    {
        currentLevel++;
        GameUI.Instance.UpdateLevel(currentLevel);
    }

    private void RefreshUI()
    {
        GameUI.Instance.UpdateGold(gold);
        GameUI.Instance.UpdateScore(score);
        GameUI.Instance.UpdateLevel(currentLevel);
    }

    public void ResetStats()
    {
        score = 0;
        gold = 0;
        currentLevel = 1;
    }

}
