using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentManager : MonoBehaviour
{
    public static AssignmentManager instance = null;
    [SerializeField] BusinessType business;
    [SerializeField] Text[] texts = null;
    [SerializeField] Button[] buttons = null;
    [SerializeField] TransferMenu[] transferMenus;
    BusinessManager b = null;
    bool isVisible = false;

    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else { instance = this; DontDestroyOnLoad(gameObject); }
    }

    public void setBusiness(BusinessType to) { business = to; }

    public void toggleVisible()
    {
        isVisible = !isVisible;
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObservables.gamePaused = isVisible;
            transform.GetChild(i).gameObject.SetActive(isVisible);
        }
        PlayerInventory.instance.togglePlayerHPBar(!isVisible);
    }

    public void staffAssign(int id)
    {
        HireeType type = HireeType.COUNT;
        switch (Mathf.Abs(id))
        {
            case 1: type = HireeType.Sales; break;
            case 2: type = HireeType.Guard; break;
            case 3: type = HireeType.Promoter; break;
            default: break;
        }

        if (id > 0)
        {
            b.getData(business).workersAssigned.Add(type);
            b.changeUnassigned(type, -1);
        }
        else if (id < 0)
        {
            b.getData(business).workersAssigned.Remove(type);
            b.changeUnassigned(type, 1);
        }
        updateUI();
    }

    public void updateUI()
    {
        if (b == null)
        {
            b = BusinessManager.instance;
        }
        texts[0].text = b.getData(HireeType.Sales).numberUnassigned.ToString();
        texts[1].text = b.getData(HireeType.Guard).numberUnassigned.ToString();
        texts[2].text = b.getData(HireeType.Promoter).numberUnassigned.ToString();

        texts[3].text = countAssignedStaff(HireeType.Sales).ToString();
        texts[4].text = countAssignedStaff(HireeType.Guard).ToString();
        texts[5].text = countAssignedStaff(HireeType.Promoter).ToString();

        buttons[0].interactable = getAssignButtonEnabled(HireeType.Sales);
        buttons[1].interactable = getAssignButtonEnabled(HireeType.Guard);
        buttons[2].interactable = getAssignButtonEnabled(HireeType.Promoter);
        buttons[3].interactable = getUnassignButtonEnabled(HireeType.Sales);
        buttons[4].interactable = getUnassignButtonEnabled(HireeType.Guard);
        buttons[5].interactable = getUnassignButtonEnabled(HireeType.Promoter);

        foreach (TransferMenu transferMenu in transferMenus)
            transferMenu.updateInventoryUI(business);
    }

    int countAssignedStaff(HireeType type)
    {
        int count = 0;
        foreach (HireeType _type in b.getData(business).workersAssigned)
        {
            if (_type == type)
                count++;
        }
        return count;
    }

    bool getAssignButtonEnabled(HireeType type)
    {
        if (b.getData(type).numberUnassigned <= 0)
            return false;
        return (b.getData(business).maxWorkers > BusinessManager.instance.getData(business).workersAssigned.Count);
    }

    bool getUnassignButtonEnabled(HireeType type)
    {
        return countAssignedStaff(type) > 0;
    }
}
