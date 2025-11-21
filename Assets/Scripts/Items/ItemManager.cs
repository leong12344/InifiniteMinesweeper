using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public bool shieldActive = false;
    public float scoreMulti = 1f;
    public float goldMulti = 1f;
    public int shieldCount = 0;
    public int maxShields = 3;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyItem(ItemData item)
    {
        switch (item.type)
        {
            case ItemData.ItemType.Heal:
                PlayerHealth.Instance.Heal(1);
                break;
            case ItemData.ItemType.Shield:
                PlayerHealth.Instance.shieldCount++;
                HeartUI.Instance.UpdateHearts(
                    PlayerHealth.Instance.currentHealth,
                    PlayerHealth.Instance.maxHealth,
                    PlayerHealth.Instance.shieldCount
                );
                break;
            case ItemData.ItemType.RevealBomb:
                Game.Instance.RevealRandomMine();
                break;
            case ItemData.ItemType.ScoreMulti:
                scoreMulti = 2f;
                break;
            case ItemData.ItemType.GoldMulti:
                goldMulti = 2f;
                break;
        }
    }

    public void AddShield()
    {
        shieldCount = Mathf.Clamp(shieldCount+1,0,maxShields);
        HeartUI.Instance.SetShields(shieldCount);
    }

    public void ResetTempororyItems()
    {
        shieldActive = false;
        scoreMulti = 1f;
        goldMulti = 1f;
    }

    public void ResetItems()
    {
        shieldActive = false;
        scoreMulti = 1f;
        goldMulti = 1f;
    }

}
