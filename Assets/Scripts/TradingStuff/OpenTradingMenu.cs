using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTradingMenu : MonoBehaviour
{
    [SerializeField] GameObject tradingMenu = null;
    [SerializeField] int storeIdx;
    bool playerInTrigger = false;
    bool menuActive = false;

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            menuActive = !menuActive;
            if(!tradingMenu) tradingMenu = PlayerInventory.instance.transform.parent.GetChild(3).gameObject;
            tradingMenu.SetActive(menuActive);
            tradingMenu.GetComponentsInChildren<ShopMenu>()[0].setStoreIdx(storeIdx);
            tradingMenu.GetComponentsInChildren<ShopMenu>()[1].setStoreIdx(storeIdx);
            GameObservables.gamePaused = menuActive;
            PlayerInventory.instance.togglePlayerHPBar(!menuActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInTrigger = false;
    }
}
