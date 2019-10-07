using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellValueChanged : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] TransferUIElement transferUIElement;
    public void sellValueChanged()
    {
        int text = 0;
        if(int.TryParse(inputField.text, out text))
            BusinessManager.instance.setCost(transferUIElement.getBusinessType() ,transferUIElement.getItem(), text);
    }
}
