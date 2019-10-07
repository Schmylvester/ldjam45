using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferUIElement : MonoBehaviour
{
    [SerializeField] Image image = null;
    [SerializeField] Image type = null;
    [SerializeField] Image rarity = null;
    [SerializeField] Text count = null;
    [SerializeField] Text valueField = null;
    [SerializeField] InputField inputField = null;
    [SerializeField] Color[] rarityLevels = null;

    Item item;
    bool selectedItem = false;
    bool belongToPlayer = true;
    int value = 0;
    TransferMenu menu;
    TransferMenu otherMenu;
    BusinessType storeIdx;

    public Item getItem() { return item; }
    public BusinessType getBusinessType() { return storeIdx; }

    public void clicked()
    {
        if (belongToPlayer)
        {
            if (BusinessManager.instance.getData(storeIdx).itemsForSale.Count < BusinessManager.instance.getData(storeIdx).maxItems)
            {
                PlayerInventory.instance.removeItem(item);
                BusinessManager.instance.getData(storeIdx).itemsForSale.Add(new ItemForSale() { item = item, cost = item.baseValue });
            }
        }
        else
        {
            PlayerInventory.instance.addItem(item);
            for (int i = 0; i < BusinessManager.instance.getData(storeIdx).itemsForSale.Count; ++i)
                if (BusinessManager.instance.getData(storeIdx).itemsForSale[i].item.name == item.name)
                {
                    BusinessManager.instance.getData(storeIdx).itemsForSale.RemoveAt(i);
                    break;
                }
        }
        menu.updateInventoryUI(storeIdx);
        otherMenu.updateInventoryUI(storeIdx, true);
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

    public void init(bool _belongToPlayer, int _value, TransferMenu _menu, TransferMenu _otherMenu, BusinessType _storeIdx)
    {
        menu = _menu;
        otherMenu = _otherMenu;
        value = _value;
        if (valueField)
            valueField.text = value.ToString();
        if (inputField)
            inputField.text = value.ToString();
        belongToPlayer = _belongToPlayer;
        storeIdx = _storeIdx;
    }
}