using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOverworldSceneLoaded : MonoBehaviour
{
    [SerializeField] BoardButtonManager buttonManager = null;
    private void Start()
    {
        BusinessManager.instance.handleExpenses();
        buttonManager.updateUI();
        StoreInventory.instance.initStoreInventory();
    }
}
