using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Text infoField = null;
    [SerializeField] protected string myInfo = "";
    [SerializeField] protected string defaultInfo = "";

    public void setInfo(string statusInfo = "")
    {
        myInfo = statusInfo == "" ? defaultInfo : statusInfo;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoField.text = myInfo;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoField.text = "";
    }

}
