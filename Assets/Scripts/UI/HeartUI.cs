using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public static HeartUI Instance;

    public Image[] hearts;         
    public Sprite fullHeartSprite;  
    public Sprite emptyHeartSprite;

    public Image[] shields;
    public Sprite fullShieldSprite;
    public Sprite emptyShieldSprite;

    private void Awake()
    {
        Instance = this;
    }

    public void SetHearts(int currentHealth)
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i<currentHealth)
                hearts[i].sprite = fullHeartSprite;
            else
                hearts[i].sprite = emptyHeartSprite;
        }
    }

    public void SetShields(int currentShields)
    {
        for(int i = 0;i<shields.Length; i++)
        {
            if (i < currentShields)
            {
                shields[i].sprite = fullShieldSprite;
            }
            else
            {
                shields[i].sprite = emptyShieldSprite;
            }
        }
    }

    public void UpdateHearts(int currentHealth, int maxHealth, int currentShields)
    {
        for(int i =0;i<hearts.Length;i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }

            hearts[i].enabled = i < maxHealth;
        }
        for (int i = 0; i < shields.Length; i++)
        {
            shields[i].sprite = (i < currentShields) ? fullShieldSprite : emptyShieldSprite;
        }
    }

}
