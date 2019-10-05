﻿using System.Collections;
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

    public void setItem(Item _item, int _count)
    {
        item = _item;
        image.sprite = item.sprite;
        count.text = _count.ToString();
        switch(item.type)
        {
            case ItemType.Consumable:
                type.color = Color.green;
                break;
            case ItemType.Equippable:
                type.color = Color.red;
                break;
            default:
                type.color = Color.clear;
                break;
        }
        if(item.rarity != Rarity.Common)
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
        if(item.sprite != null)
        {
            nameField.text = item.name;
            descriptionField.text = item.description;
            rarityField.text = item.rarity.ToString();
            if(item.type != ItemType.None)
            {
                typeField.text = item.type.ToString();
            }
            rarityField.color = rarityLevels[(int)item.rarity];
            switch(item.type)
            {
                case ItemType.Consumable:
                    typeField.color = Color.green;
                    break;
                case ItemType.Equippable:
                    typeField.color = Color.red;
                    break;
                default:
                    typeField.color = Color.black;
                    break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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