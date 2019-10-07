using UnityEngine;
using UnityEngine.UI;
/*
•	Assigning items to stores
•	Assigning staff to stores

•	Sound

 */
public class BoardButtonManager : MonoBehaviour
{
    [SerializeField] Button rentStallButton     = null;
    [SerializeField] Button buyStallButton      = null;
    [SerializeField] Button buySmallShopButton  = null;
    [SerializeField] Button buyLargeShopButton  = null;

    [SerializeField] Button[] fireButtons       = null;
    [SerializeField] Text[] countTexts          = null;

    [SerializeField] Text moneyText             = null;

    public void rentStall()
    {
        BusinessManager b = BusinessManager.instance;
        BusinessData d = b.getData(BusinessType.Stall);
        if (!d.rented)
        {
            if (attemptBuy(d.rentCost))
            {
                b.setOwned(BusinessType.Stall, 1);
                rentStallButton.interactable = false;
            }
        }
        else
        {
            b.setOwned(BusinessType.Stall, 0);
            rentStallButton.GetComponentInChildren<Text>().text = "Rent £200/day";
        }
    }

    public void buyStall()
    {
        BusinessManager b = BusinessManager.instance;
        if (attemptBuy(b.getData(BusinessType.Stall).buyCost))
        {
            b.setOwned(BusinessType.Stall, 2);
            rentStallButton.interactable = false;
            buyStallButton.interactable = false;
        }
    }

    public void buySmallShop()
    {
        BusinessManager b = BusinessManager.instance;
        if (attemptBuy(b.getData(BusinessType.LittleShop).buyCost))
        {
            b.setOwned(BusinessType.LittleShop, 2);
            buySmallShopButton.interactable = false;
        }
    }

    public void buyLargeShop()
    {
        BusinessManager b = BusinessManager.instance;
        if (attemptBuy(b.getData(BusinessType.BigShop).buyCost))
        {
            b.setOwned(BusinessType.BigShop, 2);
            buyLargeShopButton.interactable = false;
        }
    }

    public void hireWorker(int idx)
    {
        if(PlayerInventory.instance.changeCash(-(BusinessManager.instance.getData((HireeType)idx).cost)))
        {
            countTexts[idx].text = (int.Parse(countTexts[idx].text) + 1).ToString();
            fireButtons[idx].interactable = true;
            BusinessManager.instance.changeHired((HireeType)idx, 1);
        }
    }

    public void fireWorker(int idx)
    {
        countTexts[idx].text = (int.Parse(countTexts[idx].text) - 1).ToString();
        BusinessManager.instance.changeHired((HireeType)idx, -1);
        if(countTexts[idx].text == "0")
        {
            fireButtons[idx].interactable = false;
        }
    }

    bool attemptBuy(int cost)
    {
        return PlayerInventory.instance.changeCash(-cost);
    }

    public void updateUI()
    {
        rentStallButton.GetComponentInChildren<Text>().text
            = BusinessManager.instance.getData(BusinessType.Stall).rented ? "Cancel Rental" : "Rent £200/day";
        rentStallButton.interactable    = !BusinessManager.instance.getData(BusinessType.Stall).owned;
        buyStallButton.interactable     = !BusinessManager.instance.getData(BusinessType.Stall).owned;
        buySmallShopButton.interactable = !BusinessManager.instance.getData(BusinessType.LittleShop).owned;
        buyLargeShopButton.interactable = !BusinessManager.instance.getData(BusinessType.BigShop).owned;

        for(int i = 0; i < countTexts.Length; ++i)
            countTexts[i].text = BusinessManager.instance.getData((HireeType)i).numberHired.ToString();
        for(int i = 0; i < countTexts.Length; ++i)
            fireButtons[i].interactable = !(countTexts[i].text == "0");

        moneyText.text = "£" + PlayerInventory.instance.getCash();
    }
}