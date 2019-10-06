using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unequip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Text infoField = null;
    [SerializeField] protected string myInfo = "";
    [SerializeField] protected string defaultInfo = "";
    [SerializeField] Text nameField = null;
    [SerializeField] Text rareField = null;
    [SerializeField] Text typeField = null;
    
    string myName = "";
    string myRare = "";
    ItemType myType = ItemType.None;

    bool highlighted = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && highlighted)
        {
            Player.PlayerInventory.instance.unequipItem(myType);
            setInfo();
        }
    }

    public void setItem(Item item)
    {
        myInfo = item.description;
        myType = item.type;
        myRare = item.rarity.ToString();
        myName = item.name;
    }
    public void setInfo(string statusInfo = "")
    {
        myInfo = statusInfo == "" ? defaultInfo : statusInfo;
        myName = "";
        myRare = "";
        myType = ItemType.None;
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        highlighted = true;
        infoField.text = myInfo;
        nameField.text = myName;
        rareField.text = myRare;
        typeField.text = myType.ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlighted = false;
        infoField.text = "";
        nameField.text = "";
        rareField.text = "";
        typeField.text = "";
    }
}
