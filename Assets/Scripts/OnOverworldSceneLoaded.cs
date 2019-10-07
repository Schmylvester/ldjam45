using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOverworldSceneLoaded : MonoBehaviour
{
    [SerializeField] BoardButtonManager buttonManager = null;
    private void Start()
    {
        sceneLoaded();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            sceneLoaded();
    }

    void sceneLoaded()
    {
        BusinessManager.instance.handleExpenses();
        buttonManager.updateUI();
        StoreInventory.instance.initStoreInventory();
    }
}
