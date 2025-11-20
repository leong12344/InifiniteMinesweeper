using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public int maxHealth = 3;
    public int currentHealth;
    public int shieldCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Start game with full HP ONCE
            if (currentHealth == 0)
                currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (HeartUI.Instance != null)
            HeartUI.Instance.SetHearts(currentHealth);
        else
            Debug.LogWarning("HeartUI.Instance is NULL – UI did not load yet!");
    }


    public void TakeDamage(int amount = 1)
    {
        if(shieldCount > 0)
        {
            shieldCount--;
            HeartUI.Instance.UpdateHearts(currentHealth, maxHealth, shieldCount);
            return;
        }
        
        currentHealth -= amount;

        if (HeartUI.Instance != null)
            HeartUI.Instance.SetHearts(currentHealth);

        if (currentHealth <= 0)
        {
            GameOverManager.Instance.ShowGameOver(GameStats.Instance.score);
        }

        if (ItemManager.Instance.shieldActive)
        {
            ItemManager.Instance.shieldActive = false;
            return;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        HeartUI.Instance.SetHearts(currentHealth);
    }

    public void Heal(int amount = 1)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        HeartUI.Instance.SetHearts(currentHealth);
    }
}
