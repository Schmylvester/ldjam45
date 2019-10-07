using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventroryUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image = null;
    [SerializeField] Image type = null;
    [SerializeField] Image rarity = null;
    [SerializeField] Text count = null;
    [SerializeField] Color[] rarityLevels = null;

    Text nameField = null;
    Text descriptionField = null;
    Text rarityField = null;
    Text typeField = null;

    Item item;
    bool selectedItem = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedItem)
        {
            if (item.type >= 0)
            {
                PlayerInventory.instance.equipItem(item);
            }
            else if (item.type == ItemType.Consumable)
            {
                PlayerStats stats = GameObject.Find("Player").GetComponent<PlayerStats>();
                stats.changeCurrentHealth(item.health);
                PlayerInventory.instance.removeItem(item);
                SFXManager.instance.PlaySFX("Up1", 1);
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
        nameField.text = item.name;
        descriptionField.text = item.description;
        rarityField.text = item.rarity.ToString();
        if (item.type != ItemType.None)
        {
            typeField.text = item.type.ToString();
        }
        rarityField.color = rarityLevels[(int)item.rarity];
        switch (item.type)
        {
            case ItemType.Consumable:
                typeField.color = Color.green;
                break;
            case ItemType.Weapon:
            case ItemType.Shield:
            case ItemType.Headwear:
            case ItemType.Footwear:
            case ItemType.Gloves:
            case ItemType.Clothing:
                typeField.color = Color.red;
                break;
            default:
                typeField.color = Color.black;
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedItem = false;
        nameField.text = "";
        descriptionField.text = "";
        rarityField.text = "";
        typeField.text = "";
    }

    public void populateFields(Text _name, Text _desc, Text _rarity, Text _type)
    {
        nameField = _name;
        descriptionField = _desc;
        rarityField = _rarity;
        typeField = _type;
    }
}
