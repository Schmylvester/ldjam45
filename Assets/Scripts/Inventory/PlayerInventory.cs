using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [HideInInspector] public static PlayerInventory instance;

        [SerializeField] Text nameField;
        [SerializeField] Text descField;
        [SerializeField] Text rareField;
        [SerializeField] Text typeField;
        [SerializeField] GameObject inventoryItemPrefab;
        [SerializeField] Transform inventoryPanel;

        [SerializeField] int itemsPerRow;
        [SerializeField] Vector2 gridSpacing;

        List<Item> m_items = new List<Item>();
        List<int> m_counts = new List<int>();
        private bool visible = true;
        List<GameObject> m_activeChildren = new List<GameObject>();

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
            toggleInventoryMenu();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                toggleInventoryMenu();
            }
        }

        private void toggleInventoryMenu()
        {
            visible = !visible;
            populateInventory();
            for(int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(visible);
                GetComponent<Image>().enabled = visible;
            }
        }

        public void populateInventory()
        {
            for(int i = m_activeChildren.Count - 1; i >= 0; --i)
            {
                Destroy(m_activeChildren[i]);
            }
            for (int i = 0; i < m_items.Count; ++i)
            {
                InventroryUIElement inventroryUIElement =
                    Instantiate(inventoryItemPrefab, inventoryPanel).GetComponent<InventroryUIElement>();
                m_activeChildren.Add(inventroryUIElement.gameObject);

                (inventroryUIElement.transform as RectTransform).Translate(new Vector3((i % itemsPerRow) * gridSpacing.x, -(i / itemsPerRow) * gridSpacing.y));

                inventroryUIElement.populateFields(nameField, descField, rareField, typeField);
                inventroryUIElement.setItem(m_items[i], m_counts[i]);
            }
        }

        public void addItem(Item item, int count = 1)
        {
            for (int i = 0; i < m_items.Count; ++i)
            {
                if (m_items[i].name == item.name)
                {
                    m_counts[i] += count;
                    return;
                }
            }
            m_items.Add(item);
            m_counts.Add(count);
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="item">Which item</param>
        /// <param name="count">How many</param>
        /// <returns>Whether the item was successfully removed</returns>
        public bool removeItem(string item, int count = 1)
        {
            for (int i = 0; i < m_items.Count; ++i)
            {
                if (m_items[i].name == item)
                {
                    if (count <= m_counts[i])
                    {
                        m_counts[i] -= count;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="item">Which item</param>
        /// <param name="count">How many</param>
        /// <returns>Whether the item was successfully removed</returns>
        public bool removeItem(Item item, int count = 1)
        {
            return removeItem(item.name, count);
        }
    }
}