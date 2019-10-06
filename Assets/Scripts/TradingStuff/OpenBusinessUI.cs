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
        businessUI.SetActive(!businessUI.activeSelf);
        GameObservables.gamePaused = businessUI.activeSelf;
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
