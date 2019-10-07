using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBusinessUI : MonoBehaviour
{
    [SerializeField] GameObject businessUI;
    bool inTrigger = false;

    private void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            toggleMenu();
        }
    }

    public void toggleMenu()
    {
        if(!businessUI) businessUI = PlayerInventory.instance.transform.parent.GetChild(2).gameObject;
        businessUI.SetActive(!businessUI.activeSelf);
        GameObservables.gamePaused = businessUI.activeSelf;
        PlayerInventory.instance.togglePlayerHPBar(!businessUI.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = false;
    }
}
