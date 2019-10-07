using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopMenu : MonoBehaviour
{
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField] protected Transform panel;

    [SerializeField] protected int itemsPerRow;
    [SerializeField] protected Vector2 gridSpacing;

    [SerializeField] protected Text playerMoney = null;
    [SerializeField] protected ShopMenu otherPanel;

    List<BuySellUIElement> elements = new List<BuySellUIElement>();

    [SerializeField] GameObject nothingToBuy = null;
    protected int storeIdx;

    private void Awake()
    {
        gridSpacing = new Vector2(gridSpacing.x * ((float)Screen.width / 800), gridSpacing.y * ((float)Screen.height / 600));
    }

    public void setStoreIdx(int idx)
    {
        storeIdx = idx;
        updateInventoryUI(idx);
    }

    protected abstract List<Item> getItems();
    protected abstract List<int> getCounts();

    protected abstract int initValue(Item item);

    public void updateInventoryUI(int storeIdx, bool updateY = false)
    {
        if(updateY)panel.GetChild(0).GetComponent<MenuScroll>().reset();
        for (int i = elements.Count - 1; i >= 0; --i)
        {
            Destroy(elements[i].gameObject);
            elements.RemoveAt(i);
        }
        List<Item> items = getItems();
        List<int> counts = getCounts();
        for (int i = 0; i < items.Count; ++i)
        {
            BuySellUIElement buySellElement =
                Instantiate(itemPrefab, panel.GetChild(0)).GetComponent<BuySellUIElement>();
            elements.Add(buySellElement);
            (buySellElement.transform as RectTransform).Translate(new Vector3((i % itemsPerRow) * gridSpacing.x, -(i / itemsPerRow) * gridSpacing.y));
            buySellElement.init((this as SellMenu) != null, initValue(items[i]), this, otherPanel, storeIdx);
            buySellElement.setItem(items[i], counts[i]);
        }
        nothingToBuy.SetActive(items.Count == 0);
        if (playerMoney)
        {
            playerMoney.text = "£" + PlayerInventory.instance.getCash();
        }
    }
}