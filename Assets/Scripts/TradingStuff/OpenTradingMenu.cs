using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTradingMenu : MonoBehaviour
{
    [SerializeField] GameObject tradingMenu = null;
    bool playerInTrigger = false;
    bool menuActive = false;

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            foreach (MenuScroll menu in tradingMenu.GetComponentsInChildren<MenuScroll>())
                menu.reset();
            menuActive = !menuActive;
            tradingMenu.SetActive(menuActive);
            GameObservables.gamePaused = menuActive;
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
