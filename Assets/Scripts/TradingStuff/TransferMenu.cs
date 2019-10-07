using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TransferMenu : MonoBehaviour
{
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField] protected Transform panel;

    [SerializeField] protected int itemsPerCol;
    [SerializeField] protected Vector2 gridSpacing;

    [SerializeField] protected TransferMenu otherPanel;

    List<TransferUIElement> elements = new List<TransferUIElement>();

    [SerializeField] GameObject nothingToBuy = null;
    protected BusinessType storeIdx;

    private void Awake()
    {
        gridSpacing = new Vector2(gridSpacing.x * ((float)Screen.width / 800), gridSpacing.y * ((float)Screen.height / 600));
    }

    public void setStoreIdx(BusinessType idx)
    {
        storeIdx = idx;
        updateInventoryUI(idx);
    }

    protected abstract List<Item> getItems();
    protected abstract List<int> getCounts();

    protected abstract int initValue(Item item);

    public void updateInventoryUI(BusinessType storeIdx, bool updateY = false)
    {
        for (int i = elements.Count - 1; i >= 0; --i)
        {
            Destroy(elements[i].gameObject);
            elements.RemoveAt(i);
        }
        List<Item> items = getItems();
        List<int> counts = getCounts();
        for (int i = 0; i < items.Count; ++i)
        {
            TransferUIElement transferElement =
                Instantiate(itemPrefab, panel).GetComponent<TransferUIElement>();
            elements.Add(transferElement);
            (transferElement.transform as RectTransform).Translate(new Vector3((i / itemsPerCol) * gridSpacing.x, -(i % itemsPerCol) * gridSpacing.y));
            transferElement.init((this as TransferTo) != null, initValue(items[i]), this, otherPanel, storeIdx);
            transferElement.setItem(items[i], counts[i]);
        }
        nothingToBuy.SetActive(items.Count == 0);
    }
}