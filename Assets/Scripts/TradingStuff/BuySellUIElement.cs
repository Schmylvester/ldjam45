using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuySellUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image = null;
    [SerializeField] Image type = null;
    [SerializeField] Image rarity = null;
    [SerializeField] Text count = null;
    [SerializeField] Text valueField = null;
    [SerializeField] Color[] rarityLevels = null;

    Item item;
    bool selectedItem = false;
    bool belongToPlayer = true;
    int value = 0;
    ShopMenu menu;
    ShopMenu otherMenu;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedItem)
        {
            if (belongToPlayer)
            {
                StoreInventory.instance.addItem(item);
                Player.PlayerInventory.instance.removeItem(item);
                Player.PlayerInventory.instance.changeCash(value);
                menu.updateInventoryUI();
                otherMenu.updateInventoryUI();
            }
            else
            {
                if (Player.PlayerInventory.instance.changeCash(-value))
                {
                    Player.PlayerInventory.instance.addItem(item);
                    StoreInventory.instance.removeItem(item.name);
                    menu.updateInventoryUI();
                    otherMenu.updateInventoryUI();
                }
            }
        }
    }

    public void setItem(Item _item, int _count)
    {
        item = _item;
        image.sprite = ItemDatabase.instance.getSprite(item.spriteIdx);
        count.text = _count.ToString();
        switch (item.type)
        {
            case ItemType.Consumable:
                type.color = Color.green;
                break;
            case ItemType.Weapon:
            case ItemType.Shield:
            case ItemType.Headwear:
            case ItemType.Footwear:
            case ItemType.Gloves:
            case ItemType.Clothing:
                type.color = Color.red;
                break;
            default:
                type.color = Color.clear;
                break;
        }
        if (item.rarity != Rarity.Common)
        {
            rarity.color = rarityLevels[(int)item.rarity];
        }
        else
        {
            rarity.color = Color.clear;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedItem = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedItem = false;
    }

    public void init(bool _belongToPlayer, int _value, ShopMenu _menu, ShopMenu _otherMenu)
    {
        menu = _menu;
        otherMenu = _otherMenu;
        value = _value;
        valueField.text = value.ToString();
        belongToPlayer = _belongToPlayer;
    }
}