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
        HeartUI.Instance?.UpdateHearts(currentHealth, maxHealth, shieldCount);
        CatAnimationController.Instance?.UpdateHearts(currentHealth);
    }


    public void TakeDamage(int amount = 1)
    {
        if (shieldCount > 0)
        {
            shieldCount--;

            HeartUI.Instance.UpdateHearts(currentHealth, maxHealth, shieldCount);
            CatAnimationController.Instance.PlayDamageAnimation(); 
            return;
        }
        
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth > 0)
            CatAnimationController.Instance.PlayDamageAnimation();

        HeartUI.Instance.UpdateHearts(currentHealth, maxHealth, shieldCount);

        CatAnimationController.Instance.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            CatAnimationController.Instance.PlayDeathAnimation();
            GameOverManager.Instance.ShowGameOver(GameStats.Instance.score);
        }
    }


    public void Heal(int amount = 1)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        HeartUI.Instance.UpdateHearts(currentHealth, maxHealth, shieldCount);
        CatAnimationController.Instance.UpdateHearts(currentHealth);
    }


    public void ResetHealth()
    {
        currentHealth = maxHealth;
        shieldCount = 0;

        HeartUI.Instance.UpdateHearts(currentHealth, maxHealth, shieldCount);
        CatAnimationController.Instance.UpdateHearts(currentHealth);
    }

    public void AddShield()
    {
        shieldCount = Mathf.Clamp(shieldCount + 1, 0, 3);

        HeartUI.Instance.UpdateHearts(currentHealth, maxHealth, shieldCount);
    }
}
