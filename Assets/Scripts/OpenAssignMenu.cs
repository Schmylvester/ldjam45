using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAssignMenu : Sign
{
    [SerializeField] BusinessType businessType;

    protected override void showMessage()
    {
        if (BusinessManager.instance.getData(businessType).owned || BusinessManager.instance.getData(businessType).rented)
        {
            AssignmentManager.instance.setBusiness(businessType);
            AssignmentManager.instance.updateUI();
            AssignmentManager.instance.toggleVisible();
        }
        else
            base.showMessage();
    }
}
