using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    [HideInInspector] public static PlayerInventory instance;

    int m_cash = 0;

    [SerializeField] Text nameField;
    [SerializeField] Text descField;
    [SerializeField] Text rareField;
    [SerializeField] Text typeField;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] Transform inventoryPanel;

    [SerializeField] int itemsPerRow;
    [SerializeField] Vector2 gridSpacing;

    int lastKnownScene = -1;

    public List<Item> m_items { get; } = new List<Item>();
    public List<int> m_counts { get; } = new List<int>();
    private bool visible = true;
    List<GameObject> m_activeChildren = new List<GameObject>();

    Item[] m_equippedItems = new Item[(int)ItemType.EQUIP_COUNT];
    [SerializeField] Image[] m_equipSlots = null;
    [SerializeField] Sprite m_emptyIcon = null;

    Item[] getEquippedItems()
    {
        return (Item[])m_equippedItems.Clone();
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            instance = this;
            toggleInventoryMenu(false);
            gridSpacing = new Vector2(gridSpacing.x * ((float)Screen.width / 800), gridSpacing.y * ((float)Screen.height / 600));
            DontDestroyOnLoad(transform.parent.gameObject);
        }
    }

    private void Start()
    {
        for(int i = 0 ; i < 800; ++i)addItem(ItemDatabase.instance.getRandomItem(false));
        for (int i = 0; i < m_equippedItems.Length; ++i)
        {
            m_equippedItems[i].isNull = true;
        }
        //for (int i = 0; i < 1000; ++i)
            //addItem(ItemDatabase.instance.getRandomItem());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            toggleInventoryMenu(true);
            togglePlayerHPBar(!visible);
        }

        if (SceneManager.GetActiveScene().buildIndex != lastKnownScene)
        {
            lastKnownScene = SceneManager.GetActiveScene().buildIndex;
            sceneLoaded();
        }
    }

    void sceneLoaded()
    {
        PlayerStats stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        foreach (Item item in m_equippedItems)
        {
            if (item.isNull) continue;
            stats.damageModifier += item.damage;
            stats.armourModifier += item.armour;
            stats.maxHealthMod(item.health);
            stats.moveSpeedModifier += item.speed;
            stats.weaponRangeModifier += item.range;

            stats.gameObject.GetComponent<Player>().OnWeaponEquip(item);
        }
        stats.setCurrentHealth(stats.GetActualMaxHealth());
        if (lastKnownScene == 0)
            Game.AudioManager.instance.playMusic(Random.Range(0, 2));
        else
            Game.AudioManager.instance.playMusic(Random.Range(2, 5));
    }

    private void toggleInventoryMenu(bool playSound)
    {
        visible = !visible;
        GameObservables.gamePaused = visible;
        if (visible && playSound)
            SFXManager.instance.PlaySFX("Save", 1);
        else if (playSound)
            SFXManager.instance.PlaySFX("Cancel1", 1);
        updateInventoryUI();
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(visible);
            GetComponent<Image>().enabled = visible;
        }
        inventoryPanel.gameObject.SetActive(visible);
    }

    public void removeAllItemsAndEquipment()
    {
        for (int i = 0; i < (int)ItemType.EQUIP_COUNT; ++i)
        {
            unequipItemType((ItemType)i);
        }

        for (int i = 0; i < m_counts.Count; ++i)
        {
            m_counts[i] = 0;
        }
        m_counts.Clear();
        m_items.Clear();
    }

    public void togglePlayerHPBar(bool to)
    {
        Transform hpBar = transform.parent.Find("PlayerPanel");
        if (hpBar) hpBar.gameObject.SetActive(to);
    }

    public void equipItem(Item item)
    {
        unequipItemType(item.type);
        if (ArrayUtil.arrayContains(item.traits, "Two Handed") != -1)
        {
            unequipItemType(ItemType.Weapon);
            unequipItemType(ItemType.Shield);
        }
        if (item.type == ItemType.Shield && !m_equippedItems[(int)ItemType.Weapon].isNull)
        {
            if (ArrayUtil.arrayContains(m_equippedItems[(int)ItemType.Weapon].traits, "Two Handed") != -1)
                unequipItemType(ItemType.Weapon);
        }
        PlayerStats stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        stats.damageModifier += item.damage;
        stats.armourModifier += item.armour;
        stats.maxHealthMod(item.health);
        stats.moveSpeedModifier += item.speed;
        stats.weaponRangeModifier += item.range;

        stats.gameObject.GetComponent<Player>().OnWeaponEquip(item);

        m_equippedItems[(int)item.type] = item;
        m_equippedItems[(int)item.type].isNull = false;
        m_equipSlots[(int)item.type].sprite = ItemDatabase.instance.getSprite(item.spriteIdx);
        m_equipSlots[(int)item.type].GetComponentInParent<Unequip>().setItem(item);
        removeItem(item.name);
        updateInventoryUI();
        SFXManager.instance.PlaySFX("Equip1", 1);
    }

    public void unequipItemType(ItemType itemType)
    {
        if (!m_equippedItems[(int)itemType].isNull)
        {
            Item item = m_equippedItems[(int)itemType];
            PlayerStats stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            stats.damageModifier -= item.damage;
            stats.armourModifier -= item.armour;
            stats.maxHealthMod(-item.health);
            stats.moveSpeedModifier -= item.speed;
            stats.weaponRangeModifier -= item.range;

            stats.gameObject.GetComponent<Player>().OnWeaponUnequip();

            addItem(m_equippedItems[(int)itemType]);
            m_equippedItems[(int)itemType].isNull = true;
            m_equipSlots[(int)itemType].sprite = m_emptyIcon;
            updateInventoryUI();
        }
    }

    public void updateInventoryUI()
    {
        for (int i = m_activeChildren.Count - 1; i >= 0; --i)
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
        if (item.spriteIdx == 0) return;

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
                    if (m_counts[i] == 0)
                    {
                        m_counts.RemoveAt(i);
                        m_items.RemoveAt(i);
                    }
                    updateInventoryUI();
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

    public int getCash() { return m_cash; }
    public bool changeCash(int by)
    {
        if (m_cash + by < 0)
        {
            return false;
        }
        m_cash += by;
        SFXManager.instance.PlaySFX("Shop", 1);
        return true;
    }
}