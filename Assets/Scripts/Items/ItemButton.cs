using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Image icon;

    private ItemData currentItem;

    public void Setup(ItemData item)
    {
        currentItem = item;
        nameText.text = item.itemName;
        costText.text = item.cost + "";
        icon.sprite = item.icon;
    }

    public void Buy()
    {
        BetweenLevelsUI.Instance.TryPurchase(currentItem);
    }
}
