using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int cost;

    public enum ItemType { Heal, Shield, RevealBomb, ScoreMulti, GoldMulti };
    public ItemType type;

    // Extra value: heal amount, multiplier amount, etc.
    public int value;
}
