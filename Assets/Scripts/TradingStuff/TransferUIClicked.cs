using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransferUIClicked : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TransferUIElement element;
    bool selectedItem = false;

    void Update()
    {
        if(selectedItem && Input.GetMouseButtonDown(0))
            element.clicked();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedItem = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedItem = false;
    }
}
