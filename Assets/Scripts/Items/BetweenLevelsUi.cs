using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BetweenLevelsUI : MonoBehaviour
{
    public static BetweenLevelsUI Instance;

    public GameObject panel;
    public ItemData[] allItems;
    public ItemButton[] itemButtons;

    public TextMeshProUGUI goldText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateGoldUI();
    }

    public void ShowItemSelection()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);
        UpdateGoldUI();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            ItemData randomItem = allItems[Random.Range(0, allItems.Length)];
            itemButtons[i].Setup(randomItem);
        }
    }

    public void UpdateGoldUI()
    {
        if (goldText == null) return;

        goldText.text = GameStats.Instance != null ?
            GameStats.Instance.gold.ToString() : "0";
    }

    public void TryPurchase(ItemData item)
    {
        if (GameStats.Instance.gold < item.cost)
        {
            Debug.Log("Not enough gold!");
            return;
        }


        GameStats.Instance.AddGold(-item.cost);
        ItemManager.Instance.ApplyItem(item);

        CloseShop();
    }

    private void CloseShop()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        Game.Instance.NextLevel();
    }


    public void Done()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        Game.Instance.NextLevel();
    }
}
